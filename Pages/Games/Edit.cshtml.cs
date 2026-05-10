using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly GameStoreContext _context;

        public EditModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        public SelectList DeveloperList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            Game = game;
            DeveloperList = new SelectList(_context.Developers, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                DeveloperList = new SelectList(_context.Developers, "Id", "Name");
                return Page();
            }

            _context.Attach(Game).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
