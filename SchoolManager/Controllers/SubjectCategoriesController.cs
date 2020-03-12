using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InstitutionsAPI.Core.Models;
using SchoolManager.Models;

namespace SchoolManager.Controllers
{
    public class SubjectCategoriesController : Controller
    {
        private readonly ApplicationContext _context;

        public SubjectCategoriesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: SubjectCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubjectCategories.ToListAsync());
        }

        // GET: SubjectCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectCategory = await _context.SubjectCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (subjectCategory == null)
            {
                return NotFound();
            }

            return View(subjectCategory);
        }

        // GET: SubjectCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubjectCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] SubjectCategory subjectCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subjectCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectCategory);
        }

        // GET: SubjectCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectCategory = await _context.SubjectCategories.FindAsync(id);
            if (subjectCategory == null)
            {
                return NotFound();
            }
            return View(subjectCategory);
        }

        // POST: SubjectCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] SubjectCategory subjectCategory)
        {
            if (id != subjectCategory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectCategoryExists(subjectCategory.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subjectCategory);
        }

        // GET: SubjectCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectCategory = await _context.SubjectCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (subjectCategory == null)
            {
                return NotFound();
            }

            return View(subjectCategory);
        }

        // POST: SubjectCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subjectCategory = await _context.SubjectCategories.FindAsync(id);
            _context.SubjectCategories.Remove(subjectCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectCategoryExists(int id)
        {
            return _context.SubjectCategories.Any(e => e.ID == id);
        }
    }
}
