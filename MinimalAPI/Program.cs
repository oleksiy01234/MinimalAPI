namespace MinimalAPI
{
    using Azure.Core;
    using Azure.Identity;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var endpoint = "https://endpt-wavnet-20241031213850.eastus.inference.ml.azure.com/score";
            var token = await GenerateToken();

            var jsonString = JsonSerializer.Serialize(GetParametersJson(30));
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            app.MapGet("/", () => $"Hello World! Here is the response: \n\n{response}");
            app.Run();
        }

        private static JsonElement GetParametersJson(int forecastDurationDays)
        {
            var forecastData = new
            {
                forecastDays = forecastDurationDays,
                historicalData = new List<double>
            {
                49.41, 49.11, 49.55, 49.5, 49.6, 49.53, 49.54, 49.7, 49.53, 49.53, 49.58, 57.29, 63.53, 63.52, 63.52, 74.48, 94.6, 105.51, 105.57, 105.67, 104.6, 105.89, 70.09, 77.01, 76.54, 76.34, 76.51, 76.86, 76.54, 77.52, 77.45, 73.42, 73.85, 73.62, 73.43, 73.65, 74.05, 74.3, 74.7, 74.93, 73.27, 73.69, 73.93, 74.2, 74.41, 74.69, 75.06, 73.82, 73.6, 73.8, 73.99, 76.75, 76.91, 75.87, 74.55, 74.77, 80.05, 76.13, 76.76, 77.14, 73.87, 74.09, 74.49, 75.54, 75.33, 76.29, 76.55, 75.41, 73.86, 74.25, 74.63, 75.01, 75.87, 80.71, 94.25, 94.98, 95.35, 95.63, 95.9, 96.15, 96.27, 94.5, 94.57, 94.75, 95.03, 94.37, 90.8, 90.82, 90.93, 90.74, 91.12, 90.43, 90.78, 90.59, 89.22, 89.42, 89.78, 90.15, 90.5, 90.82, 91.18, 89.32, 89.29, 89.92, 90.3, 89.78, 90.79, 89.76, 88.84, 89.94, 89.63, 89.99, 90.41, 90.12, 90.87, 89.55, 89.82, 90.04, 90.33, 90.81, 91.22, 86.97, 86.86, 87.32, 87.76, 88.06, 88.44, 88.88, 88.4, 87.24, 87.4, 87.54, 87.75, 88.11, 88.63, 86.11, 86.46, 86.78, 87.26, 87.38, 87.78, 88.19, 87.27, 86.99, 87.43, 87.87, 88.31, 88.8, 89.74, 89.46, 88.06, 88.53, 92.82, 93.45, 93.85, 94.37, 94.95, 92.78, 93.1, 93.54, 93.66, 94.07, 95.21, 93.27, 93.34, 92.58, 92.89, 93.48, 94.1, 94.82, 94.45, 93.12, 93.56, 94.06, 94.53, 95.02, 95.67, 96.33, 93.88, 93.84, 93.76, 95.47
            },
                historicalStartDate = "01012024",
                historicalEndDate = "06302024"
            };

            var json = JsonSerializer.SerializeToElement(forecastData);
            return json;
        }

        private static async Task<string> GenerateToken()
        {
            var credential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(["https://management.azure.com/.default"]);
            var accessToken = await credential.GetTokenAsync(tokenRequestContext);

            return accessToken.Token;
        }
    }
}
