using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using Biblioteka.Model;
using Biblioteka.Web.Models;

namespace Biblioteka.Web.Controllers
{
    public class MembersController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;

        public MembersController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 2)
        {
            PagedResult<Member> result = null;
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Members/Get?skip={page}&take={pageSize}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<PagedResult<Member>>(data);
            }

            var viewModel = new MemberViewModel
            {
                Members = result.Items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = result.TotalCount
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Member model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Members/Post", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Member created successfully";
                    return RedirectToAction("Index",1);
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

            Member Member = new Member();

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Member = JsonConvert.DeserializeObject<Member>(data);
            }
            return View(Member);
        }

        [HttpPost]
        public IActionResult Edit(string id, Member model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Members/Put/" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Member edited successfully";
                    return RedirectToAction("Index",1);
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
                Member Member = new Member();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Members/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Member = JsonConvert.DeserializeObject<Member>(data);
                }
                return View(Member);
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
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Members/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Member deleted successfully";
                    return RedirectToAction("Index",1);
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

