using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PointOfSaleMVC.Manager;
using PointOfSaleMVC.Models;
namespace PointOfSaleMVC.Controllers
{
    public class ReportController : Controller
    {
        SetupManager setupManager = new SetupManager();
        ReportManager reportManager = new ReportManager();
        string orderNo = DateTime.Now.Ticks.ToString().Substring(0, 6);
        string orderDate = DateTime.Now.ToString("dd MMM yyyy");
        decimal totalAmtStr;
        string accountNo = "0123456789012";
        string accountName = "John Willion";
        string branch = "Phahon Yothin Branch";
        string bank = "Kasikorn Bank";
        // GET: Report
        /*
         *  Sales report code starts here 
         */
        [HttpGet]
        public ActionResult SalesReport()
        {
            return View();
        }
        /*
         *  Sales report code ends here 
         */

        /*
         *  Purchase report code starts here 
         */
        [HttpGet]
        public ActionResult PurchaseReport()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }

        [HttpPost]
        public ActionResult GetPurchaseByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportManager.GetPurchaseByBranchId(stockIn);
            return Json(stockIns, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("PurchaseReport")]
        public ActionResult PrintPurchaseReport(StockIn stockIn)
        {
            Branch branch = setupManager.GetBranch(stockIn.BranchId);
            List<StockIn> stockIns = reportManager.GetPurchaseByBranchId(stockIn);
            int sl = 1;

            // Create a Document object
            Document document = new Document(PageSize.A4, 70, 70, 70, 70);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, PDFData);

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            #region Top table
            // Create the header table 
            PdfPTable headertable = new PdfPTable(1);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 4 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX;  //for testing
            headertable.SpacingAfter = 30;
            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell nextPostCell1 = new PdfPCell(new Phrase(branch.BranchName, titleFont));
            nextPostCell1.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell1);
            PdfPCell nextPostCell2 = new PdfPCell(new Phrase(branch.BranchAddress, bodyFont));
            nextPostCell2.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell2);
            PdfPCell nextPostCell3 = new PdfPCell(new Phrase(branch.BranchContactNo, bodyFont));
            nextPostCell3.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell3);
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Rowspan = 4;
            nesthousing.Padding = 0f;
            headertable.AddCell(nesthousing);

            headertable.AddCell("");

            PdfPCell noCell = new PdfPCell(new Phrase("No : " + orderNo, bodyFont));
            noCell.HorizontalAlignment = 0;
            noCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(noCell);
            PdfPCell dateCell = new PdfPCell(new Phrase("Date : " + orderDate, bodyFont));
            dateCell.HorizontalAlignment = 0;
            dateCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(dateCell);
            PdfPCell billCell = new PdfPCell(new Phrase(""));
            billCell.HorizontalAlignment = 0;
            billCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase(""));
            headertable.AddCell(new Phrase(""));
            headertable.AddCell(new Phrase(""));
            PdfPCell invoiceCell = new PdfPCell(new Phrase("Purchase Report", titleFont));
            invoiceCell.HorizontalAlignment = 0;
            invoiceCell.PaddingLeft = 180f;
            invoiceCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(invoiceCell);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 1)));
            headertable.AddCell(line);
            document.Add(headertable);
            #endregion

            #region Items Table
            //Create body table
            PdfPTable itemTable = new PdfPTable(6);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 5, 20, 25, 30, 20, 15 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("SL", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("Date", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Description", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Branch", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Supplier", boldTableFont));
            cell5.HorizontalAlignment = 1;
            itemTable.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase("Purchase Price", boldTableFont));
            cell6.HorizontalAlignment = 1;
            itemTable.AddCell(cell6);

            foreach (StockIn aStockIn in stockIns)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(sl++.ToString(), bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(aStockIn.PurchaseDateTime.ToString("yyyy-MM-dd"), bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(descCell);

                PdfPCell categoryCell = new PdfPCell(new Phrase(aStockIn.Item.ItemName + ">" + aStockIn.Item.Quantity + ">" + aStockIn.Item.CostPrice, bodyFont));
                categoryCell.HorizontalAlignment = 0;
                categoryCell.PaddingLeft = 10f;
                categoryCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(categoryCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(aStockIn.Branch.BranchName, bodyFont));
                qtyCell.HorizontalAlignment = 1;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(qtyCell);

                PdfPCell amtCell = new PdfPCell(new Phrase(aStockIn.Party.PartyName.ToString(), bodyFont));
                amtCell.HorizontalAlignment = 1;
                amtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(amtCell);

                PdfPCell priceCell = new PdfPCell(new Phrase((aStockIn.Item.Quantity * aStockIn.Item.CostPrice).ToString(), bodyFont));
                priceCell.HorizontalAlignment = 1;
                priceCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(priceCell);
                totalAmtStr += (aStockIn.Item.Quantity * aStockIn.Item.CostPrice);
            }
            // Table footer
            PdfPCell totalAmtCell0 = new PdfPCell(new Phrase(""));
            totalAmtCell0.Border = Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell0);
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell4 = new PdfPCell(new Phrase(""));
            totalAmtCell4.Border = Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell4);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Total Amount", boldTableFont));
            totalAmtStrCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(totalAmtStr.ToString("#,###.00"), boldTableFont));
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);

            //PdfPCell cell = new PdfPCell(new Phrase("*** Please note that ABC Co., Ltd’s bank account is USD Bank Account ***", bodyFont));
            //cell.Colspan = 4;
            //cell.HorizontalAlignment = 1;
            //itemTable.AddCell(cell);
            document.Add(itemTable);
            #endregion

            Chunk transferBank = new Chunk("Reported By:", boldTableFont);
            transferBank.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
            document.Add(transferBank);
            document.Add(Chunk.NEWLINE);

            // Bank Account Info
            PdfPTable bottomTable = new PdfPTable(3);
            bottomTable.HorizontalAlignment = 0;
            bottomTable.TotalWidth = 300f;
            bottomTable.SetWidths(new int[] { 90, 10, 200 });
            bottomTable.LockedWidth = true;
            bottomTable.SpacingBefore = 20;
            bottomTable.DefaultCell.Border = Rectangle.NO_BORDER;
            bottomTable.AddCell(new Phrase("Employee No", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountNo, bodyFont));
            bottomTable.AddCell(new Phrase("Employee Name", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountName, bodyFont));
            bottomTable.AddCell(new Phrase("Branch", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch.BranchName, bodyFont));
            bottomTable.AddCell(new Phrase("Branch Address", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch.BranchAddress, bodyFont));
            document.Add(bottomTable);

            ////Approved by
            //PdfContentByte cb = new PdfContentByte(writer);
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //cb = writer.DirectContent;
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 10);
            //cb.SetTextMatrix(pageSize.GetLeft(300), 200);
            //cb.ShowText("Approved by,");
            //cb.EndText();
            ////Image Singature
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/Bill_Gates2.png"));
            //logo.SetAbsolutePosition(pageSize.GetLeft(300), 140);
            //document.Add(logo);

            //cb = new PdfContentByte(writer);
            //bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //cb = writer.DirectContent;
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 10);
            //cb.SetTextMatrix(pageSize.GetLeft(70), 100);
            //cb.ShowText("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.");
            //cb.EndText();

            writer.CloseStream = false; //set the closestream property
                                        // Close the Document without closing the underlying stream
            document.Close();
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = string.Empty;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            Response.AddHeader("Content-Disposition", $"attachment;filename=PurchaseReport-{orderNo}.pdf");
            Response.OutputStream.Write(PDFData.GetBuffer(), 0, PDFData.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();



























            //Document pdfDoc = new Document(PageSize.A4, 50, 50, 50, 25);
            //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();

            ////Top Heading
            //Chunk chunk = new Chunk(branch.BranchName, FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            //pdfDoc.Add(chunk);
            //PdfPTable table = new PdfPTable(1);
            //table.WidthPercentage = 100;
            ////0=Left, 1=Centre, 2=Right
            //table.HorizontalAlignment = 0;
            //table.SpacingBefore = 20f;
            //table.SpacingAfter = 50f;//30

            ////Cell no 2
            //chunk = new Chunk(branch.BranchAddress, FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK));
            //PdfPCell cell = new PdfPCell();
            //cell.Border = 0;
            //cell.AddElement(chunk);
            //table.AddCell(cell);
            //chunk = new Chunk(branch.BranchContactNo, FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK));
            //cell = new PdfPCell();
            //cell.Border = 0;
            //cell.AddElement(chunk);
            //table.AddCell(cell);

            ////Add table to document
            //pdfDoc.Add(table);


            //chunk = new Chunk("Purchase Report", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            //pdfDoc.Add(chunk);
            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 1)));
            //pdfDoc.Add(line);
            ////Table
            //table = new PdfPTable(6);
            //table.WidthPercentage = 100;
            //table.HorizontalAlignment = 0;
            //table.SpacingBefore = 20f;
            //table.SpacingAfter = 30f;

            ////Cell
            //cell = new PdfPCell();
            //chunk = new Chunk("SL");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //cell = new PdfPCell();
            //chunk = new Chunk("Date");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //cell = new PdfPCell();
            //chunk = new Chunk("Description");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //cell = new PdfPCell();
            //chunk = new Chunk("Branch");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //cell = new PdfPCell();
            //chunk = new Chunk("Supplier");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //cell = new PdfPCell();
            //chunk = new Chunk("Purchase Price");
            //chunk.Font.Color = BaseColor.WHITE;
            //cell.AddElement(chunk);
            //cell.BackgroundColor = new BaseColor(44, 62, 80);
            //table.AddCell(cell);

            //foreach (StockIn aStockIn in stockIns)
            //{
            //    table.AddCell((sl++).ToString());
            //    table.AddCell(aStockIn.PurchaseDateTime.ToString("yyyy-MM-dd"));
            //    table.AddCell(aStockIn.Item.ItemName + ">" + aStockIn.Item.Quantity + ">" + aStockIn.Item.CostPrice);
            //    table.AddCell(aStockIn.Branch.BranchName);
            //    table.AddCell(aStockIn.Party.PartyName);
            //    table.AddCell((aStockIn.Item.Quantity * aStockIn.Item.CostPrice).ToString());
            //}


            //pdfDoc.Add(table);
            //pdfWriter.CloseStream = false;
            //pdfDoc.Close();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=PurchaseReport.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Write(pdfDoc);
            //Response.End();
            return View();
        }
        /*
         *  Purchase report code ends here 
         */

        /*
         *  Expense report code starts here 
         */
        [HttpGet]
        public ActionResult ExpenseReport()
        {
            return View();
        }

        /*
         *  Expense report code ends here 
         */

        /*
         *  Stock report code starts here 
         */
        [HttpGet]
        public ActionResult StockReport()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }

        [HttpPost]
        [ActionName("StockReport")]
        public ActionResult PrintStockReport(StockIn stockIn)
        {
            Branch branch = setupManager.GetBranch(stockIn.BranchId);
            List<StockIn> stockIns = reportManager.GetStocksByBranchId(stockIn);
            int sl = 1;

            // Create a Document object
            Document document = new Document(PageSize.A4, 70, 70, 70, 70);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, PDFData);

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            #region Top table
            // Create the header table 
            PdfPTable headertable = new PdfPTable(1);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] {4});  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX;  //for testing
            headertable.SpacingAfter = 30;
            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell nextPostCell1 = new PdfPCell(new Phrase(branch.BranchName, titleFont));
            nextPostCell1.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell1);
            PdfPCell nextPostCell2 = new PdfPCell(new Phrase(branch.BranchAddress, bodyFont));
            nextPostCell2.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell2);
            PdfPCell nextPostCell3 = new PdfPCell(new Phrase(branch.BranchContactNo, bodyFont));
            nextPostCell3.Border = Rectangle.NO_BORDER;
            nested.AddCell(nextPostCell3);
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Rowspan = 4;
            nesthousing.Padding = 0f;
            headertable.AddCell(nesthousing);

            headertable.AddCell("");
            
            PdfPCell noCell = new PdfPCell(new Phrase("No : "+ orderNo, bodyFont));
            noCell.HorizontalAlignment = 0;
            noCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(noCell);
            PdfPCell dateCell = new PdfPCell(new Phrase("Date : "+orderDate, bodyFont));
            dateCell.HorizontalAlignment = 0;
            dateCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(dateCell);
            PdfPCell billCell = new PdfPCell(new Phrase(""));
            billCell.HorizontalAlignment = 0;
            billCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase(""));
            headertable.AddCell(new Phrase(""));
            headertable.AddCell(new Phrase(""));
            PdfPCell invoiceCell = new PdfPCell(new Phrase("Stock Report", titleFont));
            invoiceCell.HorizontalAlignment = 0;
            invoiceCell.PaddingLeft = 180f;
            invoiceCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(invoiceCell);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 1)));
            headertable.AddCell(line);
            document.Add(headertable);
            #endregion

            #region Items Table
            //Create body table
            PdfPTable itemTable = new PdfPTable(5);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 5, 30, 30, 15, 20});  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 30;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("SL", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("ITEM Name", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Category Full Path", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Stock Quantity", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Price", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell5);

            foreach (StockIn aStockIn in stockIns)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(sl++.ToString(), bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(aStockIn.Item.ItemName, bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(descCell);

                PdfPCell categoryCell = new PdfPCell(new Phrase(aStockIn.Item.Category.RootCategory.CategoryName + ">" + aStockIn.Item.Category.CategoryName + ">" + aStockIn.Item.ItemName, bodyFont));
                categoryCell.HorizontalAlignment = 0;
                categoryCell.PaddingLeft = 10f;
                categoryCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(categoryCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(aStockIn.Item.Quantity.ToString(), bodyFont));
                qtyCell.HorizontalAlignment = 1;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(qtyCell);

                PdfPCell amtCell = new PdfPCell(new Phrase(aStockIn.Item.SalePrice.ToString(), bodyFont));
                amtCell.HorizontalAlignment = 1;
                amtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(amtCell);
                
            }




            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell3 = new PdfPCell(new Phrase(""));
            totalAmtCell3.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell3);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase(""));
            totalAmtStrCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtStriCell = new PdfPCell(new Phrase(""));
            totalAmtStriCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStriCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStriCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(""));
            totalAmtCell.Border = Rectangle.TOP_BORDER;
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);

            //PdfPCell cell = new PdfPCell(new Phrase("*** Please note that ABC Co., Ltd’s bank account is USD Bank Account ***", bodyFont));
            //cell.Colspan = 4;
            //cell.HorizontalAlignment = 1;
            //itemTable.AddCell(cell);
            document.Add(itemTable);
            #endregion

            Chunk transferBank = new Chunk("Employee Information:", boldTableFont);
            transferBank.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
            document.Add(transferBank);
            document.Add(Chunk.NEWLINE);

            // Bank Account Info
            PdfPTable bottomTable = new PdfPTable(3);
            bottomTable.HorizontalAlignment = 0;
            bottomTable.TotalWidth = 300f;
            bottomTable.SetWidths(new int[] { 90, 10, 200 });
            bottomTable.LockedWidth = true;
            bottomTable.SpacingBefore = 20;
            bottomTable.DefaultCell.Border = Rectangle.NO_BORDER;
            bottomTable.AddCell(new Phrase("Account No", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountNo, bodyFont));
            bottomTable.AddCell(new Phrase("Account Name", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountName, bodyFont));
            bottomTable.AddCell(new Phrase("Branch", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch.BranchName, bodyFont));
            bottomTable.AddCell(new Phrase("Address", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch.BranchAddress, bodyFont));
            document.Add(bottomTable);

            ////Approved by
            //PdfContentByte cb = new PdfContentByte(writer);
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //cb = writer.DirectContent;
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 10);
            //cb.SetTextMatrix(pageSize.GetLeft(300), 200);
            //cb.ShowText("Approved by,");
            //cb.EndText();
            ////Image Singature
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/Bill_Gates2.png"));
            //logo.SetAbsolutePosition(pageSize.GetLeft(300), 140);
            //document.Add(logo);

            //cb = new PdfContentByte(writer);
            //bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //cb = writer.DirectContent;
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 10);
            //cb.SetTextMatrix(pageSize.GetLeft(70), 100);
            //cb.ShowText("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.");
            //cb.EndText();

            writer.CloseStream = false; //set the closestream property
                                        // Close the Document without closing the underlying stream
            document.Close();
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = string.Empty;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            Response.AddHeader("Content-Disposition", $"attachment;filename=StockReport-{orderNo}.pdf");
            Response.OutputStream.Write(PDFData.GetBuffer(), 0, PDFData.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
            return View();
        }

        [HttpPost]
        public ActionResult GetStocksByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportManager.GetStocksByBranchId(stockIn);
            return Json(stockIns, JsonRequestBehavior.AllowGet);
        }
        /*
         *  Stock report code ends here 
         */

        /*
         *  Income report code starts here 
         */
        [HttpGet]
        public ActionResult IncomeReport()
        {
            return View();
        }
        /*
         *  Income report code ends here 
         */
    }
}