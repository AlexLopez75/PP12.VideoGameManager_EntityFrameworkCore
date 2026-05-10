using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly GameStoreContext _context;
        public CreateModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public SelectList DeveloperList { get; set; } = default!;

        public IActionResult OnGet()
        {
            DeveloperList = new SelectList(_context.Developers, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                DeveloperList = new SelectList(_context.Developers, "Id", "Name");
                return Page();
            }

            _context.Games.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}