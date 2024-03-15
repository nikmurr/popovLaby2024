using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PopovLaba3
{

    public class DatabaseHelper
    {
        private readonly string connectionString;
        private readonly string dbPath;
        private SQLiteConnection connection;
        private bool permission = false;
        public long internalDifference = 0;
        public string internalLogin = "user";
        public int internalUserID = 0;

        public DatabaseHelper()
        {
            dbPath = FileHelper.GetDB("users.db");
            connectionString = $"Data Source={dbPath};Version=3;";
            connection = new SQLiteConnection(connectionString);
            CreateTable();
        }

        public void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public void CreateTable ()
        {
            try
            {
                OpenConnection();

                string query = @"
                CREATE TABLE IF NOT EXISTS users (
	            id INTEGER PRIMARY KEY AUTOINCREMENT,
	            login TEXT UNIQUE NOT NULL,
	            password_hashed CHAR(64) NOT NULL,
                attempts INTEGER DEFAULT 0,
                if_blocked INTEGER DEFAULT 0,
                blocked_time INTEGER DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS records (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER,
                text TEXT,
                record_time TEXT,
                is_edited INTEGER DEFAULT 0,
                FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
                );
            ";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int InsertUser(string username, string password)
        {
            try
            {
                OpenConnection();
                string insertUserSql = @"
                    INSERT INTO users (login, password_hashed)
                    VALUES (@Username, @Password);
                ";

                string password_hashed = Hasher.Hash(password);

                using (var command = new SQLiteCommand(insertUserSql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password_hashed);
                    command.ExecuteNonQuery();
                }

                CloseConnection();
                return 0;
            }
            catch (SQLiteException ex)
            {
                if ((int)ex.ErrorCode == (int)SQLiteErrorCode.Constraint && ex.Message.Contains("UNIQUE"))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                return 3;
            }
            finally 
            { 
                CloseConnection(); 
            };
        }

        public int CheckUser(string login, string password)
        {
            try
            {
                OpenConnection();
                string hashedPassword = Hasher.Hash(password);
                string checkPasswordQuery = "SELECT id, password_hashed, attempts, if_blocked, blocked_time FROM users WHERE login = @login";

                using (SQLiteCommand passwordCommand = new SQLiteCommand(checkPasswordQuery, connection))
                {
                    passwordCommand.Parameters.AddWithValue("@login", login);

                    using (SQLiteDataReader reader = passwordCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string storedHashedPassword = reader["password_hashed"].ToString();
                            int attempts = Convert.ToInt32(reader["attempts"]);
                            bool ifBlocked = Convert.ToInt32(reader["if_blocked"]) != 0;
                            long blockedTime = Convert.ToInt64(reader["blocked_time"]);

                            if (ifBlocked)
                            {
                                DateTime currentTime = DateTime.Now;
                                DateTime utcNow = currentTime.ToUniversalTime();
                                long unixTime = (long)(utcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                long difference = unixTime - blockedTime;

                                if (difference > 180 || permission == true)
                                {
                                    internalDifference = 0;
                                    try
                                    {
                                        string resetAttemptsQuery = "UPDATE users SET attempts = 0, if_blocked = 0, blocked_time = 0 WHERE id = @id";

                                        using (SQLiteCommand resetAttemptsCommand = new SQLiteCommand(resetAttemptsQuery, connection))
                                        {
                                            resetAttemptsCommand.Parameters.AddWithValue("@id", id);
                                            resetAttemptsCommand.ExecuteNonQuery();
                                        }

                                        attempts = 0;
                                        ifBlocked = false;
                                        blockedTime = 0;
                                    }
                                    catch (Exception) { }
                                }
                                else
                                {
                                    internalDifference = difference;
                                    return 3;
                                }
                            }

                            if (storedHashedPassword != null && hashedPassword == storedHashedPassword)
                            {
                                string setAttemptsAndBlockedQuery = "UPDATE users SET attempts = 0, if_blocked = 0, blocked_time = 0 WHERE id = @id";
                                using (SQLiteCommand setAttemptsAndBlockedCommand = new SQLiteCommand(setAttemptsAndBlockedQuery, connection))
                                {
                                    setAttemptsAndBlockedCommand.Parameters.AddWithValue("@id", id);
                                    setAttemptsAndBlockedCommand.ExecuteNonQuery();
                                }
                                internalLogin = login;
                                internalUserID = id;
                                return 0;
                            }
                            else
                            {
                                int newAttempts = attempts++;
                                bool newIfBlocked = ifBlocked;
                                long newBlockedTime = 0;
                                if (newAttempts >= 4)
                                {
                                    DateTime currentTime = DateTime.Now;
                                    DateTime utcNow = currentTime.ToUniversalTime();
                                    long unixTime = (long)(utcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                                    newIfBlocked = true;
                                    newBlockedTime = unixTime;
                                }
                                string setAttemptsAndBlockedQuery = "UPDATE users SET attempts = @attempts, if_blocked = @if_blocked, blocked_time = @blocked_time WHERE id = @id";
                                using (SQLiteCommand setAttemptsAndBlockedCommand = new SQLiteCommand(setAttemptsAndBlockedQuery, connection))
                                {
                                    setAttemptsAndBlockedCommand.Parameters.AddWithValue("@attempts", attempts);
                                    setAttemptsAndBlockedCommand.Parameters.AddWithValue("@if_blocked", newIfBlocked);
                                    setAttemptsAndBlockedCommand.Parameters.AddWithValue("@blocked_time", newBlockedTime);
                                    setAttemptsAndBlockedCommand.Parameters.AddWithValue("@id", id);
                                    setAttemptsAndBlockedCommand.ExecuteNonQuery();
                                }
                                return 2;
                            }
                        }
                        else
                        {
                            // Пользователь с указанным логином не найден
                            return 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 5;
            }
            finally
            {
                permission = false;
                CloseConnection();
            }
        }

        public void SetPermission(bool permission)
        {
            this.permission = permission;
        }

        public DataTable ShowUserRecords()
        {
            DataTable dataTable = new DataTable();

            try
            {
                OpenConnection();

                // Получаем все записи для текущего пользователя
                string query = "SELECT id, text, record_time, is_edited FROM records WHERE user_id = @user_id;";
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection))
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@user_id", internalUserID);
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка доступа к БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }

        public void InsertUserRecords(string userText)
        {
            try
            {
                OpenConnection();

                string insertUserSql = @"
                    INSERT INTO records (user_id, text, record_time)
                    VALUES (@User, @Text, @RecordTime);
                ";

                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                using (var command = new SQLiteCommand(insertUserSql, connection))
                {
                    command.Parameters.AddWithValue("@User", internalUserID);
                    command.Parameters.AddWithValue("@Text", userText);
                    command.Parameters.AddWithValue("@RecordTime", currentDateTime);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"Запись добавлена.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка добавления в БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка. Перезапустите приложение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            };
        }

        public void EditUserRecord(int recordId, string newText)
        {
            try
            {
                OpenConnection();

                string query = "UPDATE records SET text = @text, is_edited = 1 WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@text", newText);
                    command.Parameters.AddWithValue("@id", recordId);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"Запись отредактирована.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка добавления в БД", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка. Перезапустите приложение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            };
        }

        public bool DeleteUserRecords(List<int> recordIds)
        {
            try
            {
                OpenConnection();

                // Удаляем записи по списку id
                string query = "DELETE FROM records WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    foreach (int recordId in recordIds)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", recordId);
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления записей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
