using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeoplesDb.Client.Services;
using PeoplesDb.Models;

namespace PeoplesDb.Client.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IPeopleClient peopleClient;

        [BindProperty]
        public Person Person { get; set; }

        public CreateModel(IPeopleClient peopleClient)
        {
            this.peopleClient = peopleClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            bool success = await peopleClient.AddPerson(Person);

            if (success)
                return RedirectToPage("./Index");
            return Page();
        }
    }
}
