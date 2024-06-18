using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Biblioteka.Web.Controllers
{
    public class BooksController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;

        public BooksController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Book> Books = new List<Book>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Books/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Books = JsonConvert.DeserializeObject<List<Book>>(data);

            }
            return View(Books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<CategoryBookCountResult> category = new List<CategoryBookCountResult>();

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryBookCountResults/GetCategoryBookCounts/").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<List<CategoryBookCountResult>>(data);
                ViewBag.Categories = category;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Books/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Book created successfully";
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

            Book Book = new Book();

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Books/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                List<CategoryBookCountResult> category = new List<CategoryBookCountResult>();

                HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryBookCountResults/GetCategoryBookCounts/").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data2 = response2.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<List<CategoryBookCountResult>>(data2);
                    ViewBag.Categories = category;
                }
                string data = response.Content.ReadAsStringAsync().Result;
                Book = JsonConvert.DeserializeObject<Book>(data);
                
            }
            return View(Book);
        }

        [HttpPost]
        public IActionResult Edit(string id, Book model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Books/Put/" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Book edited successfully";
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
                Book Book = new Book();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Books/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Book = JsonConvert.DeserializeObject<Book>(data);
                }
                return View(Book);
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
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Books/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Book deleted successfully";
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

