using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;
using VideoGameManager.Services;    

namespace VideoGameManager.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly GameStoreContext _context;

        public DetailsModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.Include(g => g.Developer).FirstOrDefaultAsync(g => g.Id == id);
            
            if (game == null) return NotFound();

            Game = game;

            return Page();
        }
    }
}
