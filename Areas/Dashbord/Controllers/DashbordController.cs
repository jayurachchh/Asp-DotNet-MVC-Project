using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Management2.Areas.Dashbord.Models;
using Project_Management2.Areas.Employee.Models;

namespace Project_Management2.Areas.Dashbord.Controllers
{
    [Area("Dashbord")]
    [Route("Dashbord/[controller]/[action]")]
    public class DashbordController : Controller
    {
        Uri baseAddres = new Uri("https://localhost:7149/api/Dashbord");
        private readonly HttpClient _client;
        public DashbordController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;
        }

        [HttpGet]
        public IActionResult EmployeeList()
        {
            DashbordModel dashbord = new DashbordModel();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dashbord = JsonConvert.DeserializeObject<List<DashbordModel>>(extractdDatajson);

            }
            return View(dashbord);
        }
    }
}
