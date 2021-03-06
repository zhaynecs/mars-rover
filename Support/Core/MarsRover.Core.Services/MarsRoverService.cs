﻿using MarsRover.Config.Models;
using MarsRover.Core.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public class MarsRoverService: IMarsRoverService
    {
        private readonly HttpClient httpClient;
        private readonly IApiKeyConfig apiKeyConfig;

        public MarsRoverService(HttpClient httpClient, IApiKeyConfig apiKeyConfig)
        {
            this.httpClient = httpClient;
            this.apiKeyConfig = apiKeyConfig;
        }

        public async Task<MarsRoverPhotoResponseViewModel> GetImageDataAsync(string rover, DateTime date, int page = 1)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var response = await this.httpClient.GetAsync($"rovers/{rover}/photos?earth_date={formattedDate}&page={page}&api_key={this.apiKeyConfig.Key}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseString))
                throw new Exception("Empty response from API");

            return JsonConvert.DeserializeObject<MarsRoverPhotoResponseViewModel>(responseString);
        }

        public async IAsyncEnumerable<MarsRoverPhotoResponseViewModel> GetImageDataStreamAsync(string rover, DateTime date, int page = 1)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var response = await this.httpClient.GetAsync($"rovers/{rover}/photos?earth_date={formattedDate}&page={page}&api_key={this.apiKeyConfig.Key}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            if (responseStream == null || !responseStream.CanRead)
                throw new Exception("Empty response from API");

            using (var streamReader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                yield return new JsonSerializer().Deserialize<MarsRoverPhotoResponseViewModel>(jsonReader);
            }
        }
    }
}
