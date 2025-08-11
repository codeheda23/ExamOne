using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TakeHomeTasks.WeatherAPIClient.Model.Json;
using TakeHomeTasks.WeatherAPIClient.Model.Xml;

namespace TaskHomeTasks.WeatherAPIClient
{
    class Program
    {
        private const string APIKey = "4aa639d0111c32fbd369dc631b0cb849";
        private const string APIUrl = "https://api.openweathermap.org/data/2.5/weather";
        private static readonly HttpClient httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            while (true)
            {
                DisplaySampleCities();

                Console.Write("\nEnter a city name: ");
                string city = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(city))
                {
                    Console.WriteLine("City name cannot be empty.");
                    return;
                }
                try
                {
                    //JSON
                    var jsonRaw = await FetchWeather(city, "json");
                    Console.WriteLine($"\n Raw JSON: \n{jsonRaw}");
                    var jsonData = DeserializeJson(jsonRaw);
                    DisplayDeserializedJson(jsonData);

                    //XML
                    var xmlRaw = await FetchWeather(city, "xml");
                    Console.WriteLine($"\nRaw XML: \n{xmlRaw}");
                    var xmlData = DeserializeXml(xmlRaw);
                    DisplayDeserializedXml(xmlData);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Try again... \n");
                    continue; //
                }
                Console.WriteLine("\nPress any key to continue... ");
                Console.ReadKey();
                Console.Clear();
            }
        }


        private static void DisplayDeserializedJson(WeatherAPIJsonResponse weatherData)
        {
            Console.WriteLine($"\nDeserialized JSON:\n");
            Console.WriteLine($"City: {weatherData?.Name ?? "N/A"}");
            Console.WriteLine($"Country: {weatherData?.Sys?.Country ?? "N/A"}");
            Console.WriteLine($"Coordinates: Lat {weatherData?.Coord?.Lat ?? 0f}, Lon {weatherData?.Coord?.Lon ?? 0f}");
            Console.WriteLine($"Temperature: {ConvertKelvinToCelsius(weatherData?.Main?.Temp ?? 0f)}°C (Feels like: {ConvertKelvinToCelsius(weatherData?.Main?.Feels_Like ?? 0f)}°C)");
            // display more data if needed
        }

        private static void DisplayDeserializedXml(WeatherAPIXmlResponse weatherData)
        {

            Console.WriteLine($"\nDeserialized XML:\n");
            Console.WriteLine($"City: {weatherData?.City?.Name ?? "N/A"}");
            Console.WriteLine($"Country: {weatherData?.City?.Country ?? "N/A"}");
            Console.WriteLine($"Coordinates: Lat {weatherData?.City?.Coord?.Lat ?? 0f}, Lon {weatherData?.City?.Coord?.Lon ?? 0f}");
            Console.WriteLine($"Temperature: {ConvertKelvinToCelsius(weatherData?.Temperature?.Value ?? 0f)}°C (Feels like: {ConvertKelvinToCelsius(weatherData?.FeelsLike?.Value ?? 0f)}°C)");
            // display more data if needed

        }

        private static async Task<string> FetchWeather(string cityName, string mode = "json")
        {
            string apiUrl = $"{APIUrl}?q={cityName}&appid={APIKey}" + (mode == "xml" ? "&mode=xml" : "");
            var response = await httpClient.GetAsync(apiUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new HttpRequestException($"City '{cityName}' not found.");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"API request failed with status {(int)response.StatusCode}.");


            return await response.Content.ReadAsStringAsync();
        }

        private static WeatherAPIJsonResponse DeserializeJson(string json)
        {
            try
            {
                var weatherData = JsonConvert.DeserializeObject<WeatherAPIJsonResponse>(json);
                return weatherData;
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to deserialize json: {ex.Message}");
            }

        }
        private static WeatherAPIXmlResponse DeserializeXml(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(WeatherAPIXmlResponse));
                using (var reader = new StringReader(xml))
                {
                    var weatherData = (WeatherAPIXmlResponse)serializer.Deserialize(reader);
                    return weatherData;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Failed to deserialize xml: {ex.Message}");
            }
        }
        private static double ConvertKelvinToCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }
        private static void DisplaySampleCities()
        {
            Console.WriteLine("Sample cities:");

            var sampleCities = new List<string> { "Rome", "Moscow", "London", "New York", "Tokyo", "Paris", "Sydney", "Beijing" };

            foreach (var city in sampleCities)
                Console.WriteLine($"- {city}");
        }
    }
}

#region old code
//private static async Task GetWeather(string cityName)
//{
//    string apiUrl = $"{APIUrl}?q={cityName}&appid={APIKey}";

//    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
//    if (response.IsSuccessStatusCode)
//    {
//        var strResponse = await response.Content.ReadAsStringAsync();

//        try
//        {
//            var weatherData = JsonConvert.DeserializeObject<OpenWeatherMapAPIResponse>(strResponse);
//            if (weatherData == null)
//            {
//                Console.WriteLine("API returned an empty response.");
//                return;
//            }
//            Console.WriteLine($"City: {weatherData.Name}, {weatherData.Sys.Country}");
//            Console.WriteLine($"Coordinates: Lat {weatherData.Coor.Lat}, Lon {weatherData.Coor.Lon}");
//            Console.WriteLine($"Weather: {weatherData.Weather[0].Main} - {weatherData.Weather[0].Description}");
//            Console.WriteLine($"Temperature: {ConvertKelvinToCelsius(weatherData.Main.Temp):F1}°C (Feels like: {ConvertKelvinToCelsius(weatherData.Main.Feels_Like):F1}°C)");
//            Console.WriteLine($"Pressure: {weatherData.Main.Pressure} hPa, Humidity: {weatherData.Main.Humidity}%");
//        }
//        catch (JsonException ex)
//        {
//            Console.WriteLine($"Failed to parse API JSON: {ex.Message}");
//            return;
//        }
//    }
//    else
//    {
//        Console.WriteLine("Error retrieving weather data.");
//    }
//}
#endregion
