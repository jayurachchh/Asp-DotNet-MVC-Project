using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Management2.Areas.Employee.Models;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = iTextSharp.text.Font;
using Paragraph = iTextSharp.text.Paragraph;
using Document = iTextSharp.text.Document;
using System.Net.Http;
using System.Text;
using DocumentFormat.OpenXml.Vml;
using System.IO;
using Path = System.IO.Path;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace Project_Management2.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("Employee/[controller]/[action]")]

    public class EmployeeController : Controller
    {
        Uri baseAddres = new Uri("https://localhost:7149/api/Employee");
        // Uri baseAddres1 = new Uri("https://localhost:7149/api/Employee/Update");

        //Uri baseAddres = new Uri("http://www.projectmanagementadminpanel.somee.com/api/EmployeeControllers");
        private readonly HttpClient _client;
        public EmployeeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;
        }

        [HttpGet]
        public IActionResult EmployeeList(int? id)
        {
            /*id = 1053;*/
            Employee_Main employee_Main = new Employee_Main();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Get{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;

                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Emp_Info_By_ID = JsonConvert.DeserializeObject<Emp_Info_By_ID>(extractdDatajson);
                    ViewBag.Data = employee_Main.Emp_Info_By_ID;
                              
                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);
                }
            }
            //TempData["EmpData"] = ;
            return View(employee_Main);
        }

        [HttpGet]
        public IActionResult TaskNotAssignEmployeeList(int? id)
        {
            /*id = 1053;*/
            Employee_Main employee_Main = new Employee_Main();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Gettasknotassign{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;

                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Emp_Info_By_ID = JsonConvert.DeserializeObject<Emp_Info_By_ID>(extractdDatajson);
                    ViewBag.Data = employee_Main.Emp_Info_By_ID;

                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);
                }
            }
            //TempData["EmpData"] = ;
            return View(employee_Main);
        }

        [HttpGet]
        public IActionResult TaskAssignEmployeeList(int? id)
        {
            /*id = 1053;*/
            Employee_Main employee_Main = new Employee_Main();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Gettaskassign{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;

                if (id != null)
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Emp_Info_By_ID = JsonConvert.DeserializeObject<Emp_Info_By_ID>(extractdDatajson);
                    ViewBag.Data = employee_Main.Emp_Info_By_ID;

                }
                else
                {
                    var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);
                }
            }
            //TempData["EmpData"] = ;
            return View(employee_Main);
        }

        [HttpGet]
        public IActionResult Employeebyid(int? id)
        {
            /*id = 1053;*/
            Employee_Main employee_Main = new Employee_Main();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Get{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employee_Main.Emp_Info_By_ID = JsonConvert.DeserializeObject<Emp_Info_By_ID>(extractdDatajson);
 
            }
            return PartialView("_EmployeeInfoParitialview", employee_Main.Emp_Info_By_ID);

        }

        public DataTable EmployeeList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Get").Result;

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

        public FileResult Export_Employee_List_To_Excel()
        {
            DataTable dataTable = EmployeeList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Employee Statements");

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

                    string fileName = "Employee_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_Employee_List_To_pdf()
        {
            DataTable dataTable = EmployeeList();
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
                    Paragraph title = new Paragraph("Employee List", new Font(boldBaseFont, 35));
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
                string fileName = "EmployeeList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }


        public DataTable EmployeeTaskNotAssignList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Gettasknotassign").Result;

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

        public FileResult Export_EmployeeTaskNotAssignList_List_To_Excel()
        {
            DataTable dataTable = EmployeeTaskNotAssignList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Employee Statements");

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

                    string fileName = "EmployeeTaskNotAssign_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_EmployeeTaskNotAssignList_List_To_pdf()
        {
            DataTable dataTable = EmployeeTaskNotAssignList();
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
                    Paragraph title = new Paragraph("EmployeeTaskNotAssign List", new Font(boldBaseFont, 35));
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
                string fileName = "EmployeeTasknotAssignList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }


        }


        public DataTable EmployeeTaskAssignList()
        {

            DataTable dataTable = new DataTable();

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Gettaskassign").Result;

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

        public FileResult Export_EmployeeTaskAssignList_List_To_Excel()
        {
            DataTable dataTable = EmployeeTaskAssignList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Employee Statements");

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

                    string fileName = "EmployeeTaskAssign_Lists.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public FileResult Export_EmployeeTaskAssignList_List_To_pdf()
        {
            DataTable dataTable = EmployeeTaskAssignList();
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
                    Paragraph title = new Paragraph("EmployeeTaskAssign List", new Font(boldBaseFont, 35));
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
                string fileName = "EmployeeTaskAssignList.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }





        [HttpGet]
        /*       public IActionResult Delete(string[] EmpIDlist)
               {
                   HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{EmpIDlist}").Result;
                   if (response.IsSuccessStatusCode)
                   {
                       TempData["Message"] = "Employee Deleted Sucessfully";
                   }

                   return RedirectToAction("EmployeeList");
               }*/
        public IActionResult Delete(string[] EmpIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (EmpIDlist != null && EmpIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", EmpIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Employees Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Employees";
                }
            }
            else
            {
                TempData["Message1"] = "No Employees Selected for Deletion";
            }

            return RedirectToAction("EmployeeList");

        }



        [HttpGet]
        public IActionResult DeleteTasknotAssign(string[] EmpIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (EmpIDlist != null && EmpIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", EmpIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Employees Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Employees";
                }
            }
            else
            {
                TempData["Message1"] = "No Employees Selected for Deletion";
            }
            return RedirectToAction("TaskNotAssignEmployeeList");

        }


        [HttpGet]
        public IActionResult DeleteTaskAssign(string[] EmpIDlist)
        {
            // Check if EmpIDlist is not null and contains IDs
            if (EmpIDlist != null && EmpIDlist.Length > 0)
            {
                // Convert the array of IDs to a comma-separated string
                string idList = string.Join(",", EmpIDlist);

                // Call the API to delete the multiple employees
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Delete{idList}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Employees Deleted Successfully";
                }
                else
                {
                    TempData["Message1"] = "Failed to Delete Employees";
                }
            }
            else
            {
                TempData["Message1"] = "No Employees Selected for Deletion";
            }
            return RedirectToAction("TaskAssignEmployeeList");

        }

   
        public IActionResult EmployeeAddedit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Employeeedit(int? EmpID)
        {
            EmployeeModel employees = new EmployeeModel();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Get{EmpID}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employees = JsonConvert.DeserializeObject<EmployeeModel>(extractdDatajson);
            }
            return View("EmployeeAddedit", employees);
        }

      
        // upload comment code
        /*        [HttpPost]
                public async Task<IActionResult> Save(EmployeeModel employeeModel)
                {
                    try
                    {
                        var client = new HttpClient();

                        if (employeeModel.EmpID == null)
                        {
                            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Insert");
                            string model = JsonConvert.SerializeObject(employeeModel);
                            var content = new StringContent(model, null, "application/json");
                            request.Content = content;
                            var response = await client.SendAsync(request);
                            response.EnsureSuccessStatusCode();
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Message"] = "Employee Inserted Sucessfully";
                                return RedirectToAction("EmployeeList");
                            }
                        }
                        else
                        {
                            var request = new HttpRequestMessage(HttpMethod.Put, baseAddres + "/Update");
                            string model = JsonConvert.SerializeObject(employeeModel);
                            var content = new StringContent(model, null, "application/json");
                            request.Content = content;
                            var response = await client.SendAsync(request);
                            response.EnsureSuccessStatusCode();
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Message"] = "Employee Updated Sucessfully";
                                return RedirectToAction("EmployeeList");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "An Error occur" + ex.Message;
                    }
                    return RedirectToAction("EmployeeList");
                }*/
        /*


                [HttpPost]
                public async Task<IActionResult> Save(EmployeeModel employeeModel)
                {
                    try
                    {
                        var client = new HttpClient();
                        var formData = new MultipartFormDataContent();
                        // Check if Profile Image File Exists
                        if (employeeModel.EmpProfileImagefile != null && employeeModel.EmpProfileImagefile.Length > 0)
                        {
                            // Create a unique file name for the profile image
                            var profileImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeModel.EmpProfileImagefile.FileName);

                            // Save the uploaded profile image to a location on the server
                            var profileImagePath = Path.Combine("uploads", profileImageFileName);
                            using (var stream = new FileStream(profileImagePath, FileMode.Create))
                            {
                                await employeeModel.EmpProfileImagefile.CopyToAsync(stream);
                            }

                            // Set the profile image path in the employee model
                            employeeModel.EmpProfileImage = profileImagePath;

                            // Add the profile image file to form data with the unique file name
                            formData.Add(new StreamContent(employeeModel.EmpProfileImagefile.OpenReadStream()), "EmpProfileImage", profileImageFileName);
                        }

                        // Check if Proof Image File Exists
                        if (employeeModel.EmpProofImagefile != null && employeeModel.EmpProofImagefile.Length > 0)
                        {
                            // Create a unique file name for the proof image
                            var proofImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeModel.EmpProofImagefile.FileName);

                            // Save the uploaded proof image to a location on the server
                            var proofImagePath = Path.Combine("uploads", proofImageFileName);
                            using (var stream = new FileStream(proofImagePath, FileMode.Create))
                            {
                                await employeeModel.EmpProofImagefile.CopyToAsync(stream);
                            }

                            // Set the proof image path in the employee model
                            employeeModel.EmpProofImage = proofImagePath;

                            // Add the proof image file to form data with the unique file name
                            formData.Add(new StreamContent(employeeModel.EmpProofImagefile.OpenReadStream()), "EmpProofImage", proofImageFileName);
                        }

                        // Create and send the request
                        var request = new HttpRequestMessage(employeeModel.EmpID == null ? HttpMethod.Post : HttpMethod.Put,
                            _client.BaseAddress + (employeeModel.EmpID == null ? "/Insert" : "/Update"));
                        string model = JsonConvert.SerializeObject(employeeModel);
                        var content = new StringContent(model, Encoding.UTF8, "application/json");
                        request.Content = content;

                        // Send the request
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = employeeModel.EmpID == null ? "Employee Inserted Successfully" : "Employee Updated Successfully";
                            return RedirectToAction("EmployeeList");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "An Error occurred: " + ex.Message;
                    }
                    return RedirectToAction("EmployeeList");
                }

                private async Task<Dictionary<string, dynamic>> UploadImageAsync(IFormFile file)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    try
                    {
                        dict.Add("status", false);

                        if (file == null || file.Length == 0)
                        {
                            return dict;
                        }

                        if (!file.ContentType.ToLower().StartsWith("image/"))
                        {
                            return dict;
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        dict.Add("filePath", filePath);
                        dict["status"] = true;
                        return dict;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return dict;
                    }
                }*/

        [HttpPost]
        public async Task<IActionResult> Save(EmployeeModel employeeModel)
        {
            try
            {
                var client = new HttpClient();

                var formData = new MultipartFormDataContent();

                /*                // Add EmpProfileImage
                                if (employeeModel.EmpProfileImagefile != null && employeeModel.EmpProfileImagefile.Length > 0)
                                {
                                    formData.Add(new StreamContent(employeeModel.EmpProfileImagefile.OpenReadStream()), "EmpProfileImage", employeeModel.EmpProfileImagefile.FileName);
                                }

                                // Add EmpProofImage
                                if (employeeModel.EmpProofImagefile != null && employeeModel.EmpProofImagefile.Length > 0)
                                {
                                    formData.Add(new StreamContent(employeeModel.EmpProofImagefile.OpenReadStream()), "EmpProofImage", employeeModel.EmpProofImagefile.FileName);
                                }*/

                // Check if Profile Image File Exists
                if (employeeModel.EmpProfileImagefile != null && employeeModel.EmpProfileImagefile.Length > 0)
                {
                    // Create a unique file name for the profile image
                    var profileImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeModel.EmpProfileImagefile.FileName);

                    // Save the uploaded profile image to a location on the server
                    var profileImagePath = Path.Combine("uploads", profileImageFileName);
                    using (var stream = new FileStream(profileImagePath, FileMode.Create))
                    {
                        await employeeModel.EmpProfileImagefile.CopyToAsync(stream);
                    }

                    // Set the profile image path in the employee model
                    employeeModel.EmpProfileImage = profileImagePath;

                    // Add the profile image file to form data with the unique file name
                    formData.Add(new StreamContent(employeeModel.EmpProfileImagefile.OpenReadStream()), "EmpProfileImage", profileImageFileName);
                }

                // Check if Proof Image File Exists
                if (employeeModel.EmpProofImagefile != null && employeeModel.EmpProofImagefile.Length > 0)
                {
                    // Create a unique file name for the proof image
                    var proofImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeModel.EmpProofImagefile.FileName);

                    // Save the uploaded proof image to a location on the server
                    var proofImagePath = Path.Combine("uploads", proofImageFileName);
                    using (var stream = new FileStream(proofImagePath, FileMode.Create))
                    {
                        await employeeModel.EmpProofImagefile.CopyToAsync(stream);
                    }

                    // Set the proof image path in the employee model
                    employeeModel.EmpProofImage = proofImagePath;

                    // Add the proof image file to form data with the unique file name
                    formData.Add(new StreamContent(employeeModel.EmpProofImagefile.OpenReadStream()), "EmpProofImage", proofImageFileName);
                }


                // Add other form fields
                formData.Add(new StringContent(employeeModel.EmpID.ToString()), "EmpID");
                formData.Add(new StringContent(employeeModel.EmpName), "EmpName");
                formData.Add(new StringContent(employeeModel.EmpCode), "EmpCode");
                formData.Add(new StringContent(employeeModel.EmpPosition), "EmpPosition");
                formData.Add(new StringContent(employeeModel.EmpContact), "EmpContact");
                formData.Add(new StringContent(employeeModel.EmpEmail), "EmpEmail");
                formData.Add(new StringContent(employeeModel.EmpDepartment), "EmpDepartment");
                formData.Add(new StringContent(employeeModel.EmpDateOfBirth.ToString()), "EmpDateOfBirth");
                formData.Add(new StringContent(employeeModel.EmpProfileImage), "EmpProfileImage");
                formData.Add(new StringContent(employeeModel.EmpProofName), "EmpProofName");
                formData.Add(new StringContent(employeeModel.EmpProofImage), "EmpProofImage");
                formData.Add(new StringContent(employeeModel.EmpManagerId), "EmpManagerId");
                formData.Add(new StringContent(employeeModel.EmpPerHourCharge), "EmpPerHourCharge");
                formData.Add(new StringContent(employeeModel.EmpGitLink), "EmpGitLink");
                // Add other fields similarly

                // Create and send the request
                var response = await client.PostAsync(_client.BaseAddress + (employeeModel.EmpID == null ? "/Insert" : "/Update"), formData);
                response.EnsureSuccessStatusCode();

                Console.WriteLine(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = employeeModel.EmpID == null ? "Employee Inserted Successfully" : "Employee Updated Successfully";
                    return RedirectToAction("EmployeeList");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error occurred: " + ex.Message;
            }

            return RedirectToAction("EmployeeList");
        }
        [HttpPost]
        public async Task<IActionResult> Filter(EmployeeModel employee)
        {
            Employee_Main employee_Main = new Employee_Main();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/Post");
            string model = JsonConvert.SerializeObject(employee);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                /* var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                 employees=JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);*/

                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);

            }
            return View("EmployeeList", employee_Main);
        }

        [HttpPost]
        public async Task<IActionResult> TaskNotAssignFilter(EmployeeModel employee)
        {
            Employee_Main employee_Main = new Employee_Main();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/TaskNotassignFilter");
            string model = JsonConvert.SerializeObject(employee);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                /* var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                 employees=JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);*/

                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);

            }
            return View("TaskNotAssignEmployeeList", employee_Main);
        }

        [HttpPost]
        public async Task<IActionResult> TaskAssignFilter(EmployeeModel employee)
        {
            Employee_Main employee_Main = new Employee_Main();
            //HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}").Result;
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddres + "/TaskassignFilter");
            string model = JsonConvert.SerializeObject(employee);
            var content = new StringContent(model, null, "application/json");
            request.Content = content;
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                /* var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                 employees=JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);*/

                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                employee_Main.Employee = JsonConvert.DeserializeObject<List<EmployeeModel>>(extractdDatajson);

            }
            return View("TaskAssignEmployeeList", employee_Main);
        }



    }
}
