using QuocThinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; 

public class AdminAuthorizationFilter : IAuthorizationFilter
{
    private readonly ApplicationDbContext _context;

    public AdminAuthorizationFilter(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetInt32("UserID");

        if (!user.HasValue)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }

        var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.ID == user);

        if (nguoiDung == null || nguoiDung.Quyen != 1)
        {
            context.Result = new RedirectToPageResult("/login");
        }
    }
}
