using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.WebUI.Core.ViewModels.Food;


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
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Index()
        {
            var foods = _foodService.GetFoodsAsync();
            return View(foods);
        }

        [HttpGet]
        public IActionResult Compare()
        {
            var foods = _foodService.GetFoodsAsync();

            var model = new FoodCompareViewModel
            {
                AllFoods = foods.ToList(),
                Grams = 100 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Compare(FoodCompareViewModel model)
        {
            if (model.SelectedFoodId <= 0 || model.TargetFoodId <= 0 || model.Grams <= 0)
            {
                ModelState.AddModelError("", "Please select foods and enter grams.");
                model.AllFoods = _foodService.GetFoodsAsync().ToList();
                return View(model);
            }

            var selectedFood = await _foodService.GetFoodByIdAsync(model.SelectedFoodId);
            var targetFood = await _foodService.GetFoodByIdAsync(model.TargetFoodId);

            if (selectedFood == null || targetFood == null)
            {
                ModelState.AddModelError("", "Food not found.");
                model.AllFoods = _foodService.GetFoodsAsync().ToList();
                return View(model);
            }

            // Protein in selected food (per entered grams)
            var proteinSelected = (selectedFood.Protein / (double)selectedFood.Gram) * model.Grams;

            // Required grams of target food to match protein
            var proteinPerGramTarget = targetFood.Protein / (double)targetFood.Gram;
            var requiredGrams = proteinSelected / proteinPerGramTarget;

            model.SelectedFood = selectedFood;
            model.TargetFood = targetFood;
            model.RequiredTargetGrams = requiredGrams;
            model.AllFoods = _foodService.GetFoodsAsync().ToList();

            return View(model);
        }

        [Authorize(Roles = AppRoles.Admin)]
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
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create(Food model)
        {
            if (ModelState.IsValid)
            {
                await _foodService.AddFoodAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Edit(FoodDto model)
        {
            if (ModelState.IsValid)
            {
                _foodService.UpdateFood(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult DeleteConfirmed(int id)
        {
            _foodService.DeleteFood(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string term)
        {
            var foods = _foodService.GetFoodsAsync()
                .Where(f => string.IsNullOrEmpty(term) || f.Name!.Contains(term))
                .Select(f => new { id = f.Id, text = f.Name })
                .Take(20) // limit results
                .ToList();

            return Json(foods);
        }
    }
}
