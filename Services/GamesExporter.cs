using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VideoGameManager.Models;
using Microsoft.AspNetCore.Hosting;
namespace VideoGameManager.Services
{
    public class GamesExporter
    {
        private readonly string _path;

        public GamesExporter(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.WebRootPath, "data", "games.csv");
        }

        public void ExportToCsv(IEnumerable<Game> games)
        {
            var lines = new List<string>();

            lines.Add("Id,Title,Genre,Year,Score");

            foreach (var game in games)
            {
                string line = string.Join(",", new[]
                {
                    game.Id.ToString(),
                    game.Title,
                    game.Genre,
                    game.Year.ToString(),
                    game.Score.ToString()
                });
                lines.Add(line);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllLines(_path, lines, Encoding.UTF8);
        }

        public List<Game> ImportFromCsv()
        {
            if (!File.Exists(_path)) return new List<Game>();

            var games = new List<Game>();
            var lines = File.ReadAllLines(_path);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    games.Add(new Game
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Genre = parts[2],
                        Year = int.Parse(parts[3]),
                        Score = double.Parse(parts[4])
                    });
                }
            }
            return games;
        }
    }
}
