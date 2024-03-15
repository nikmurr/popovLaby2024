using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PopovLaba3
{
    public static class DataGridHelper
    {
        public static void SetDataSource(DatabaseHelper databaseHelper, DataGridView dataGridView)
        {
            DataTable dataTable = databaseHelper.ShowUserRecords();
            DataColumn column = new DataColumn("Ред.", typeof(bool));
            column.Expression = "IIF(is_edited = 1, True, False)";
            dataTable.Columns.Add(column);

            dataTable.Columns["id"].ColumnName = "ID";
            dataTable.Columns["text"].ColumnName = "Текст";
            dataTable.Columns["record_time"].ColumnName = "Время записи";

            dataGridView.DataSource = dataTable;

            dataGridView.Columns["is_edited"].Visible = false;
            dataGridView.Columns["ID"].FillWeight = 1;
            dataGridView.Columns["Текст"].FillWeight = 8;
            dataGridView.Columns["Время записи"].FillWeight = 2;
            dataGridView.Columns["Ред."].FillWeight = 1;
        }

        public static void AddRecord(DatabaseHelper databaseHelper, DataGridView dataGridView, string newText)
        {
            try
            {
                databaseHelper.InsertUserRecords(newText);
                SetDataSource(databaseHelper, dataGridView);
            }
            catch (Exception) { }
        }

        public static void EditRecord(DatabaseHelper databaseHelper, DataGridView dataGridView, List<Font> fontList)
        {
            try
            {

                if (dataGridView.SelectedRows.Count == 1)
                {
                    DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                    int selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    if (selectedId > 0)
                    {
                        Form4 form4 = new Form4(fontList);

                        if (form4.ShowDialog() == DialogResult.OK)
                        {
                            string enteredText = form4.EnteredText;
                            databaseHelper.EditUserRecord(selectedId, enteredText);
                            SetDataSource(databaseHelper, dataGridView);
                        } 
                    }
                    else
                    {
                        MessageBox.Show("Выберите хотя бы одну строку для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (dataGridView.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Пожалуйста, выделите только одну строку для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Выберите хотя бы одну строку для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception) { }
        }

        public static void DeleteRecords(DatabaseHelper databaseHelper, DataGridView dataGridView)
        {
            try
            {
                List<int> selectedIds = new List<int>();

                if (dataGridView.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
                    {
                        int id = Convert.ToInt32(selectedRow.Cells["id"].Value);
                        selectedIds.Add(id);
                    }

                    databaseHelper.DeleteUserRecords(selectedIds);
                    SetDataSource(databaseHelper, dataGridView);
                }
                else
                {
                    MessageBox.Show("Выберите хотя бы одну строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception) { }
        }
    }
}
