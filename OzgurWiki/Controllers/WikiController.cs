using Microsoft.AspNetCore.Mvc;
using OzgurWiki.Context;
using OzgurWiki.Helpers;
using OzgurWiki.Models;

namespace OzgurWiki.Controllers
{
    public class WikiController : Controller
    {
        private MyContext _context;

        public WikiController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var query = _context.WikiPages.OrderByDescending(p => p.LastUpdated);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pages = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(pages);
        }




        public IActionResult Detail(int id)
        {
            var page = _context.WikiPages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                return NotFound();

            var allPages = _context.WikiPages.ToList();

            // büyük/küçük harf duyarsız eşleşme için helper'ı güncelle (yukarıda gösterildi)
            page.Content = ContentHelper.LinkifyWikiContent(page.Content, allPages);

            Console.WriteLine("İşlenmiş içerik: " + page.Content);

            return View(page);
        }

        [HttpGet]
        public IActionResult Create(string title = "")
        {
            var page = new WikiPage
            {
                Title = title
            };
            return View(page);
        }

        [HttpPost]
        public IActionResult Create(WikiPage page)
        {
            if (ModelState.IsValid)
            {
                _context.WikiPages.Add(page);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WikiPage wiki)
        {
            if (ModelState.IsValid)
            {
                var page = _context.WikiPages.FirstOrDefault(p => p.Id == wiki.Id);
                if (page == null)
                    return NotFound();

                page.Title = wiki.Title;
                page.Content = wiki.Content;
                page.LastUpdated = DateTime.Now;

                _context.SaveChanges();
                return RedirectToAction("Detail", new { id = page.Id });
            }

            var originalPage = _context.WikiPages.FirstOrDefault(p => p.Id == wiki.Id);
            return View(originalPage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var page = _context.WikiPages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                return NotFound();

            _context.WikiPages.Remove(page);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
