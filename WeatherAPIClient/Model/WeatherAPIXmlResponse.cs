using System;
using System.Xml.Serialization;

namespace TakeHomeTasks.WeatherAPIClient.Model.Xml
{
    [XmlRoot("current")]
    public class WeatherAPIXmlResponse
    {
        [XmlElement("city")]
        public City City { get; set; }

        [XmlElement("temperature")]
        public Temperature Temperature { get; set; }

        [XmlElement("feels_like")]
        public FeelsLike FeelsLike { get; set; }
    }

    public class City
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("coord")]
        public Coord Coord { get; set; }// Coord();

        [XmlElement("country")]
        public string Country { get; set; }

    }

    public class Coord
    {
        [XmlAttribute("lon")]
        public double Lon { get; set; }

        [XmlAttribute("lat")]
        public double Lat { get; set; }
    }

    public class Temperature
    {
        [XmlAttribute("value")]
        public double Value { get; set; }

    }

    public class FeelsLike
    {
        [XmlAttribute("value")]
        public double Value { get; set; }
    }
}

  