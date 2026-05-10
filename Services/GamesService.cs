using VideoGameManager.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace VideoGameManager.Services
{
    public class GamesService
    {
        private readonly List<Game> _games = new()
    {
        new() { Id=1, Title="The Legend of Zelda: TotK", Genre="Adventure", Year=2023, Score=9.8, Description="Open-world action RPG" },
        new() { Id=2, Title="Elden Ring", Genre="RPG", Year=2022, Score=9.5, Description="Open-world soulslike" },
        new() { Id=3, Title="Celeste", Genre="Platformer", Year=2018, Score=9.0, Description="Precision platformer" },
    };
        private int _nextId = 4;

        private readonly string _logPath = Path.Combine("wwwroot", "data", "activity_log.txt");
        public List<Game> GetAll() => _games;
        public Game? GetById(int id) => _games.FirstOrDefault(g => g.Id == id);
        public void Add(Game game)
        {
            game.Id = _nextId++; 
            _games.Add(game);
            LogAction("Create", game.Title);
        }
        public void Update(Game game)
        {
            var index = _games.FindIndex(g => g.Id == game.Id);
            if (index >= 0) 
            { 
                _games[index] = game;
                LogAction("Edit", game.Title);
            }
        }
        public void Delete(int id)
        {
            var game = GetById(id);
            if (game != null)
            {
                _games.Remove(game);
                LogAction("Delete", game.Title);
            }
        }

        private void LogAction(string action, string title)
        {
            Directory.CreateDirectory(Path.Combine("wwwroot", "data"));

            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string line = $"[{date}] - [{action}] [{title}]{Environment.NewLine}";

            File.AppendAllText(_logPath, line);
        }

        public string[] ReadLogs()
        {
            if (File.Exists(_logPath))
            {
                return File.ReadAllLines(_logPath);
            }
            return Array.Empty<string>();
        }

        public void ReplaceAllGames(List<Game> newGames)
        {
            _games.Clear();
            _games.AddRange(newGames);

            if (_games.Any())
            {
                _nextId = _games.Max(g => g.Id) + 1;
            }
        }
    }

}
