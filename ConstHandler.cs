using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopovLaba3
{
    public static class ConstHandler
    {
        public static bool CheckLoginAndPassword(string login, string password, out string errorMessage)
        {
            bool isLoginCorrect = false;
            bool isPasswordCorrect = false;
            string checkLoginErrorMessage = "";
            string checkPasswordErrorMessage = "";

            switch (CheckLogin(login))
            {
                case 0: 
                    isLoginCorrect = true; 
                    break;
                case 1: 
                    isLoginCorrect = false; 
                    checkLoginErrorMessage = "Логин может содержать только латиницу и цифры от 0 до 9."; 
                    break;
                case 2: 
                    isLoginCorrect = false; 
                    checkLoginErrorMessage = "Логин должен содержать от 4 до 32 символов."; 
                    break;
                default:
                    isLoginCorrect = false; checkLoginErrorMessage = "Произошла неизвестная ошибка при обработке ввода."; 
                    break;
            }

            switch (CheckPassword(password))
            {
                case 0:
                    isPasswordCorrect = true;
                    break;
                case 1:
                    isPasswordCorrect = false;
                    checkPasswordErrorMessage = "Пароль может содержать только латиницу, цифры от 0 до 9 и специальные символы.";
                    break;
                case 2:
                    isPasswordCorrect = false;
                    checkPasswordErrorMessage = "Пароль должен содержать от 8 до 32 символов.";
                    break;
                default:
                    isPasswordCorrect = false; checkPasswordErrorMessage = "Произошла неизвестная ошибка при обработке ввода.";
                    break;
            }

            if (isLoginCorrect && isPasswordCorrect)
            {
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = $"{checkLoginErrorMessage} {checkPasswordErrorMessage}";
                return false;
            }

        }

        public static void CheckLength(System.Windows.Forms.TextBox textbox, System.Windows.Forms.Label label, int target_quantity)
        {
            int quantity = textbox.Text.Length;
            label.Text = quantity.ToString();
            if (quantity >= target_quantity) label.ForeColor = Color.FromArgb(122, 255, 122);
            else label.ForeColor = Color.FromArgb(255, 122, 122);
        }


        public static bool CheckPasswordSimilarity(string password, string repeatedPassword, out string errorMessage)
        {
            if (!string.IsNullOrEmpty(repeatedPassword))
            {
                if (password.Equals(repeatedPassword))
                {
                    errorMessage = "";
                    return true;
                }
                else 
                {
                    errorMessage = "Введенные пароли не совпадают.";
                    return false; 
                } 
            }
            else
            {
                errorMessage = "Подтвердите пароль.";
                return false;
            }
        }

        //private functions
        //------------------------------------------------------------------------------------------------------------------------------

        private static int CheckLogin(string login)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(login) && login.Length > 3)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception)
            {
                return 3;
            }
        }

        private static int CheckPassword(string password)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(password) && password.Length > 7)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(password, "^[a-zA-Z0-9!@#$%^&*()_+\\-=\\[\\]{};':\",./<>?]+$"))
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception)
            {
                return 3;
            }
        }
    }
}
