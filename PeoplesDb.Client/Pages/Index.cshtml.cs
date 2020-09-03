using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PeoplesDb.Client.Services;
using PeoplesDb.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeoplesDb.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPeopleClient peopleClient;

        public IEnumerable<Person> People { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IPeopleClient peopleClient)
        {
            _logger = logger;
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
