using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Yahyo.Models;

namespace Yahyo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarServise _servise;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,ICarServise servise)
        {
            _servise = servise;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Car car,List<IFormFile> files)
        {
            if(ModelState.IsValid)
            {
                var response = await _servise.CreateAsync(car, files);
                if(response.IsFile)
                {
                    ViewBag.message = response.Message;
                    return View();
                }
                return RedirectToAction("Main");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Main(int index = 0,int ishora = 1)
        {
           
            index += ishora;
            ViewBag.index = index;
            return View(await _servise.GetCarPaginationAsync(5, index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _servise.DeleteCarAsync(id);
            return RedirectToAction("Main");
        }
        [HttpGet]
        public async Task<IActionResult> GetPagination(int index,int length,bool min)
        {
            ViewBag.index = (min) ? ViewBag.index + 1 : ViewBag.index - 1;
            return View(await _servise.GetCarPaginationAsync(length, index));
        }
    }
}
