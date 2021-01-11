using MarsRover.Config.Models;
using MarsRover.Core.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace MarsRover.Core.Services
{
    public class MarsRoverClient: IMarsRoverClient
    {
        private readonly HttpClient httpClient;
        private readonly ApiKeyConfig apiKeyConfig;

        public MarsRoverClient(HttpClient httpClient, ApiKeyConfig apiKeyConfig)
        {
            this.httpClient = httpClient;
            this.apiKeyConfig = apiKeyConfig;
        }

        public async IAsyncEnumerable<MarsRoverPhotoResponseViewModel> GetImagesAsync(DateTime date, int page = 1)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var response = await this.httpClient.GetAsync($"rovers/curiosity/photos?earth_date={formattedDate}&page={page}&api_key={this.apiKeyConfig.Key}");
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
