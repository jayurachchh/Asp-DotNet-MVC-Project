using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Management2.Areas.Employee.Models;
using Project_Management2.Areas.ProjectWiseTask.Models;
using System.Data;
using ClosedXML.Excel;
using Project_Management2.Areas.Project.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Project_Management2.UrlEncryption;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
namespace Project_Management2.Areas.ProjectWiseTask.Controllers
{
    [Area("ProjectWiseTask")]
    [Route("ProjectWiseTask/[controller]/[action]")]
    public class ProjectWiseTaskController : Controller
    {
        Uri baseAddres = new Uri("https://localhost:7149/api");
        private readonly HttpClient _client;
        public ProjectWiseTaskController()
        { 
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;
        }

        [HttpGet]
        public IActionResult ProjectWiseTaskList(int? id)
        {
            ViewBag.projectList = ProjectList();
            ViewBag.employeelist = EmployeeList();
            List<ProjectWiseTaskModel> projectWiseTask = new List<ProjectWiseTaskModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/ProjectWiseTask/Get{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    projectWiseTask.Add(JsonConvert.DeserializeObject<ProjectWiseTaskModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    projectWiseTask = JsonConvert.DeserializeObject<List<ProjectWiseTaskModel>>(extractdDatajson);
                }
            }
            return View(projectWiseTask);
        }

        public DataTable ProjectWiseTaskList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/ProjectWiseTask/Get").Result;

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

        public FileResult Export_ProjectWiseTask_List_To_Excel()
        {
            DataTable dataTable = ProjectWiseTaskList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("ProjectWiseTask Statements");

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

                    string fileName = "ProjectWiseTask_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_ProjectWiseTask_List_To_pdf()
        {
            DataTable dataTable = ProjectWiseTaskList();
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
                    Paragraph title = new Paragraph("ProjectWiseTask List", new Font(boldBaseFont, 35));
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
                string fileName = "ProjectWiseTaskList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }

        [HttpGet]
        /*        public IActionResult Delete(int ProjectWiseTaskID)
                {
                    HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{ProjectWiseTaskID}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "ProjectWiseTask Deleted Sucessfully";
                    }

                    return RedirectToAction("ProjectWiseTaskList");
                }*/


        public IActionResult Delete(string[] ProjectWiseTaskIDlist)
        {

            // Check if EmpIDlist is not null and contains IDs
            if (ProjectWiseTaskIDlist != null && ProjectWiseTaskIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", ProjectWiseTaskIDlist);
                
                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/ProjectWiseTask/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "ProjectWiseTask Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete ProjectWiseTask";
                }
            }
            else
            {
                TempData["Message1"] = "No ProjectWiseTask Selected for Deletion";
            }

            return RedirectToAction("ProjectWiseTaskList");
        }
        /*        public IActionResult ProjectWiseTaskAddedit()
                {
                    return View();
                }*/
        [HttpGet]
        public IActionResult ProjectWiseTaskAddedit(string? ProjectWiseTaskID)
        {
            ViewBag.projectList = ProjectList();
            ViewBag.employeelist = EmployeeList();
            if (ProjectWiseTaskID != null)
            {
                ProjectWiseTaskModel projectWiseTaskModel = new ProjectWiseTaskModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/ProjectWiseTask/Get{UrlEncryptor.Decrypt(ProjectWiseTaskID)}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    projectWiseTaskModel = JsonConvert.DeserializeObject<ProjectWiseTaskModel>(extractdDatajson);
                }
                return View(projectWiseTaskModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProjectWiseTaskModel projectWiseTaskModel)
        {
            try
            {
                var client = new HttpClient();
                if (projectWiseTaskModel.ProjWiseTaskID == null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/ProjectWiseTask/Insert");
                    string model = JsonConvert.SerializeObject(projectWiseTaskModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "projectWiseTask Inserted Sucessfully";
                        return RedirectToAction("projectWiseTaskList");
                    }
                }
                else
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, baseAddres + "/ProjectWiseTask/Update");
                    string model = JsonConvert.SerializeObject(projectWiseTaskModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "projectWiseTask Updated Sucessfully";
                        return RedirectToAction("projectWiseTaskList");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error occur" + ex.Message;
            }
            return RedirectToAction("projectWiseTaskList");
        }


        [HttpPost]
        public async Task<IActionResult> Filter(ProjectWiseTaskModel projectWiseTask)
        {
            List<ProjectWiseTaskModel> projectWiseTaskModel = new List<ProjectWiseTaskModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/ProjectWiseTask/Post");
            string model = JsonConvert.SerializeObject(projectWiseTask);
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
                projectWiseTaskModel = JsonConvert.DeserializeObject<List<ProjectWiseTaskModel>>(extractdDatajson);
            }
            ViewBag.projectList = ProjectList();
            ViewBag.employeelist = EmployeeList();
            return View("projectWiseTaskList", projectWiseTaskModel);
        }


        [HttpGet]
        public List<ProjectDropDownModel> ProjectList()
        {

            List<ProjectDropDownModel> projectdropdownlist = new List<ProjectDropDownModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectdropdownlist = JsonConvert.DeserializeObject<List<ProjectDropDownModel>>(extractdDatajson);
            }
            return projectdropdownlist;
        }


        [HttpGet]
        public List<EmployeeDropDownModel> EmployeeList()
        {
            List<EmployeeDropDownModel> employeeDropDownModel = new List<EmployeeDropDownModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Employee/Get").Result;

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
