using System.Text.Json;
using VideoGameManager.Models;

namespace VideoGameManager.Services
{
    public class GamesRepository
    {
        private readonly string path = Path.Combine("wwwroot", "data", "games.json");
        public List<Game> LoadAll()
        {
            if (!File.Exists(path)) return new List<Game>();

            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new List<Game>();

            return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
        }

        public void SaveAll(IEnumerable<Game> games)
        {
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.Combine("wwwroot", "data"));
            File.WriteAllText(path, json);
        }
    }
}
