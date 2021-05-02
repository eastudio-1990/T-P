using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tahlile_Parseh.Data;
using Tahlile_Parseh.Models;
using Tahlile_Parseh.ViewModels;

namespace Tahlile_Parseh.Controllers
{
    public class HomeController : Controller
    {
        DBTahlile_Parseh db;
        public HomeController(DBTahlile_Parseh _db)
        {
            db = _db;
        }


        public IActionResult Index()
        {
            ViewData["Page"] = db.pages.ToList();
            ViewData["Menu"] = db.menus.ToList();
            ViewData["Quick_N"] = db.quick_Ns.ToList();
            ViewData["Message"] = db.messages.ToList();
            ViewData["quick_text"] = db.quick_Texts.ToList();
            ViewData["quick_blog"] = db.quick_Blogs.ToList();
            ViewData["about_us_button"] = db.about_Us_Buttons.ToList();
            ViewData["about_us_header"] = db.about_Us_Headers.ToList();
            ViewData["about_us_image"] = db.about_Us_Images.ToList();
            ViewData["about_us_text"] = db.about_Us_Texts.ToList();
            ViewData["services_header_text"] = db.services_Header_Texts.ToList();
            ViewData["services_text"] = db.services_Texts.ToList();
            ViewData["single_service"] = db.single_Services.ToList();
            ViewData["requset_callback"] = db.requset_Callbacks.ToList();
            ViewData["requset_callback_image"] = db.requset_Callback_Images.ToList();
            ViewData["team_member_header_text"] = db.team_Member_Header_Texts.ToList();
            ViewData["team_member_text"] = db.team_Member_Texts.ToList();
            ViewData["portfolios_header_text"] = db.portfolios_Header_Texts.ToList();
            ViewData["portfolios_text"] = db.portfolios_Texts.ToList();
            ViewData["portfolio_single"] = db.portfolio_Singles.ToList();
            ViewData["testimonial"] = db.testimonials.ToList();
            ViewData["testimonial_header_text"] = db.testimonial_Header_Texts.ToList();
            ViewData["testimonial_text"] = db.testimonial_Texts.ToList();
            ViewData["blogs_header_text"] = db.blogs_Header_Texts.ToList();
            ViewData["blogs_text"] = db.blogs_Texts.ToList();
            ViewData["single_blog"] = db.single_Blogs.ToList();
            ViewData["brand_logo"] = db.brand_Logos.ToList();
            ViewData["call_action"] = db.call_Actions.ToList();
            ViewData["header_logo"] = db.header_Logos.ToList();
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            ViewData["hero_slide"] = db.hero_Slides.ToList();
            ViewData["footer_logo"] = db.footer_Logos.ToList();
            ViewData["footer_text"] = db.footer_Texts.ToList();
            ViewData["footer_form"] = db.footer_Forms.ToList();
            ViewData["footer_image"] = db.footer_Images.ToList();
            ViewData["footer_quick_link"] = db.footer_Quick_Links.ToList();
            ViewData["Description"] = db.descriptions.ToList();
            ViewData["Archive_Learn"] = db.archive_Learns.ToList();
            ViewData["t_jame"] = db.t_James.ToList();
            ViewData["t_fanda"] = db.t_Fandas.ToList();
            ViewData["t_navasan"] = db.t_Navasans.ToList();
            ViewData["t_tecnin"] = db.t_Tecnics.ToList();
            ViewData["footer_social"] = db.footer_Socials.ToList();
            ViewData["vip_product"] = db.vip_Products.ToList();
            ViewData["uip_product"] = db.uip_Products.ToList();
            ViewData["quick_news_header"] = db.quick_News_Headers.ToList();
            ViewData["quick_news_text"] = db.quick_News_Texts.ToList();
            ViewData["Team_member"] = db.team_Members.ToList();
            ViewData["Single_funfact"] = db.single_Funfacts.ToList();
            ViewData["Request_call_form"] = db.request_Call_Forms.ToList();
            ViewData["Bg_image"] = db.bg_Images.ToList();
            ViewData["Team_member_socialmedia"] = db.team_Member_Socialmedias.ToList();
            ViewData["VipImage"] = db.vipImages.ToList();
            ViewData["vip_product"] = db.vip_Products.ToList();
            ViewData["UipImage"] = db.uipImages.ToList();
            ViewData["uip_product"] = db.uip_Products.ToList();
            ViewData["P_P"] = db.p_Ps.ToList();

            return View();
        }

        public IActionResult Privacy_Policy()
        {

            return View(db.p_Ps.ToList()); ;
        }

        public IActionResult singleBlogId(int id)
        {
            var blog = db.Find<single_blog>(id);
            return View(blog);
            //return Json(new{content=blog.small_content,subject=blog.header,date=blog.date,author=blog.author);
        }
        public IActionResult tamas()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.tamasForms.ToList());
        }
        //public IActionResult Message() => View();
        public IActionResult _Message(MessagesViewModel messagesViewModel)
        {
            Message m = new Message
            {
                name = messagesViewModel.name,
                email = messagesViewModel.email,
                message = messagesViewModel.message,
                tell = messagesViewModel.tell
            };

            db.Add(m);
            db.SaveChanges();

            var status = m;
            if (status != null)
            {
                TempData["msg"] = "پیام شما با موفقیت ارسال شد و در اسراع وقت پاسخ داده میشود";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا فرم را تکمیل کنید";
            }
            return RedirectToAction("tamas", "Home");

        }
        public IActionResult _req_call(Request_call_form_ViewModel request_Call_Form_ViewModel)
        {
            Request_call_form q = new Request_call_form()
            {
                id = request_Call_Form_ViewModel.id,
                date = request_Call_Form_ViewModel.date,
                text = request_Call_Form_ViewModel.text,
                name = request_Call_Form_ViewModel.name,
                email = request_Call_Form_ViewModel.email
            };         
           
            var status = q;
            if (status != null)
            {
                TempData["msg"] = "پیام شما با موفقیت ارسال شد و در اسراع وقت پاسخ داده میشود";
                db.Add(q);
                db.SaveChanges();
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا فرم را تکمیل کنید";
            }
            return RedirectToAction("Index", "Home");
           
        }
        [Route("نمایندگی")]
        public IActionResult Branches()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.branches.ToList());
        }


        //   public IActionResult search(string data)

        //{
        //  Methodes.GetAll search = new Methodes.GetAll(db);            
        //return Ok(GetAll._Search(data));
        //if (data!=null)
        //{

        //Methodes.BranchesSearch search = new Methodes.BranchesSearch(db);
        //return Ok(search._Search(data));
        //}
        //return View();



        //if (data != null)
        //{
        //    data = data.ToLower();
        //    var branches = db.branches.ToList().Where(x =>
        //    (x.name != null && x.name.ToLower().Contains(data))
        //    ||
        //    (x.city != null && x.city.ToLower().Contains(data))
        //    ||
        //  (x.state != null && x.state.ToLower().Contains(data)) ||
        //  (x.code !=0 &&  x.code.ToString().ToLower().Contains(data))
        //    ).ToList();
        //    ViewData["branches"] = branches;
        //}
        //else if (data == null)
        //{
        //    ViewData["baranches"] = "نمایندگی مورد نظر یافت نشد";
        //}
        //else
        //    ViewData["branches"] = new List<Branches>();


        // return View();
        // }

        //public List<BranchesViewModel> _search(string name)
        //{
        //    List<BranchesViewModel> res = (from Branches in db.branches
        //                                   where Branches.name.Contains(name)
        //                                   select new BranchesViewModel
        //                                   {
        //                                       id=Branches.id,
        //                                       name=Branches.name,
        //                                       city=Branches.city,
        //                                       state=Branches.state,
        //                                       address=Branches.address,
        //                                       code=Branches.code,
        //                                       mobile=Branches.mobile,
        //                                        phone=Branches.phone
        //                                   }).ToList();
        //    return res;
        //}

        // public IActionResult Search_(string name)
        //{
        //    if (name!=null)
        //    {
        //        return Ok(_search(name));
        //    }
        //    else if (name==null)
        //    {
        //        return Ok(Messages.Msg.EnterName);
        //    }
        //    else
        //    {
        //        return Ok(Messages.Msg.NoRef);
        //    }

        //}


        //مطالب آموزشی
        //[Route("مطالب-آموزشی")]


        public IActionResult Learn_Posts_Show()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.learn_Posts.OrderByDescending(x => x.publishdate).ToList());
        }
        //اخبار
        //Bours
        //[Route("اخبار-بورس")]
        public IActionResult Bours_News()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.bours_Ns.OrderByDescending(x => x.id).ToList());
        }
        //Gold
        //[Route("اخبار-طلا")]
        public IActionResult Gold_News()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.gold_Ns.OrderByDescending(x => x.id).ToList());
            
        }
        //Quick
       // [Route("اخبار-فوری")]
        public IActionResult Quick_News()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();

            return View(db.quick_Ns.OrderByDescending(x => x.id).ToList());
        }
        //Archive
        //[Route("آرشیو-خبر")]
        public IActionResult Archive_News()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View(db.archive_Ns.OrderByDescending(x => x.id).ToList());
            
        }

        
   
        public IActionResult Products()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            ViewData["VipImage"] = db.vipImages.ToList();
            ViewData["vip_product"] = db.vip_Products.ToList();
            ViewData["UipImage"] = db.uipImages.ToList();
            ViewData["uip_product"] = db.uip_Products.ToList();
            return View();

        }
        public IActionResult VipImage()
        {
            return View(db.vipImages.ToList());
        }
        public IActionResult UipImage()
        {
            return View(db.uipImages.ToList());
        }
        public IActionResult vip_product()
        {
            return View(db.vip_Products.ToList());
        }
        public IActionResult uip_product()
        {
            return View(db.uip_Products.ToList());
        }
        public IActionResult shopping_cart()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
    }

}
