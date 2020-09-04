using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeoplesDb.Client.Services;
using PeoplesDb.Models;

namespace PeoplesDb.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPeopleClient peopleClient;

        public IEnumerable<Person> People { get; private set; }

        public IndexModel(IPeopleClient peopleClient)
        {
            this.peopleClient = peopleClient;
        }

        public async Task OnGetAsync()
        {
            People = await peopleClient.GetPeople();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await peopleClient.DeletePerson(id);
            return RedirectToPage();
        }
    }
}
