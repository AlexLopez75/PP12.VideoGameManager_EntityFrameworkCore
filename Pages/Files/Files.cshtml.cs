using VideoGameManager.Models;
using VideoGameManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;


namespace VideoGameManager.Pages.Files
{
    public class FilesModel : PageModel
    {
        private readonly GamesService _gameService;
        private readonly GamesRepository _gameRepository;
        private readonly GamesExporter _gameExporter;
        private readonly GamesRanking _gamesRanking;

        public string[] LogEntries { get; set; } = Array.Empty<string>();

        public string Message { get; set; } = string.Empty;

        public FilesModel(GamesService gameService, GamesRepository gameRepository, GamesExporter gameExporter, GamesRanking gamesRanking) 
        {
            _gameService = gameService; 
            _gameRepository = gameRepository;
            _gameExporter = gameExporter;
            _gamesRanking = gamesRanking;
        }

        public void OnGet()
        {
            LogEntries = _gameService.ReadLogs();
        }

        public IActionResult OnPostExport()
        {
            var currentGames = _gameService.GetAll();
            _gameRepository.SaveAll(currentGames);

            Message = $"Exported {currentGames.Count} games to JSON.";
            LogEntries = _gameService.ReadLogs();
            return Page();
        }

        public IActionResult OnPostImport()
        {
            var importedGames = _gameRepository.LoadAll();
            _gameService.ReplaceAllGames(importedGames);

            Message = $"Imported {importedGames.Count} games from JSON.";
            LogEntries = _gameService.ReadLogs();
            return Page();
        }

        public IActionResult OnPostExportCsv()
        {
            var games = _gameService.GetAll();
            _gameExporter.ExportToCsv(games);

            string filePath = Path.Combine("wwwroot", "data", "games.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "text/csv", "games.csv");
        }

        public IActionResult OnPostImportCsv()
        {
            var imported = _gameExporter.ImportFromCsv();
            _gameService.ReplaceAllGames(imported);
            Message = $"Imported {imported.Count} games from CSV.";
            OnGet();
            return Page();
        }

        public IActionResult OnPostGenerateXml()
        {
            var games = _gameService.GetAll();
            _gamesRanking.GenerateRankingXml(games);
            
            string filePath = Path.Combine("wwwroot", "data", "games_ranking.xml");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            
            return File(fileBytes, "application/xml", "games_ranking.xml");
        }
    }
}
