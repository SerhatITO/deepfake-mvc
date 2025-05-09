using Microsoft.AspNetCore.Mvc;
using DeepfakeWebApp.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeepfakeWebApp.Controllers
{
    public class VideoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VideoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadModel model)
        {
            if (model.VideoFile == null || model.VideoFile.Length == 0)
            {
                ViewBag.Message = "Lütfen bir video dosyası yükleyin.";
                return View();
            }

            var client = _httpClientFactory.CreateClient();

            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(model.VideoFile.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("video/mp4");

                content.Add(fileContent, "video", model.VideoFile.FileName);

                try
                {
                    var response = await client.PostAsync("http://127.0.0.1:5050/analyze", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "API'den cevap alınamadı.";
                        return View();
                    }

                    dynamic json = JsonConvert.DeserializeObject(responseString);
                    ViewBag.Label = json.result;
                    ViewBag.Confidence = json.confidence;
                }
                catch
                {
                    ViewBag.Message = "C# Web API şu anda erişilemiyor.";
                }
            }

            return View();
        }
    }
}
