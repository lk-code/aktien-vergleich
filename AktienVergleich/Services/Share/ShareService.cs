using AktienVergleich.Interfaces;
using AktienVergleich.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AktienVergleich.Services.Share
{
    public class ShareService : IShareService
    {
        #region Properties

        private const string HOST = "https://api.polygon.io";
        private const string API_KEY = "RypC0fMEDuQQXVfp9xZ_M5devSfbzmOt";

        #endregion

        #region Constructors

        public ShareService()
        {

        }

        #endregion

        #region Logic

        public async Task<List<Company>> GetCompaniesAsync(string name = null)
        {
            HttpClient client = this.GetHttpClient();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "apiKey", API_KEY },
                { "active", "true" },
                { "sort", "ticker" },
                { "order", "asc" },
                { "limit", "10" }
            };
            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add("search", name);
            }
            string urlParameters = this.RenderUrlParameters(parameters);

            HttpResponseMessage httpResponseMessage = await client.GetAsync($"/v3/reference/tickers{urlParameters}");
            string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonContent);
            JArray results = (json.GetValue("results") as JArray)!;

            List<Company> companies = new List<Company>();
            if (results == null)
            {
                return companies;
            }

            foreach (JToken result in results)
            {
                JObject entry = (result as JObject)!;
                companies.Add(new Company
                {
                    Name = (result as JObject)!.GetValue("name")!.ToString(),
                    Symbol = (result as JObject)!.GetValue("ticker")!.ToString()
                });
            }

            return companies;
        }

        public async Task<Company> GetCompanyDetailsAsync(Company company)
        {
            HttpClient client = this.GetHttpClient();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "apiKey", API_KEY }
            };
            string urlParameters = this.RenderUrlParameters(parameters);

            HttpResponseMessage httpResponseMessage = await client.GetAsync($"/v3/reference/tickers/{company.Symbol}{urlParameters}");
            string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonContent);
            JObject results = (json.GetValue("results") as JObject)!;

            JObject branding = (results.GetValue("branding") as JObject)!;

            company.Logo = branding.GetValue("icon_url")!.ToString() + "?apiKey=" + API_KEY;

            return company;
        }

        public async Task<List<Dividend>> GetDividendsAsync(string symbol)
        {
            HttpClient client = this.GetHttpClient();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "apiKey", API_KEY },
                { "ticker", symbol }
            };
            string urlParameters = this.RenderUrlParameters(parameters);

            HttpResponseMessage httpResponseMessage = await client.GetAsync($"/v3/reference/dividends{urlParameters}");
            string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonContent);
            JArray results = (json.GetValue("results") as JArray)!;

            List<Dividend> dividends = new List<Dividend>();
            foreach (JToken result in results)
            {
                JObject entry = (result as JObject)!;
                dividends.Add(new Dividend
                {
                    //Name = (result as JObject)!.GetValue("name")!.ToString(),
                    //Symbol = (result as JObject)!.GetValue("ticker")!.ToString()
                });
            }

            return dividends;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(HOST);

            return client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string RenderUrlParameters(Dictionary<string, string> parameters)
        {
            StringBuilder urlParametersStringBuilder = new StringBuilder();
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i == 0)
                {
                    urlParametersStringBuilder.Append("?");
                }
                else
                {
                    urlParametersStringBuilder.Append("&");
                }

                KeyValuePair<string, string> parameter = parameters.ElementAt(i);
                urlParametersStringBuilder.AppendFormat("{0}={1}", parameter.Key, parameter.Value);
            }

            string urlParameters = urlParametersStringBuilder.ToString();

            return urlParameters;
        }

        #endregion
    }
}
