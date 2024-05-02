﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Contexts;

namespace ProniaWebApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ProniaDbContext _context;

        public FooterViewComponent(ProniaDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _context.Settings.ToDictionaryAsync(r => r.Key, r => r.Value);
            return View(settings);
        }
    }
}
