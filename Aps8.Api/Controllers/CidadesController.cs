using Aps8.Api.Models.Contexts;
using Aps8.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Newtonsoft.Json;

namespace Aps8.Api.Controllers
{
    [Route("api/cidades")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "cidades")]
    public class CidadesController : Controller
    {
        private readonly CidadesContext _context;
        public CidadesController(CidadesContext context)
        {
            _context = context;
        }


            /// <summary>
            /// Obter Todos
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpGet]
            public async Task<List<Cidade>> GetTodasCidades()
            {
                return await _context.Cidades.Select(o => new Cidade
                {
                    Id = o.Id,
                    Nome = o.Nome
                }).ToListAsync();
            }

            /// <summary>
            /// Obter Cidade
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpGet("{id}")]
            public async Task<ActionResult<Cidade>> GetCliente(Guid id)
            {
                var cidade = await _context.Cidades.FindAsync(id);

                if (cidade == null)
                {
                    return NotFound();
                }

                return cidade;
            }

            /// <summary>
            /// Adicionar Cidade
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpPost]
            public async Task<Cidade> PostCidade(string nome)
            {
                var entidade = new Cidade
                {
                    Id = Guid.NewGuid(),
                    Nome = nome
                };

                await _context.AddAsync(entidade);
                await _context.SaveChangesAsync();

                return entidade;
            }

            /// <summary>
            /// Atualizar Cidade
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpPut("{id}")]
            public async Task<IActionResult> PutCidade(Guid id, Cidade cidadeDTO)
            {
                if (id != cidadeDTO.Id)
                {
                    return BadRequest();
                }

                var cidade = await _context.Cidades.FindAsync(id);
                if (cidade == null)
                {
                    return NotFound();
                }

                cidade.Nome = cidadeDTO.Nome;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!CidadeExists(id))
                {
                    return NotFound();
                }

                return NoContent();
            }

            /// <summary>
            /// Excluir Cidade
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCidade(Guid id)
            {
                var cidade = await _context.Cidades.FindAsync(id);
                if (cidade == null)
                {
                    return NotFound();
                }

                _context.Cidades.Remove(cidade);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            #region Métodos Privados/Auxiliares
            private bool CidadeExists(Guid id)
            {
                return (_context.Cidades?.Any(e => e.Id == id)).GetValueOrDefault();
            }
            #endregion

            /// <summary>
            /// Obter Todos
            /// </summary>
            /// <returns>Objeto correspondente</returns>
            [HttpGet("qualidades")]
            public async Task<List<QualidadeAr>> GetTodasQualidades()
            {
                var cidades = await GetTodasCidades();

            return await GetQualidadePorCidades(cidades);
            }

        #region Métodos Privados/Auxiliares
        private async Task<List<QualidadeAr>> GetQualidadePorCidades(List<Cidade> cidades)
        {
            var resposta = new List<QualidadeAr>();
            var url = "https://api.openaq.org/v2/cities?limit=100&page=1&offset=0&sort=asc&country=BR&order_by=city";

            foreach (var cidade in cidades)
            {
                url += "&city=" + cidade.Nome;
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
                QualidadeAr entidade = new()
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
        #endregion
    }
    }