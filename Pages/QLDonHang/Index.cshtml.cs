using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLDonHang
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class IndexModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public IndexModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DonHang> DonHang { get;set; }

        public async Task OnGetAsync()
        {
            DonHang = await _context.DonHang.ToListAsync();
        }
    }
}
