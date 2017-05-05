using log4net;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FashionStore.Code
{
    static public class CompatiblePdfReader
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CompatiblePdfReader));
        /// <summary>
        /// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf, called instead of PdfReader.open
        /// </summary>
        /// <param name="sFilename"></param>
        /// <returns></returns>
        static public PdfDocument Open(string sFilename)
        {
            string newName = WriteCompatiblePdf(sFilename);
            PdfDocument reader = PdfReader.Open(newName, PdfDocumentOpenMode.Import);
            try
            {
                if (File.Exists(newName))
                    File.Delete(newName);
            }catch(Exception ex)
            {
                logger.Error(ex);
            }
            return reader;
        }


        /// <summary>
        /// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf
        /// </summary>
        /// <param name="sFilename"></param>
        /// <returns></returns>
        static private string WriteCompatiblePdf(string sFilename)
        {
            string sNewPdf = HttpContext.Current.Server.MapPath("~/OrderReceipts") + @"\" + "NewAttachment_" + DateTime.Now.ToString("ddMMyyyyhhmmss")+".pdf";

            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(sFilename);

            // we retrieve the total number of pages
            int n = reader.NumberOfPages;
            // step 1: creation of a document-object
            iTextSharp.text.Document document = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            // step 2: we create a writer that listens to the document
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(sNewPdf, FileMode.Create));
            //write pdf that pdfsharp can understand
            writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_4);
            // step 3: we open the document
            document.Open();
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            iTextSharp.text.pdf.PdfImportedPage page;

            int rotation;

            int i = 0;
            while (i < n)
            {
                i++;
                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                document.NewPage();
                page = writer.GetImportedPage(reader, i);
                rotation = reader.GetPageRotation(i);
                if (rotation == 90 || rotation == 270)
                {
                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                }
                else
                {
                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }
            // step 5: we close the document
            document.Close();
            return sNewPdf;
        }
    }
}

