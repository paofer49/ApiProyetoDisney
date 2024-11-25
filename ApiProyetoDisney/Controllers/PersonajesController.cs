using Microsoft.AspNetCore.Mvc;
using ApiProyetoDisney.Models;
using System.Text.Json;


namespace ApiProyetoDisney.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PersonajesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<List<Personajes>> GetCharacters() 
        {
            var response = await _httpClient.GetAsync("https://api.disneyapi.dev/character");
            response.EnsureSuccessStatusCode();

            // Leer el JSON como string
            var jsonString = await response.Content.ReadAsStringAsync();

            // Deserializar como un objeto anónimo para acceder a "data"
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonString);

            // Acceder al array "data"
            var dataArray = jsonObject.GetProperty("data").GetRawText();

            // Deserializar el array "data" como lista de personajes
            var characters = JsonSerializer.Deserialize<List<Personajes>>(dataArray);

            return characters.OrderBy(personaje => personaje._id).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personajes>> GetCharactersId(int id)
        {
            var response = await _httpClient.GetAsync($"https://api.disneyapi.dev/character/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonString);
            // Verificar si la propiedad "data" existe y contiene elementos
            if (jsonObject.TryGetProperty("data", out var dataElement))
            {
                // Si la propiedad "data" está vacía, indicar que no se encontró el personaje
                if (dataElement.ValueKind == JsonValueKind.Array && dataElement.GetArrayLength() == 0)
                {
                    return NotFound(new { message = "Personaje no encontrado" });
                }
            }
            // Deserializar la propiedad "data" como un objeto del tipo Personajes
            var character = JsonSerializer.Deserialize<Personajes>(dataElement.GetRawText());

            return Ok(character);
        }
    }
}
