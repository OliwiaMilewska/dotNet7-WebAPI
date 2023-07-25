using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApiPlayground.UI.Models;
using WebApiPlayground.UI.Models.DTO;

namespace WebApiPlayground.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public WalksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = new List<WalkDto>();

            try
            {
                var httpResponseMesage = await _httpClient.GetAsync("https://localhost:7015/api/v1/Walks?pageNumber=1&pageSize=1000");
                httpResponseMesage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMesage.Content.ReadFromJsonAsync<IEnumerable<WalkDto>>());
            }
            catch (Exception)
            {
                throw;
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddWalkViewModel model)
        {
            var cont = new StringContent(JsonSerializer.Serialize(model), encoding: Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7015/api/v1/Walks"),
                Content = new StringContent(JsonSerializer.Serialize(model), encoding: Encoding.UTF8, "application/json")
            };

            var httpResponseMesage = await _httpClient.SendAsync(httpRequestMessage);
            httpResponseMesage.EnsureSuccessStatusCode();

            var response = await httpResponseMesage.Content.ReadFromJsonAsync<WalkDto>();

            if (response is not null)
                return RedirectToAction("Index", "Walks");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<WalkDto>($"https://localhost:7015/api/v1/Walks/{id}");
            return response is not null ? View(response) : View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WalkDto walkDomain)
        {
            var walk = await _httpClient.GetFromJsonAsync<WalkDto>($"https://localhost:7015/api/v1/Walks/{walkDomain.Id}");

            var walkDto = new AddWalkViewModel
            {
                Description = walkDomain.Description,
                Name = walkDomain.Name,
                LengthInKm = walkDomain.LengthInKm,
                WalkImageUrl = walkDomain.WalkImageUrl,
                RegionId = walk.Region.Id,
                DifficultyId = walk.Difficulty.Id
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7015/api/v1/Walks/{walkDomain.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(walkDto), encoding: Encoding.UTF8, "application/json")
            };

            var httpResponseMesage = await _httpClient.SendAsync(request);
            httpResponseMesage.EnsureSuccessStatusCode();

            var response = await httpResponseMesage.Content.ReadFromJsonAsync<WalkDto>();

            if (response is not null)
                return RedirectToAction("Index", "Walks");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7015/api/v1/Walks/{id}");
            return RedirectToAction("Index", "Walks");
        }
    }
}