using System.Data.SqlClient;
using LocationTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocationTask.Controllers
{
    public class LocationController : Controller
    {
        private readonly HttpClient _httpClient;

        public LocationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7174/");
        }

        public async Task<IActionResult> Index()
        {
            var states = await _httpClient.GetFromJsonAsync<List<StateModel>>("https://localhost:7174/api/locationapi/states");
            ViewBag.States = states;
            return View();
        }

        [HttpGet]
        [Route("Location/GetCities")]
        public async Task<JsonResult> GetCities(int stateId)
        {
            var cities = await _httpClient.GetFromJsonAsync<List<CityModel>>($"https://localhost:7174/api/locationapi/cities/{stateId}");
            return Json(cities);
        }

        [HttpGet]
        [Route("Location/GetTaluks")]
        public async Task<JsonResult> GetTaluks(int cityId)
        {
            var taluks = await _httpClient.GetFromJsonAsync<List<TalukModel>>($"https://localhost:7174/api/locationapi/taluks/{cityId}");
            return Json(taluks);
        }
    }
}
