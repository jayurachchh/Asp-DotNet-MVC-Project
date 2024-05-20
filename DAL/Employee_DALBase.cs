using Newtonsoft.Json;
using Project_Management2.Areas.Employee.Models;
using System.Text;

namespace Project_Management2.DAL
{
    public class Employee_DAlBase
    {
        static string apiUrl = "http://www.projectmanagementadminpanel.somee.com/api/EmployeeControllers";

        /*public static async Task<dynamic> GetEmployee(int? id = null)
        {
            var client = new HttpClient();
            string url = apiUrl + (id == null ? "" : ("/" +id.ToString()));
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();
            if (id == null)
            {
                ApiResponse api = JsonConvert.DeserializeObject<ApiResponse>(responseData);
                return api.data;
            }
            else
            {
                Areas.Employee.Models.Employee emp = JsonConvert.DeserializeObject<Areas.Employee.Models.Employee>(responseData);
                return emp;
            }
        }
        public static async Task DeleteEmployee(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl + "/" + id.ToString());
            var response = await client.SendAsync(request);
            Console.WriteLine(response.EnsureSuccessStatusCode());
        }
        public static async Task AddUpdateA(int? id = null, Employee emp = null)
        {
            var client = new HttpClient();


            string endpoint = id != null ? $"/{id}" : "";
            string url = apiUrl + endpoint;

            string jsonContent = JsonConvert.SerializeObject(emp);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(id != null ? HttpMethod.Put : HttpMethod.Post, url);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }*/
    }  
}
