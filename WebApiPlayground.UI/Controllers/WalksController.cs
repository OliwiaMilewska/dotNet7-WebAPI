using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using WebApiPlayground.UI.Models.DTO;

namespace WebApiPlayground.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WalksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var response = new List<WalkDto>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponseMesage = await client.GetAsync("https://localhost:7015/api/v1/Walks?pageNumber=1&pageSize=1000");
                httpResponseMesage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMesage.Content.ReadFromJsonAsync<IEnumerable<WalkDto>>());
            }
            catch (Exception)
            {
                throw;
            }

            return View(response);
        }
    }
}
