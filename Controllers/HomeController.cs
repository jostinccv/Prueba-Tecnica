using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        
        public async Task<ActionResult> Index()
        {
            //https://localhost:44312/api/Books
             var httpclient = new HttpClient();
             var json = await httpclient.GetStringAsync("https://localhost:44312/api/Books");
             List<Books> BooksLits = JsonConvert.DeserializeObject<List<Books>>(json);
             return View(BooksLits);
        }

     
        public async Task<ActionResult> Detalles(int id)
        {
            var httpclient = new HttpClient();
            var json = await httpclient.GetStringAsync("https://localhost:44312/api/Books/"+id);
            var book = JsonConvert.DeserializeObject<Books>(json);
            return View(book);
        }

        
        public ActionResult Create(Books books)
        {
            
            using (var httpclient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(books), Encoding.UTF8, "application/json");

                HttpResponseMessage responde = httpclient.PostAsync("https://localhost:44312/api/Books", content).Result;

                if (responde.IsSuccessStatusCode)
                {
                 return Redirect("~/Home/Index");
                }

            }
            return View();

        }

        public async Task<ActionResult> Editar(int id)
        {
            var httpclient = new HttpClient();
            var json = await httpclient.GetStringAsync("https://localhost:44312/api/Books/" + id);
            var book = JsonConvert.DeserializeObject<Books>(json);
            return View(book);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(int id, Books books)
        {
           Books booker = new Books();

            using (var Httpclient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(books), Encoding.UTF8, "application/json");
                using (var respon = await Httpclient.PutAsync("https://localhost:44312/api/Books/"+id, content))
                {
                    string apiResponse = await respon.Content.ReadAsStringAsync();
                    booker = JsonConvert.DeserializeObject<Books>(apiResponse);
                }
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult Delete(int id)
        {   
            var httpclient = new HttpClient();
            var json =  httpclient.DeleteAsync("https://localhost:44312/api/Books/"+id.ToString());
            var result = json.Result;

            if (result.IsSuccessStatusCode)
            {
                return Redirect("~/home/Index");
            }

            return Redirect("~/home/Index");
        }
        
        


    }
}