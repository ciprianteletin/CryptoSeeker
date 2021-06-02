using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Proiect.NET
{
    class PDFGenerator
    {
        private static string path = "docs\\Crypto.pdf";
        private static string pdfFilePath = "";
        private static Document document;
        private static Dictionary<string, string> Dict = new Dictionary<string, string>();

        public static void CreateDocument(string CryptoName, dynamic[] coins)
        {
            pdfFilePath = path;
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            pdfFilePath = projectDirectory + "\\" + pdfFilePath;

            document = new Document();
            if (!Directory.Exists(projectDirectory + "\\docs"))
            {
                Directory.CreateDirectory(projectDirectory + "\\docs");
            }
            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }
            PdfWriter.GetInstance(document, new FileStream(pdfFilePath, FileMode.Create));
            document.Open();
            document.AddTitle("CryptoChartPDF");
            document.AddAuthor("Crypto Finder");

            Paragraph paragraph = new Paragraph("Data");
            foreach (dynamic coin in coins)
            {
                double coinPrice = coin["quote"]["USD"]["price"];
                coinPrice = Math.Round(coinPrice, 2);
                string coinName = coin["name"];

                paragraph = new Paragraph(coinName + " : " + coinPrice + "$");
                document.Add(paragraph);
            }
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("You selected: " + CryptoName));
            document.Add(new Paragraph("Below is the chart for the crypto currency that you selected in the application:"));
            document.Add(new Paragraph("\n"));

            Image plot = Image.GetInstance("Crypto_Fig.png");
            document.Add(plot);
            document.Close();
        }

        static PDFGenerator()
        {
            string[] ids = { "1", "1027", "2", "1958", "1376", "2010", "512", "1765", "74", "825", "52" };

            for(int i = 0; i < ids.Length; ++i)
            {
                Dict.Add(CoinMarket.coins[i], ids[i]);
            }
        }

        private PDFGenerator()
        {

        }
    }
}