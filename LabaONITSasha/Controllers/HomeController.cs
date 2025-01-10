using LabaONITSasha.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
namespace LabaONITSasha.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatContext _context;

        private readonly IHttpClientFactory _httpClientFactory;
         
        public HomeController(CatContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;

            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> LoadCatsFromJson()
        {
            var jsonUrl = "https://raw.githubusercontent.com/Kutru1/JSONFile/refs/heads/main/data.json ";

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(jsonUrl);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var cats = JsonSerializer.Deserialize<List<CatModel>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View("Index", cats);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = $"Ошибка при загрузке данных: {ex.Message}";
                return View("Index", new List<CatModel>());
            }
        }
        public IActionResult Index()
        {
             var TableDB = _context.Cats.ToList();
            return View(TableDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string name, int Gender, DateTime BirthDate, DateTime DateOfPet)
        {
            int id = _context.Cats.Any() ? _context.Cats.Max(s => s.ID) + 1 : 1;
            var student = new CatModel
            {
                ID = id,
                Name = name,
                BirthDate = BirthDate,
                Gender = Gender,
                DateOfPet = DateOfPet
            };

            _context.Cats.Add(student);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Cats.FindAsync(id);
            if (student != null)
            {
                _context.Cats.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Filter(int? ageFrom, int? ageTo, DateTime? admissionBefore, DateTime? admissionAfter)
        {
            var studentsQuery = _context.Cats.AsQueryable();

            if (ageFrom.HasValue)
            {
                var fromDate = DateTime.Today.AddYears(-ageFrom.Value);
                studentsQuery = studentsQuery.Where(s => s.BirthDate <= fromDate);
            }

            if (ageTo.HasValue)
            {
                var toDate = DateTime.Today.AddYears(-ageTo.Value);
                studentsQuery = studentsQuery.Where(s => s.BirthDate >= toDate);
            }

            if (admissionBefore.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.DateOfPet <= admissionBefore.Value);
            }

            if (admissionAfter.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.DateOfPet >= admissionAfter.Value);
            }

            var filteredStudents = studentsQuery.ToList();

            return View("Index", filteredStudents);
        }
    }

}