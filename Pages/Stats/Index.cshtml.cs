using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Stats
{
    public class IndexModel : PageModel
    {
        private readonly GameStoreContext _context;

        public IndexModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? selectedGenre { get; set; }
        public SelectList Genres { get; set; } = default!;
        public IList<Game> FilteredGames { get; set; } = new List<Game>();
        public IList<Game> Top5Games { get; set; } = new List<Game>();
        public Dictionary<string, int> GamesPerDecade { get; set; } = new Dictionary<string, int>();


        public class DevStat
        {
            public string Name { get; set; } = default!;
            public int GameCount { get; set; }
            public double AvgScore { get; set; }
        }
        public IList<DevStat> AvgByDev { get; set; } = new List<DevStat>();

        [BindProperty(SupportsGet = true)]
        public string? titleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? genreFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? minYear { get; set; }
        public IList<Game> Results { get; set; } = new List<Game>();


        [BindProperty(SupportsGet = true)]
        public int threshold { get; set; } = 1;
        public IList<Developer> ProductiveDevs { get; set; } = new List<Developer>();

        public async Task OnGetAsync()
        {
            var genreQuery = from g in _context.Games
                             orderby g.Genre
                             select g.Genre;

            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            if (!string.IsNullOrEmpty(selectedGenre))
            {
                var rpgGames = await _context.Games
                    .Where(g => g.Genre == selectedGenre)
                    .OrderByDescending(g => g.Score)
                    .ToListAsync();

                FilteredGames = rpgGames;
            }
            else
            {
                FilteredGames = new List<Game>();
            }

            var top5 = await _context.Games
                .Include(g => g.Developer)
                .OrderByDescending(g => g.Score)
                .Take(5)
                .ToListAsync();

            Top5Games = top5;
            
            var byDecade = await _context.Games
                .GroupBy(g => (g.Year / 10) * 10)
                .Select(grp => new { Decade = grp.Key, Count = grp.Count() })
                .OrderBy(x => x.Decade)
                .ToListAsync();

            GamesPerDecade = byDecade.ToDictionary(x => $"{x.Decade}s", x => x.Count);



            var avgByDev = await _context.Developers
                .Include(d => d.Games)
                .Where(d => d.Games.Any())
                .Select(d => new {
                    d.Name,
                    GameCount = d.Games.Count,
                    AvgScore = d.Games.Average(g => g.Score)
                })
                .OrderByDescending(x => x.AvgScore)
                .ToListAsync();

            AvgByDev = avgByDev.Select(x => new DevStat
            {
                Name = x.Name,
                GameCount = x.GameCount,
                AvgScore = x.AvgScore
            }).ToList();


            var query = _context.Games.Include(g => g.Developer).AsQueryable();

            if (!string.IsNullOrEmpty(titleFilter))
                query = query.Where(g => g.Title.Contains(titleFilter));

            if (!string.IsNullOrEmpty(genreFilter))
                query = query.Where(g => g.Genre == genreFilter);

            if (minYear.HasValue)
                query = query.Where(g => g.Year >= minYear.Value);

            Results = await query.OrderBy(g => g.Title).ToListAsync();


            var productiveDev = await _context.Developers
                .Include(d => d.Games)
                .Where(d => d.Games.Count > threshold)
                .OrderByDescending(d => d.Games.Count)
                .ToListAsync();

            ProductiveDevs = productiveDev;
        }
    }
}
