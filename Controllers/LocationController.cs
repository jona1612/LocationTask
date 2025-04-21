using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using LocationTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocationTask.Controllers
{
    public class LocationController : Controller
    {
        private readonly string _connectionString;

        public LocationController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var states = GetStates();
            ViewBag.States = states;
            return View();
        }

        [HttpGet]
        [Route("Location/GetCities")]
        public JsonResult GetCities(int stateId)
        {
            var cities = new List<CityModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT CityId, CityName FROM LocationDetails WHERE StateId = @stateId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@stateId", stateId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cities.Add(new CityModel
                    {
                        CityId = Convert.ToInt32(rdr["CityId"]),
                        CityName = rdr["CityName"].ToString()
                    });
                }
            }
            return Json(cities);
        }


        [HttpGet]
        [Route("Location/GetTaluks")]
        public JsonResult GetTaluks(int cityId){
    var taluks = new List<TalukModel>();
    using (SqlConnection con = new SqlConnection(_connectionString))
    {
        string query = "SELECT DISTINCT TalukId, TalukName FROM LocationDetails WHERE CityId = @cityId";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cityId", cityId);
        con.Open();
        SqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            taluks.Add(new TalukModel
            {
                TalukId = Convert.ToInt32(rdr["TalukId"]),
                TalukName = rdr["TalukName"].ToString()
            });
        }
    }
    return Json(taluks);
}


        public List<StateModel> GetStates()
        {
            var states = new List<StateModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT StateId, StateName FROM LocationDetails";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    states.Add(new StateModel
                    {
                        StateId = Convert.ToInt32(rdr["StateId"]),
                        StateName = rdr["StateName"].ToString()
                    });
                }
            }
            return states;
        }
    }
}
