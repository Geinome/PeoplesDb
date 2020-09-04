using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeoplesDb.Client.Services;
using PeoplesDb.Models;

namespace PeoplesDb.Client.Pages
{
    public class EditModel : PageModel
    {
        private readonly IPeopleClient peopleClient;

        [BindProperty(SupportsGet = true)]
        public Person Person { get; set; }

        public EditModel(IPeopleClient peopleClient)
        {
            this.peopleClient = peopleClient;
        }

        public async Task<IActionResult> OnGet()
        {
            Person = await peopleClient.GetPerson(Person.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool success = await peopleClient.UpdatePerson(Person);

            if(success)
                return RedirectToPage("./Index");
            return Page();
        }
    }
}
