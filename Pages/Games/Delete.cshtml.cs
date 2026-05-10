using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class DeleteModel : PageModel
    {
        private readonly GameStoreContext _context;

        public DeleteModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.Include(g => g.Developer).FirstOrDefaultAsync(g => g.Id == id);
            
            if (game == null) return NotFound();
            
            Game = game;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);

            if (game != null)
            {
                Game = game;
                _context.Games.Remove(Game);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
