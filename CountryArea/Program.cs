using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CountryArea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JObject countriesWithArea = new JObject();
            List<OldCountry> oldCountries = new List<OldCountry>();
            List<Area> oldAreas = new List<Area>();

            //Load countries to Memory from JSON
            using (StreamReader file = File.OpenText(@"C:\Users\debojyoti\Desktop\CountryExtraction\countries.json"))
            {
                var countries = file.ReadToEnd();
                oldCountries = JsonConvert.DeserializeObject<List<OldCountry>>(countries);
            }

            //Load areas to Memory rfom JSON
            using (StreamReader file = File.OpenText(@"C:\Users\debojyoti\Desktop\CountryExtraction\areas.json"))
            {
                var areas = file.ReadToEnd();
                oldAreas = JsonConvert.DeserializeObject<List<Area>>(areas);
            }

            //Load country-city file to Memory from JSON
            using (StreamReader file = File.OpenText(@"C:\Users\debojyoti\Desktop\CountryExtraction\countries-with-cities.json"))
            {
                var countriesWithAreaJSON = file.ReadToEnd();
                countriesWithArea = JObject.Parse(countriesWithAreaJSON);
            }

            //Add areas that are not present
            foreach (var country in oldCountries)
            {
                if (countriesWithArea[country.Name] != null)
                {
                    foreach (var city in countriesWithArea[country.Name])
                    {
                        if (oldAreas.Find(x => x.StateCity == city.ToString()) == null)
                        {
                            Area area = new Area
                            {
                                StateCity = city.ToString(),
                                CountryId = country.Id,
                                Code = null
                            };
                            oldAreas.Add(area);
                        }
                    }
                }
            }

            //Open file stream
            using (StreamWriter file = File.CreateText(@"C:\Users\debojyoti\Desktop\CountryExtraction\all-areas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, oldAreas);
            }
        }
    }
    public class OldCountry
    {
        public  Int64 Id;
        public  string Name;
        public  Int64 CountryCode;
    }
    public class Area
    {
        public string StateCity;
        public Int64? Code;
        public Int64 CountryId;
    }
}
