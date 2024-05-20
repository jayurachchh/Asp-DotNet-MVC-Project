using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Management2.Areas.Employee.Models;
using Project_Management2.Areas.Project.Models;
using Project_Management2.Areas.Status.Models;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.CodeAnalysis;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = iTextSharp.text.Font;
using Paragraph = iTextSharp.text.Paragraph;
using Document = iTextSharp.text.Document;
using Project_Management2.UrlEncryption;
namespace Project_Management2.Areas.Project.Controllers
{
    [Area("Project")]
    [Route("Project/[controller]/[action]")]
    public class ProjectController : Controller
    {
        Uri baseAddres = new Uri("https://localhost:7149/api");
        private readonly HttpClient _client;
        public ProjectController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;
        }

        [HttpGet]
        public IActionResult ProjectList(int? id)
        {
            ViewBag.StatusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            List<ProjectModel> project = new List<ProjectModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Get{id}/").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project.Add(JsonConvert.DeserializeObject<ProjectModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
                }
            }
            return View(project);
        }
        public IActionResult ProjectUpcomingList(int? id)
        {
            ViewBag.StatusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            List<ProjectModel> project = new List<ProjectModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Upcoming{id}/").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project.Add(JsonConvert.DeserializeObject<ProjectModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
                }
            }
            return View(project);
        }
        public IActionResult ProjectCurrentList(int? id)
        {
            ViewBag.StatusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            List<ProjectModel> project = new List<ProjectModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Current{id}/").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project.Add(JsonConvert.DeserializeObject<ProjectModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
                }
            }
            return View(project);
        }
        public IActionResult ProjectCompleteList(int? id)
        {
            ViewBag.StatusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            List<ProjectModel> project = new List<ProjectModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Complete{id}/").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project.Add(JsonConvert.DeserializeObject<ProjectModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
                }
            }
            return View(project);
        }

        public DataTable ProjectList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dataTable = JsonConvert.DeserializeObject<DataTable>(extractdDatajson);
            }

            return dataTable;
        }

        public FileResult Export_Project_List_To_Excel()
        {
            DataTable dataTable = ProjectList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Project Statements");

                // Adding the DataTable data to the worksheet starting from cell A1
                var tableRange = ws.Cell(1, 1).InsertTable(dataTable, true).AsRange();

                // Adjust column widths to fit contents
                ws.Columns().AdjustToContents();

                // Apply styling to header row
                var headerRow = tableRange.FirstRow();
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Set the row height and cell alignment for data rows
                foreach (var row in tableRange.RowsUsed().Skip(1))
                {
                    ws.Row(row.RowNumber()).Height = 20; // Set the height for the row
                    foreach (var cell in row.CellsUsed())
                    {
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    string fileName = "Project_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_Project_List_To_pdf()
        {
            DataTable dataTable = ProjectList();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Custom page size
                iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(2300, 1200);
                using (Document document = new Document(customPageSize))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    BaseFont boldBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                    Font boldFont = new Font(boldBaseFont, 12);


                    // Title
                    Paragraph title = new Paragraph("Project List", new Font(boldBaseFont, 35));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);
                    document.Add(new Chunk("\n"));


                    // Table setup
                    PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100,
                        DefaultCell = { Padding = 10 }
                    };

                    // Headers
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Font headerFont = boldFont;
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10
                        };
                        pdfTable.AddCell(headerCell);
                    }

                    // Data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var item = row[column];
                            Font itemFont = boldFont;

                            PdfPCell dataCell = new PdfPCell(new Phrase(item?.ToString(), itemFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 10
                            };
                            pdfTable.AddCell(dataCell);
                        }
                    }

                    document.Add(pdfTable);
                    document.Close();
                }

                // File result
                string fileName = "ProjectList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }

        public DataTable UpcomingProjectList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Upcoming").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dataTable = JsonConvert.DeserializeObject<DataTable>(extractdDatajson);
            }

            return dataTable;
        }

        public FileResult Export_ProjectUpcoming_List_To_Excel()
        {
            DataTable dataTable = UpcomingProjectList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Project Statements");

                // Adding the DataTable data to the worksheet starting from cell A1
                var tableRange = ws.Cell(1, 1).InsertTable(dataTable, true).AsRange();

                // Adjust column widths to fit contents
                ws.Columns().AdjustToContents();

                // Apply styling to header row
                var headerRow = tableRange.FirstRow();
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Set the row height and cell alignment for data rows
                foreach (var row in tableRange.RowsUsed().Skip(1))
                {
                    ws.Row(row.RowNumber()).Height = 20; // Set the height for the row
                    foreach (var cell in row.CellsUsed())
                    {
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    string fileName = "ProjectUpcoming_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_ProjectUpcoming_List_To_pdf()
        {
            DataTable dataTable = UpcomingProjectList();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Custom page size
                iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(2300, 1200);
                using (Document document = new Document(customPageSize))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    BaseFont boldBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                    Font boldFont = new Font(boldBaseFont, 12);


                    // Title
                    Paragraph title = new Paragraph("ProjectUpcoming List", new Font(boldBaseFont, 35));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);
                    document.Add(new Chunk("\n"));


                    // Table setup
                    PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100,
                        DefaultCell = { Padding = 10 }
                    };

                    // Headers
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Font headerFont = boldFont;
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10
                        };
                        pdfTable.AddCell(headerCell);
                    }

                    // Data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var item = row[column];
                            Font itemFont = boldFont;

                            PdfPCell dataCell = new PdfPCell(new Phrase(item?.ToString(), itemFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 10
                            };
                            pdfTable.AddCell(dataCell);
                        }
                    }

                    document.Add(pdfTable);
                    document.Close();
                }

                // File result
                string fileName = "ProjectUpcomingList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }

        public DataTable CurrentProjectList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Current").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dataTable = JsonConvert.DeserializeObject<DataTable>(extractdDatajson);
            }

            return dataTable;
        }

        public FileResult Export_ProjectCurrent_List_To_Excel()
        {
            DataTable dataTable = CurrentProjectList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Project Statements");

                // Adding the DataTable data to the worksheet starting from cell A1
                var tableRange = ws.Cell(1, 1).InsertTable(dataTable, true).AsRange();

                // Adjust column widths to fit contents
                ws.Columns().AdjustToContents();

                // Apply styling to header row
                var headerRow = tableRange.FirstRow();
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Set the row height and cell alignment for data rows
                foreach (var row in tableRange.RowsUsed().Skip(1))
                {
                    ws.Row(row.RowNumber()).Height = 20; // Set the height for the row
                    foreach (var cell in row.CellsUsed())
                    {
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    string fileName = "ProjectCurrent_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_ProjectCurrent_List_To_pdf()
        {
            DataTable dataTable = CurrentProjectList();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Custom page size
                iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(2300, 1200);
                using (Document document = new Document(customPageSize))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    BaseFont boldBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                    Font boldFont = new Font(boldBaseFont, 12);


                    // Title
                    Paragraph title = new Paragraph("ProjectCurrent List", new Font(boldBaseFont, 35));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);
                    document.Add(new Chunk("\n"));


                    // Table setup
                    PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100,
                        DefaultCell = { Padding = 10 }
                    };

                    // Headers
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Font headerFont = boldFont;
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10
                        };
                        pdfTable.AddCell(headerCell);
                    }

                    // Data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var item = row[column];
                            Font itemFont = boldFont;

                            PdfPCell dataCell = new PdfPCell(new Phrase(item?.ToString(), itemFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 10
                            };
                            pdfTable.AddCell(dataCell);
                        }
                    }

                    document.Add(pdfTable);
                    document.Close();
                }

                // File result
                string fileName = "ProjectCurrentList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }

        public DataTable CompleteProjectList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Complete").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dataTable = JsonConvert.DeserializeObject<DataTable>(extractdDatajson);
            }

            return dataTable;
        }

        public FileResult Export_ProjectComplete_List_To_Excel()
        {
            DataTable dataTable = CompleteProjectList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Project Statements");

                // Adding the DataTable data to the worksheet starting from cell A1
                var tableRange = ws.Cell(1, 1).InsertTable(dataTable, true).AsRange();

                // Adjust column widths to fit contents
                ws.Columns().AdjustToContents();

                // Apply styling to header row
                var headerRow = tableRange.FirstRow();
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Set the row height and cell alignment for data rows
                foreach (var row in tableRange.RowsUsed().Skip(1))
                {
                    ws.Row(row.RowNumber()).Height = 20; // Set the height for the row
                    foreach (var cell in row.CellsUsed())
                    {
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    string fileName = "ProjectComplete_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_ProjectComplete_List_To_pdf()
        {
            DataTable dataTable = CompleteProjectList();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Custom page size
                iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(2300, 1200);
                using (Document document = new Document(customPageSize))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    BaseFont boldBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                    Font boldFont = new Font(boldBaseFont, 12);


                    // Title
                    Paragraph title = new Paragraph("ProjectComplete List", new Font(boldBaseFont, 35));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);
                    document.Add(new Chunk("\n"));


                    // Table setup
                    PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100,
                        DefaultCell = { Padding = 10 }
                    };

                    // Headers
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Font headerFont = boldFont;
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10
                        };
                        pdfTable.AddCell(headerCell);
                    }

                    // Data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var item = row[column];
                            Font itemFont = boldFont;

                            PdfPCell dataCell = new PdfPCell(new Phrase(item?.ToString(), itemFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 10
                            };
                            pdfTable.AddCell(dataCell);
                        }
                    }

                    document.Add(pdfTable);
                    document.Close();
                }

                // File result
                string fileName = "ProjectCompleteList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }


        [HttpGet]
        /* public IActionResult Delete(int ProjID)
         {
             HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{ProjID}").Result;
             if (response.IsSuccessStatusCode)
             {
                 TempData["Message"] = "Project Deleted Sucessfully";
             }

             return RedirectToAction("ProjectList");
         }*/


        public IActionResult Delete(string[] ProjIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (ProjIDlist != null && ProjIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", ProjIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Project/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Project Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Project";
                }
            }
            else
            {
                TempData["Message1"] = "No Project Selected for Deletion";
            }

            return RedirectToAction("ProjectList");
        }

        public IActionResult DeleteUpcoming(string[] ProjIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (ProjIDlist != null && ProjIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", ProjIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Project/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Project Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Project";
                }
            }
            else
            {
                TempData["Message1"] = "No Project Selected for Deletion";
            }

            return RedirectToAction("ProjectUpcomingList");
        }
        public IActionResult DeleteCurrent(string[] ProjIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (ProjIDlist != null && ProjIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", ProjIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Project/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Project Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Project";
                }
            }
            else
            {
                TempData["Message1"] = "No Project Selected for Deletion";
            }

            return RedirectToAction("ProjectCurrentList");
        }
        public IActionResult DeleteComplete(string[] ProjIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (ProjIDlist != null && ProjIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", ProjIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Project/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Project Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Project";
                }
            }
            else
            {
                TempData["Message1"] = "No Project Selected for Deletion";
            }

            return RedirectToAction("ProjectCompleteList");
        }
        /*
                public IActionResult ProjectAddedit()
                {
                    return View();
                }*/

        [HttpGet]
        public IActionResult ProjectAddedit(string? ProjID)
        {
            ViewBag.StatusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            if (ProjID!=null)
            {
                ProjectModel? project = new ProjectModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Get{ProjID}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    project = JsonConvert.DeserializeObject<ProjectModel>(extractdDatajson);
                }
                return View(project);
            }
            else
            {
                return View();
            }           
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProjectModel projectModel)
        {
            try
            {
                var client = new HttpClient();
                if (projectModel.ProjID == null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Project/Insert");
                    string model = JsonConvert.SerializeObject(projectModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Project Inserted Sucessfully";
                        return RedirectToAction("ProjectList");
                    }
                }
                else
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, baseAddres + "/Project/Update");
                    string model = JsonConvert.SerializeObject(projectModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Project Updated Sucessfully";
                        return RedirectToAction("ProjectList");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error occur" + ex.Message;
            }
            return RedirectToAction("ProjectList");
        }

        [HttpPost]
        public async Task<IActionResult> Filter(ProjectModel project)
        {
            List<ProjectModel> projectModel = new List<ProjectModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Project/Post");
            string model = JsonConvert.SerializeObject(project);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectModel = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
            }
           
            ViewBag.statusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            return View("ProjectList", projectModel);
        }

        [HttpPost]
        public async Task<IActionResult> FilterUpcoming(ProjectModel project)
        {
            List<ProjectModel> projectModel = new List<ProjectModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Project/Upcoming");
            string model = JsonConvert.SerializeObject(project);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectModel = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
            }

            ViewBag.statusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            return View("ProjectUpcomingList", projectModel);
        }
        [HttpPost]
        public async Task<IActionResult> FilterCurrent(ProjectModel project)
        {
            List<ProjectModel> projectModel = new List<ProjectModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Project/Current");
            string model = JsonConvert.SerializeObject(project);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectModel = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
            }

            ViewBag.statusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            return View("ProjectCurrentList", projectModel);
        }
        [HttpPost]
        public async Task<IActionResult> FilterComplete(ProjectModel project)
        {
            List<ProjectModel> projectModel = new List<ProjectModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Project/Complete");
            string model = JsonConvert.SerializeObject(project);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectModel = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);
            }

            ViewBag.statusList = StatusList();
            ViewBag.employeemanagerlist = EmployeemanagerList();
            return View("ProjectCompleteList", projectModel);
        }

        [HttpGet]
        public List<StatusDropDownModel> StatusList()
        {
            List<StatusDropDownModel> statusDropDownlist = new List<StatusDropDownModel>();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Status/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                statusDropDownlist = JsonConvert.DeserializeObject<List<StatusDropDownModel>>(extractdDatajson);
            }
            return statusDropDownlist;
/*            foreach (var item in project)
            {

                StatusDropDownModel list = new StatusDropDownModel();
                list.StatusID = Convert.ToInt32(item.StatusID);
                list.StatusName = Convert.ToString(item.StatusName);
                statusDropDownlist.Add(list);
            }
            ViewBag.StatusList = statusDropDownlist;*/
          /* return View();*/
        }
        [HttpGet]
        public List<EmployeeDropDownModel> EmployeemanagerList()
        {
            List<EmployeeDropDownModel> employeeDropDownModel = new List<EmployeeDropDownModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Employee/GetManager").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employeeDropDownModel = JsonConvert.DeserializeObject<List<EmployeeDropDownModel>>(extractdDatajson);
            }
            return employeeDropDownModel;
        }

    }
}
