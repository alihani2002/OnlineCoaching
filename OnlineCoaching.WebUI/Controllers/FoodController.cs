using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;


namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize]
    public class FoodController : Controller
    {
        private readonly IFoodServices _foodService;

        public FoodController(IFoodServices foodService)
        {
            _foodService = foodService;
        }

        public IActionResult Index()
        {
            var foods =  _foodService.GetFoodsAsync();
            return View(foods);
        }

        public async Task<IActionResult> Details(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return View(food);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food model)
        {
            if (ModelState.IsValid)
            {
                await _foodService.AddFoodAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FoodDto model)
        {
            if (ModelState.IsValid)
            {
                _foodService.UpdateFood(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _foodService.DeleteFood(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
