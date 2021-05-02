using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tahlile_Parseh.Controllers
{
    public class Students : Controller
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
        //    Learn_Post _Post = new Learn_Post()
        //    {
        //        //id = learn_Post.id,
        //        title = learn_Post.title,
        //        content = learn_Post.content,
        //        link = learn_Post.link,
        //        kw = learn_Post.kw,
        //        des = learn_Post.des
        //    };
        //    if (learn_Post.image != null)
        //    {
        //        byte[] b = new byte[learn_Post.image.Length];
        //        learn_Post.image.OpenReadStream().Read(b, 0, b.Length);
        //        _Post.image = b;
        //    }

        //    db.Add(_Post);
        //    db.SaveChanges();
        //    var status = _Post;

        //    if (status != null)
        //    {
        //        TempData["msg"] = "اطلاعات ثبت گردید";
        //    }
        //    else if (status == null)
        //    {
        //        TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
        //    }


        //    return RedirectToAction("Learn_post", "AdminPanel");
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
