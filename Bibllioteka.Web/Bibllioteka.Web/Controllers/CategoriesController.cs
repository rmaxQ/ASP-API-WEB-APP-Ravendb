using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Biblioteka.Web.Controllers
{
    public class CategoriesController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;

        public CategoriesController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Category> Categories = new List<Category>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Categories/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Categories = JsonConvert.DeserializeObject<List<Category>>(data);

            }
            return View(Categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Categories/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Category created successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            
            Category Category = new Category();
            
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Categories/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Category = JsonConvert.DeserializeObject<Category>(data);
            }
            return View(Category);
        }

        [HttpPost]
        public IActionResult Edit(string id, Category model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Categories/Put/" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Category edited successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        [HttpGet]
        public IActionResult Remove(string id)
        {
            try
            {
                Category Category = new Category();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Categories/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Category = JsonConvert.DeserializeObject<Category>(data);
                }
                return View(Category);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Categories/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Category deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
    }
}

