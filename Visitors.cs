using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tahlile_Parseh.Controllers
{
    public class Visitors : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
