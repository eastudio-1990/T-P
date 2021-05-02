using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tahlile_Parseh.Areas.Identity.Data;
using Tahlile_Parseh.Data;
using Tahlile_Parseh.Models;
using Tahlile_Parseh.ViewModels;

namespace Tahlile_Parseh.Controllers
{
    public class Tahlilha : Controller
    {
       // DTime dt;

 

        UserManager<ApplicationUser> userManager { get; set; }
        public Tahlilha(UserManager<ApplicationUser> _userManager)//, DBTahlile_Parseh dB)
        {
            //db = dB;
            userManager = _userManager;
        }

        public IActionResult Index([FromServices] DBTahlile_Parseh db)
        {

            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            ViewData["Talar"] = db.talars.ToList();
            //ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }

        public IActionResult InsertTalar([FromServices] DBTahlile_Parseh db)
        {
            ViewData["Talar"] = db.talars.ToList();
            ViewData["TalarType"] = db.talarTypes.ToList();
            return View();
        }

        public IActionResult TalarList([FromServices] DBTahlile_Parseh db)
        {
            ViewData["Talar"] = db.talars.ToList();
            return View();
        }

        public IActionResult delTalarpost(int ID, [FromServices] DBTahlile_Parseh db)
        {
            var q = db.Find<Talar>(ID);
            if (q!=null)
            {
                db.Remove(q);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("InsertTalar", "Tahlilha");
        }

        public async Task<IActionResult> TalarAdding(Talar_ViewModel talar_ViewModel, [FromServices] DBTahlile_Parseh db)
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Talar t = new Talar()
            {
                id = talar_ViewModel.id,
                comment = talar_ViewModel.comment,
                commentdate = talar_ViewModel.commentdate,
                likes = 0,
                dislike = 0,
                UserId = user.Id,
                TalarTypeId = talar_ViewModel.TalarTypeId,
                namad=talar_ViewModel.namad,
                namad_link=talar_ViewModel.namad_link,
                
                
            };
            if (talar_ViewModel.image != null)
            {
                byte[] b = new byte[talar_ViewModel.image.Length];
                talar_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                t.image = b;
            }
            else
            {

            }

            db.Add(t);
            db.SaveChanges();
            return RedirectToAction("InsertTalar", "Tahlilha");
        }


        public IActionResult Tecnical([FromServices] DBTahlile_Parseh dB, int ID = 1)
        {
            ViewData["DTime"] = dB.dTimes.ToList();
            ViewData["header_top_info"] = dB.header_Top_Infos.ToList();
            ViewData["master_quick_nazar"] = dB.master_Quick_Nazars.ToList();
            return View(dB.talars.Include(x=>x.user).Where(x=>x.TalarTypeId == ID).OrderByDescending(x=>x.commentdate).ToList());
        }
    
        public IActionResult Bonyadi([FromServices] DBTahlile_Parseh dB, int ID = 2)
        {
            ViewData["header_top_info"] = dB.header_Top_Infos.ToList();
            ViewData["master_quick_nazar"] = dB.master_Quick_Nazars.ToList();
            return View(dB.talars.Include(x=>x.user).Where(x=>x.TalarTypeId == ID).OrderByDescending(x=>x.commentdate).ToList());
        }



        public IActionResult master_quick_nazar([FromServices] DBTahlile_Parseh db)
        {
            return View(db.master_Quick_Nazars.ToList());
        }
        public IActionResult master_quick_nazar_add() => View();
        public IActionResult master_quick_nazar_adding([FromServices] DBTahlile_Parseh db,master_quick_nazar_ViewModel master_Quick_Nazar_ViewModel)
        {
            master_quick_nazar q = new master_quick_nazar()
            {
                id = master_Quick_Nazar_ViewModel.id,
                text = master_Quick_Nazar_ViewModel.text
            };
            db.Add(q);
            db.SaveChanges();

            return RedirectToAction("master_quick_nazar", "Tahlilha");
        }
        public IActionResult del_master_quick_nazar(int id, [FromServices] DBTahlile_Parseh db)
        {
            var q = db.Find<master_quick_nazar>(id);
            db.Remove(q);
            db.SaveChanges();
            return RedirectToAction("master_quick_nazar", "Tahlilha");
        }

    }
}
