using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace builderminecraftIA
{
    public class OllamaClient
    {
        private readonly HttpClient _http = new();

        public async Task<string> AskAsync(string prompt, string model)
        {
            var payload = new
            {
                model = model,
                stream = false,
                format = "json",
                messages = new[]
                {
                    new { role = "system", content = "Tu es un générateur de schémas Minecraft. Réponds uniquement en JSON strict valide, sans aucun texte libre." },
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("http://localhost:11434/api/chat", content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur Ollama: {(int)response.StatusCode} - {body}");

            using var doc = JsonDocument.Parse(body);
            var text = doc.RootElement.GetProperty("message").GetProperty("content").GetString();

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Réponse Ollama vide.");

            return text;
        }
    }
}
