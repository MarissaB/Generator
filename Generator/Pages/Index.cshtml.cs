using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
// TODO: Make this more of a landing page so the Layout bar isn't so heavily used
// TODO: Move Fill pages under their own folder for better organization?
// TODO: Add user scaffolding? https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-7.0&tabs=visual-studio
// TODO: Add emailing system for validating accounts? https://code-maze.com/aspnetcore-send-email/ https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-7.0&tabs=visual-studio

namespace Generator.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}