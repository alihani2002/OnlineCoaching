using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.Domain.Entities;
using OnlineCoaching.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachingPackagesController : Controller
    {
        private readonly ICoachingPackageServices _packageService;

        public CoachingPackagesController(ICoachingPackageServices packageService)
        {
            _packageService = packageService;
        }

        // GET: CoachingPackage
        public IActionResult Index()
        {
            var packages = _packageService.GetCoachingPackages();
            return View(packages);
        }

        // GET: CoachingPackage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        // GET: CoachingPackage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoachingPackage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCoachingPackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                await _packageService.AddCoachingPackageAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // GET: CoachingPackage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        // POST: CoachingPackage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CoachingPackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _packageService.UpdateCoachingPackageAsync(dto);
            if (updated == null)
            {
                ModelState.AddModelError("", "Unable to update package.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CoachingPackage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        // POST: CoachingPackage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _packageService.DeleteCoachingPackageAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete package.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
