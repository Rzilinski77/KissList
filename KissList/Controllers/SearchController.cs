using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KissList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KissList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private JsonDocument jDoc;
        private IConfiguration _config;
        Random random = new Random();
        Listing luckyListing = new Listing();
        List<Listing> giftList = new List<Listing>();
        List<int> pictureIdList = new List<int>();
        int listingId;
        string parameters;

        public SearchController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Listing> GetLucky()
        {
            string apiKey = _config["ApiKey"];
            int num = random.Next(12000);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://openapi.etsy.com/v2/listings/active?api_key={apiKey}&limit=100&offset={num}"))
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    jDoc = JsonDocument.Parse(stringResponse);

                    luckyListing.title = jDoc.RootElement.GetProperty("results")[0].GetProperty("title").ToString();
                    luckyListing.url = jDoc.RootElement.GetProperty("results")[0].GetProperty("url").ToString();
                    luckyListing.description = jDoc.RootElement.GetProperty("results")[0].GetProperty("description").ToString();
                    luckyListing.price = jDoc.RootElement.GetProperty("results")[0].GetProperty("price").ToString();
                    listingId = jDoc.RootElement.GetProperty("results")[0].GetProperty("listing_id").GetInt32();
                }
                using (var response = await httpClient.GetAsync($"https://openapi.etsy.com/v2/listings/{listingId}/images?api_key={apiKey}"))
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    jDoc = JsonDocument.Parse(stringResponse);

                    luckyListing.imageUrl = jDoc.RootElement.GetProperty("results")[0].GetProperty("url_fullxfull").ToString();
                }
            }
            return luckyListing;
        }


        //Calls the API 
        public async Task<List<Listing>> Search(string occasion, string recipient, string minPrice, string maxPrice, string keyword)
        {
            parameters = Parameter(occasion);
            parameters = Parameter(recipient);
            if (minPrice != null)
            {
                parameters += "&min_price=" + minPrice;
            }
            if (maxPrice != null)
            {
                parameters += "&max_price=" + maxPrice;
            }
            if (keyword != null)
            {
                parameters += "&keywords=" + keyword;
            }

            string apiKey = _config["ApiKey"];
            int num = random.Next(10);
            int arrayLength;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://openapi.etsy.com/v2/listings/active?api_key={apiKey}&limit=100&offset={num}{parameters}"))
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    jDoc = JsonDocument.Parse(stringResponse);

                    arrayLength = jDoc.RootElement.GetProperty("results").GetArrayLength();
                   
                    for (int i = 0; i < arrayLength && i < 5 ; i++)
                    {
                        Listing searchListing = new Listing();
                        searchListing.title = jDoc.RootElement.GetProperty("results")[i].GetProperty("title").ToString();
                        searchListing.url = jDoc.RootElement.GetProperty("results")[i].GetProperty("url").ToString();
                        searchListing.description = jDoc.RootElement.GetProperty("results")[i].GetProperty("description").ToString();
                        searchListing.price = jDoc.RootElement.GetProperty("results")[i].GetProperty("price").ToString();
                        listingId = jDoc.RootElement.GetProperty("results")[i].GetProperty("listing_id").GetInt32();

                        pictureIdList.Add(listingId);
                        giftList.Add(searchListing);
                    }
                }

                for (int i = 0; i < arrayLength && i < 5; i++)
                {
                    using (var response = await httpClient.GetAsync($"https://openapi.etsy.com/v2/listings/{pictureIdList[i]}/images?api_key={apiKey}"))
                    {
                        var stringResponse = await response.Content.ReadAsStringAsync();

                        jDoc = JsonDocument.Parse(stringResponse);

                        giftList[i].imageUrl = jDoc.RootElement.GetProperty("results")[0]
                            .GetProperty("url_fullxfull").ToString();
                    }
                }
                return giftList;
            }
        }
        public string Parameter(string input)
        {
            if (input != null)
            {
                parameters += "&tags=" + input;
            }
            return parameters;
        }
    }
}

