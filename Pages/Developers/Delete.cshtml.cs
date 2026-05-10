using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameManager.Data;
using VideoGameManager.Models;

namespace VideoGameManager.Pages.Developers
{
    public class DeleteModel : PageModel
    {
        private readonly GameStoreContext _context;

        public DeleteModel(GameStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Developer Developer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var developer = await _context.Developers.Include(d => d.Games).FirstOrDefaultAsync(m => m.Id == id);

            if (developer == null) return NotFound();

            Developer = developer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var developerToDelete = await _context.Developers.Include(d => d.Games).FirstOrDefaultAsync(m => m.Id == id);

            if (developerToDelete != null)
            {
                if (developerToDelete.Games.Any())
                {
                    ModelState.AddModelError("", $"Cannot delete {developerToDelete.Name} because they have {developerToDelete.Games.Count} associated games.");
                    Developer = developerToDelete; // Re-populate the Developer property to show details on the page.
                    return Page();
                }

                _context.Developers.Remove(developerToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");

        }
    }
}
