using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

public class PdfPrinter
{
    public static void SaveAndPrintPdf(RichTextBox richTextBox)
    {
        if (richTextBox == null)
        {
            MessageBox.Show("RichTextBox is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.DefaultExt = "pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Создаем новый документ PDF
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.Info.Title = "RichTextBox Content";

                    // Создаем страницу PDF
                    PdfPage pdfPage = pdfDocument.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Arial", 12);

                    // Получаем текст из RichTextBox
                    string text = richTextBox.Text;

                    // Определяем прямоугольник для текста, учитывая отступы
                    XRect rect = new XRect(10, 10, pdfPage.Width - 20, pdfPage.Height - 20);

                    // Печатаем текст с автоматическим переносом
                    DrawTextWithWordWrap(gfx, font, XBrushes.Black, rect, text);

                    // Сохраняем PDF в файл
                    pdfDocument.Save(filePath);

                    MessageBox.Show($"PDF saved successfully to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Открытие диалога печати
                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = filePath,
                            Verb = "Print",
                            CreateNoWindow = true,
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving or printing PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void DrawTextWithWordWrap(XGraphics gfx, XFont font, XBrush brush, XRect rect, string text)
    {
        // Разделяем текст на слова
        string[] words = text.Split(' ');

        // Используем StringBuilder для построения строки с переносами
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var word in words)
        {
            // Измеряем ширину слова
            double wordWidth = gfx.MeasureString(word, font).Width;

            // Если добавление следующего слова приведет к переполнению строки, добавляем перенос
            if (rect.Width < wordWidth)
            {
                stringBuilder.Append(Environment.NewLine);
            }

            stringBuilder.Append(word + " ");
        }

        // Печатаем собранную строку с переносами
        gfx.DrawString(stringBuilder.ToString(), font, brush, rect, XStringFormats.TopLeft);
    }
}
