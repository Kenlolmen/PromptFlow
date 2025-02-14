using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutomationForPromptingApi.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using AutomationForPromptingApi.Exceptions;


namespace AutomationForPromptingApi.Service
{
    public class OpenAiFileService : IOpenAiFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAiFileService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                      ?? configuration["OpenAiSettings:ApiKey"];
            _model = configuration["OpenAiSettings:Model"];
        }

        public async Task<string> SendToChatGptAsync(List<string> keywords)
        {
            var requestData = new
            {
                model = _model, 
                messages = new[] 
                {
                    new { role = "system", content = "You are an AI assistant." },
                    new { role = "user", content = string.Join(", ", keywords) }
                },
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestData);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Headers = { { "Authorization", $"Bearer {_apiKey}" } },
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

            return jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }

        public async Task<string> GetChatGptResponse(string prompt)
        {
            if (string.IsNullOrEmpty(_apiKey))
                throw new ApiKeyIsInvalidException();

            var requestData = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestData), 
                Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new OpenAiFailedResponseException();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
