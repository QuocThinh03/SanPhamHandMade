using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLNguoiDung
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class IndexModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public IndexModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NguoiDung> NguoiDung { get;set; }

        public async Task OnGetAsync()
        {
            NguoiDung = await _context.NguoiDung.ToListAsync();
        }
    }
}
