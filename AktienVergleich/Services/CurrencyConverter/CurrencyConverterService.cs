using AktienVergleich.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AktienVergleich.Services.CurrencyConverter
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        #region Properties

        private const string HOST = "https://v6.exchangerate-api.com";
        private const string API_KEY = "657d2ec867e5775d5ad33de5";

        #endregion

        #region Constructors

        public CurrencyConverterService()
        {

        }

        #endregion

        #region Logic

        public async Task<double> ConvertAsync(double value, string from, string to)
        {
            string fromCurrency = from.ToUpperInvariant();
            string toCurrency = to.ToUpperInvariant();

            HttpClient client = this.GetHttpClient();
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"/v6/{API_KEY}/latest/{fromCurrency}");
            string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            //JObject json = JObject.Parse(jsonContent);
            //JArray results = (json.GetValue("results") as JArray)!;

            //List<Company> companies = new List<Company>();
            //if (results == null)
            //{
            //    return companies;
            //}

            //foreach (JToken result in results)
            //{
            //    JObject entry = (result as JObject)!;
            //    companies.Add(new Company
            //    {
            //        Name = (result as JObject)!.GetValue("name")!.ToString(),
            //        Symbol = (result as JObject)!.GetValue("ticker")!.ToString()
            //    });
            //}

            return 0;
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
