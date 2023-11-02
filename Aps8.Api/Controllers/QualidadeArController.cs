using RestSharp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Aps8.Api.Models;

namespace Aps8.Api.Controllers
{
    [Route("api/qualidade-ar")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "qualidade-ar")]
    public class QualidadeArController : ControllerBase
    {
        /// <summary>
        /// Obter Todos
        /// </summary>
        /// <returns>Objeto correspondente</returns>
        [HttpGet]
        public async Task<List<QualidadeAr>> GetTodos()
        {
            var resposta = new List<QualidadeAr>();
            var url = "https://api.openaq.org/v2/cities?limit=100&page=1&offset=0&sort=asc&country=BR&order_by=city";
            var cidades = new string[]
            {
               "Sorocaba",
               "Campinas",
               "São Paulo"
            };

            foreach (var cidade in cidades)
            {
                url += "&city=" + cidade;
            };

            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            if (!response.IsSuccessStatusCode) return new List<QualidadeAr>();

            var result = JsonConvert.DeserializeObject<RetornoApi>(response.Content);

            foreach (var cidade in result.Results)
            {
                QualidadeAr entidade = new QualidadeAr()
                {
                    Country = cidade.Country,
                    City = cidade.City,
                    Count = cidade.Count,
                    Locations = cidade.Locations,
                    FirstUpdated = cidade.FirstUpdated,
                    LastUpdated = cidade.LastUpdated,
                    Parameters = cidade.Parameters
                };

                resposta.Add(entidade);
            };

            return resposta;
        }
    }
}
