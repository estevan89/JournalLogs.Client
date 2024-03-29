﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using client.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient = new HttpClient();

        public HomeController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await GetJournalLogs();
            return View(logs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private async Task<List<JournalLog>> GetJournalLogs()
        {
            var token = await _tokenService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
           
            var res = await _httpClient.GetAsync("https://localhost:7001/api/values");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var journalLogs = JsonConvert.DeserializeObject<List<JournalLog>>(content);
                return journalLogs;
            }
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
