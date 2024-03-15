using Microsoft.VisualStudio.TestTools.UnitTesting;
using PopovLaba3;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace UnitTestPopovLaba
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConstHandler_CheckLoginAndPassword_ValidAll_ReturnsTrue()
        {
            // Arrange
            string login = "nikita24";
            string password = "primitelabi!!!";
            string errorMessage = "";

            // Act
            bool result = ConstHandler.CheckLoginAndPassword(login, password, out errorMessage);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(errorMessage, "");

        }

        [TestMethod]
        public void ConstHandler_CheckLoginAndPassword_ValidLoginShortPassword_ReturnsFalse()
        {
            // Arrange
            string login = "nikita24";
            string password = "prim";
            string errorMessage = "";

            // Act
            bool result = ConstHandler.CheckLoginAndPassword(login, password, out errorMessage);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(errorMessage, "");

        }

        [TestMethod]
        public void ConstHandler_CheckLoginAndPassword_InvalidLoginValidPassword_ReturnsFalse()
        {
            // Arrange
            string login = "логинлогин";
            string password = "primitelabi!!!";
            string errorMessage = "";

            // Act
            bool result = ConstHandler.CheckLoginAndPassword(login, password, out errorMessage);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(errorMessage, "");

        }

        [TestMethod]
        public void ConstHandler_CheckLoginAndPassword_ValidLoginInvalidPassword_ReturnsFalse()
        {
            // Arrange
            string login = "nikita24";
            string password = "примителаби";
            string errorMessage = "";

            // Act
            bool result = ConstHandler.CheckLoginAndPassword(login, password, out errorMessage);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(errorMessage, "");

        }

        [TestMethod]
        public void CheckLength_TextboxLengthReachesTargetQuantity_ReturnsTrue()
        {
            // Arrange
            TextBox textbox = new TextBox();
            textbox.Text= "tenAAAAAAA";
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            int targetQuantity = 10;

            // Act
            ConstHandler.CheckLength(textbox, label, targetQuantity);

            // Assert
            Assert.AreEqual(Color.FromArgb(122, 255, 122), label.ForeColor);
        }

        [TestMethod]
        public void CheckLength_TextboxLengthLessThanTargetQuantity_ReturnsFalse()
        {
            // Arrange
            TextBox textbox = new TextBox();
            textbox.Text = "nineAAAAA";
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            int targetQuantity = 10;

            // Act
            ConstHandler.CheckLength(textbox, label, targetQuantity);

            // Assert
            Assert.AreEqual(Color.FromArgb(255, 122, 122), label.ForeColor);
        }
    }
}
