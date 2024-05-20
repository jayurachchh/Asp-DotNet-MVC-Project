using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Management2.Areas.Employee.Models;
using Project_Management2.Areas.TaskAssign.Models;
using System.Data;
using ClosedXML.Excel;
using Project_Management2.Areas.ProjectWiseTask.Models;
using DocumentFormat.OpenXml.EMMA;
using Project_Management2.Areas.Status.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Project_Management2.UrlEncryption;

namespace Project_Management2.Areas.TaskAssign.Controllers
{
    [Area("TaskAssign")]
    [Route("TaskAssign/[controller]/[action]")]
    public class TaskAssignController : Controller
    {
        Uri baseAddres = new Uri("https://localhost:7149/api");

        private readonly HttpClient _client;
        public TaskAssignController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;
        }
        [HttpGet]
        public IActionResult TaskAssignList(int? id)
        {
            ViewBag.statusList = StatusList();
            ViewBag.projectWistTaskList = ProjectWiseTaskList();
            ViewBag.employeelist = EmployeeList();
            List<TaskAssignModel> taskAssignModels = new List<TaskAssignModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/TaskAssign/Get{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    taskAssignModels.Add(JsonConvert.DeserializeObject<TaskAssignModel>(extractdDatajson));
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    taskAssignModels = JsonConvert.DeserializeObject<List<TaskAssignModel>>(extractdDatajson);
                }
            }
            return View(taskAssignModels);
        }



        public DataTable TaskAssignList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/TaskAssign/Get").Result;

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
        /*        public async Task<String> getApiData (string path, HttpMethod type,dynamic model=null)
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(type, path);
                    request.Content = new StringContent(JsonConvert.SerializeObject(model??""), null, "application/json");
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }*/
        public FileResult Export_TaskAssign_List_To_Excel()
        {
            DataTable dataTable = TaskAssignList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("TaskAssign Statements");

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

                    string fileName = "TaskAssign_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            } 
        }


        public FileResult Export_TaskAssign_List_To_pdf()
        {
            DataTable dataTable = TaskAssignList();
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
                    Paragraph title = new Paragraph("TaskAssign List", new Font(boldBaseFont, 35));
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
                string fileName = "TaskAssignList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }

        [HttpGet]
        /*  public IActionResult Delete(int TaskAssignID)
          {
              HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{TaskAssignID}").Result;
              if (response.IsSuccessStatusCode)
              {
                  TempData["Message"] = "TaskAssign Deleted Sucessfully";
              }

              return RedirectToAction("TaskAssignList");
          }*/

        public IActionResult Delete(string[] TaskAssignIDlist) 
        {
            // Check if EmpIDlist is not null and contains IDs
            if (TaskAssignIDlist != null && TaskAssignIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", TaskAssignIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/TaskAssign/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "TaskAssign Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete TaskAssign";
                }
            }
            else
            {
                TempData["Message1"] = "No TaskAssign Selected for Deletion";
            }

            return RedirectToAction("TaskAssignList");
        }
        /*        public IActionResult TaskAssignAddedit()
                {
                    return View();
                }*/

        [HttpGet]
        public IActionResult TaskAssignAddedit(int? TaskAssignID)
        {
            ViewBag.statusList = StatusList();
            ViewBag.projectWistTaskList = ProjectWiseTaskList();
            ViewBag.employeelist = EmployeeList();
            if (TaskAssignID != null)
            {
                TaskAssignModel? taskAssignModel = new TaskAssignModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/TaskAssign/Get{TaskAssignID}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    taskAssignModel = JsonConvert.DeserializeObject<TaskAssignModel>(extractdDatajson);
                }
                return View(taskAssignModel);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Save(TaskAssignModel taskAssignModel)
        {
            try
            {
                var client = new HttpClient();
                if (taskAssignModel.TaskAssignID == null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/TaskAssign/Insert");
                    string model = JsonConvert.SerializeObject(taskAssignModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "TaskAssign Inserted Sucessfully";
                        return RedirectToAction("TaskAssignList");
                    }
                }
                else
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, baseAddres + "/TaskAssign/Update");
                    string model = JsonConvert.SerializeObject(taskAssignModel);
                    var content = new StringContent(model, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "TaskAssign Updated Sucessfully";
                        return RedirectToAction("TaskAssignList");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error occur" + ex.Message;
            }
            return RedirectToAction("TaskAssignList");
        }


        [HttpPost]
        public async Task<IActionResult> Filter(TaskAssignModel taskAssign)
        {
            List<TaskAssignModel> taskAssignsModel = new List<TaskAssignModel>();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/TaskAssign/Post");
            string model = JsonConvert.SerializeObject(taskAssign);
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
                taskAssignsModel = JsonConvert.DeserializeObject<List<TaskAssignModel>>(extractdDatajson);
            }
            ViewBag.statusList = StatusList();
            ViewBag.projectWistTaskList = ProjectWiseTaskList();
            ViewBag.employeelist = EmployeeList();
            return View("TaskAssignList", taskAssignsModel);
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
        }

        [HttpGet]
        public List<ProjectWiseTaskDropDownModel> ProjectWiseTaskList()
        {
            List<ProjectWiseTaskDropDownModel> projectWiseTasklist = new List<ProjectWiseTaskDropDownModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/ProjectWiseTask/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                projectWiseTasklist = JsonConvert.DeserializeObject<List<ProjectWiseTaskDropDownModel>>(extractdDatajson);

            }
            return projectWiseTasklist;
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
