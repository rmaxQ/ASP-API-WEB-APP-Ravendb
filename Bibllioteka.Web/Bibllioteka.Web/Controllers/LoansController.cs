using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Biblioteka.Api.Indices;

namespace Biblioteka.Web.Controllers
{
    public class LoansController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;

        public LoansController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Loan> Loans = new List<Loan>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Loans/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Loans = JsonConvert.DeserializeObject<List<Loan>>(data);

            }
            return View(Loans);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<Members_WithIdAndName.Result> members = new List<Members_WithIdAndName.Result>();
            List<Books_WithIdAndTitleAndAuthor.Result> books = new List<Books_WithIdAndTitleAndAuthor.Result>();

            HttpResponseMessage responseB = _httpClient.GetAsync(_httpClient.BaseAddress + "/Books_WithIdAndTitleAndAuthorResult/Get/").Result;
            HttpResponseMessage responseM = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members_WithIdAndNameResult/Get/").Result;
            
            if (responseB.IsSuccessStatusCode)
            {
                string dataB = responseB.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<Books_WithIdAndTitleAndAuthor.Result>>(dataB);
                ViewBag.Books = books;
            }
            if (responseM.IsSuccessStatusCode)
            {
                string dataM = responseM.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Members_WithIdAndName.Result>>(dataM);
                ViewBag.Members = members;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Loan model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Loans/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loan created successfully";
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

            Loan Loan = new Loan();

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Loans/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Loan = JsonConvert.DeserializeObject<Loan>(data);
                List<Members_WithIdAndName.Result> members = new List<Members_WithIdAndName.Result>();
                List<Books_WithIdAndTitleAndAuthor.Result> books = new List<Books_WithIdAndTitleAndAuthor.Result>();
                HttpResponseMessage responseB = _httpClient.GetAsync(_httpClient.BaseAddress + "/Books_WithIdAndTitleAndAuthorResult/Get/").Result;
                HttpResponseMessage responseM = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members_WithIdAndNameResult/Get/").Result;

                if (responseB.IsSuccessStatusCode)
                {
                    string dataB = responseB.Content.ReadAsStringAsync().Result;
                    books = JsonConvert.DeserializeObject<List<Books_WithIdAndTitleAndAuthor.Result>>(dataB);
                    ViewBag.Books = books;
                }
                if (responseM.IsSuccessStatusCode)
                {
                    string dataM = responseM.Content.ReadAsStringAsync().Result;
                    members = JsonConvert.DeserializeObject<List<Members_WithIdAndName.Result>>(dataM);
                    ViewBag.Members = members;
                }
            }
            return View(Loan);
        }

        [HttpPost]
        public IActionResult Edit(string id, Loan model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Loans/Put/" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loan edited successfully";
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
                Loan Loan = new Loan();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Loans/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Loan = JsonConvert.DeserializeObject<Loan>(data);
                }
                return View(Loan);
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
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Loans/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loan deleted successfully";
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

