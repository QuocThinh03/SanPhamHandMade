using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuocThinh.Pages
{
    public class CallBackModel : PageModel
    {
        public string NavCssClass { get; set; }
        public void OnGet()
        {
            NavCssClass = "category-style-2";
        }
    }
}
