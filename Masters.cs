using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tahlile_Parseh.Controllers
{
    public class Masters : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult panel()
        //{
        //    return View(db.sefrtosad_ONs.ToList());
        //}
        //public IActionResult new_post()
        //{
        //    return View(db.sefrtosad_HOZURIs.ToList());
        //}
        //public IActionResult add_post()
        //{
        //    return View();
        //}
        //public IActionResult del_post(int ID)
        //{
        //    var p = db.Find<sefrtosad_ON>(ID);
        //    if (p != null)
        //    {
        //        db.Remove(p);
        //        db.SaveChanges();
        //    }
        //    else
        //    {

        //    }
        //    return RedirectToAction("table_sefrtosad_ON", "AdminPanel");
        //}
    }
}
