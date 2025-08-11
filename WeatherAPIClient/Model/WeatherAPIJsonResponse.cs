using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TakeHomeTasks.WeatherAPIClient.Model.Json
{
    public class WeatherAPIJsonResponse
    {
        public string Name { get; set; }
        public MainData Main { get; set; }
        public Coord Coord { get; set; }
        public Sys Sys { get; set; }
    }

    public class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
    }
    public class Sys
    {
        public string Country { get; set; }
    }

}

