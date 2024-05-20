using Microsoft.AspNetCore.Mvc;
using Project_Management2.Models;
using System.Diagnostics;
using Project_Management2.BAL;
using Newtonsoft.Json;
using Project_Management2.Areas.TaskAssign.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Project_Management2.Areas.Dashbord.Models;
using Project_Management2.Areas.Project.Models;
using Microsoft.Build.ObjectModelRemoting;

namespace Project_Management2.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        Uri baseAddres = new Uri("https://localhost:7149/api");


        private readonly HttpClient _client;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = baseAddres;

        }

        [HttpGet]
        public List<TaskAssignModel> TaskAssignList()
        {
            List<TaskAssignModel> taskAssignModels = new List<TaskAssignModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/TaskAssign/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                taskAssignModels = JsonConvert.DeserializeObject<List<TaskAssignModel>>(extractdDatajson);
            }
            return taskAssignModels;
        }

        [HttpGet]
        public List<DashbordModel> DashbordList()
        {
            List<DashbordModel> dashbord = new List<DashbordModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Dashbord/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dashbord = JsonConvert.DeserializeObject<List<DashbordModel>>(extractdDatajson);
            }
            return dashbord;
        }
        [HttpGet]
        public List<ProjectModel> ProjectList()
        {
            List<ProjectModel> project = new List<ProjectModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Project/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                project = JsonConvert.DeserializeObject<List<ProjectModel>>(extractdDatajson);

            }
            return project;
        }
      /*  [CheckAccess]*/
        public IActionResult Index()
        {
            ViewBag.DashbordData = DashbordList();
            List<TaskAssignModel> taskAssigns = TaskAssignList();
            List<ProjectModel> projects = ProjectList();
            List<RecentAction> recent = ActionList();
            var model = new Tuple<IEnumerable<TaskAssignModel>, IEnumerable<ProjectModel>,IEnumerable<RecentAction>>(taskAssigns, projects,recent);
            return View(model);

        }               
        [HttpGet]
        public List<RecentAction> ActionList()
        {
            List<RecentAction> recent = new List<RecentAction>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/RecentAction/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extractdDatajson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                recent = JsonConvert.DeserializeObject<List<RecentAction>>(extractdDatajson);

            }
            return recent;
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }
        public IActionResult profile()
        {
            return View();
        }
        public IActionResult Faq()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}