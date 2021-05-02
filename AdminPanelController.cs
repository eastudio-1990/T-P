using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panel_Sms;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tahlile_Parseh.Areas.Identity.Data;
using Tahlile_Parseh.Data;
using Tahlile_Parseh.Models;
using Tahlile_Parseh.ViewModels;

namespace Tahlile_Parseh.Controllers
{
    //[Authorize(Policy = "AdminGroup")]
    [Authorize(Policy = "Tahlile_ParsehCustomAuthorize")]
    public class AdminPanelController : Controller
    {
        //Dependency_Injection
        UserManager<ApplicationUser> userManager { get; set; }
        DBTahlile_Parseh db;
        public AdminPanelController(UserManager<ApplicationUser> _userManager, DBTahlile_Parseh dB)
        {
            db = dB;
            userManager = _userManager;
        }

        //public IActionResult Name(LoginViewModel loginViewModel)
        //{
            
        //    return View();
        //}

        //index
        public IActionResult Index()
        {
            ViewData["ToTal_Show"] = db.toTalViews.ToList();
            ViewData["Online_Show"] = db.OnlineUsers.ToList();
            return View();
        }

        //Data_Table
        public IActionResult DataTable()
        {
            return View(userManager.Users.ToList());
        }

        public IActionResult create_role([FromServices] RoleManager<IdentityRole> roleManager)
        {
            ViewData["roles"] = roleManager.Roles.ToList();
            return View();
        }

        public async Task<IActionResult> insert_role_confirm(string name,[FromServices] RoleManager<IdentityRole> roleManager)
        {
            IdentityRole identityRole = new IdentityRole(name);
            await roleManager.CreateAsync(identityRole);

            return RedirectToAction("create_role", "AdminPanel");
        }

        public async Task<IActionResult> DeleteRole(string Id,[FromServices] RoleManager<IdentityRole> roleManager)
        {
            await roleManager.DeleteAsync(await roleManager.FindByIdAsync(Id));

            return RedirectToAction("create_role", "AdminPanel");

        }

        public IActionResult ApiRules()
        {
            return View();
        }
        //[AllowAnonymous]
        public IActionResult Role_Management()
        {
            ViewData["controllers"] = db.controller_Names.ToList();
            return View();
        }

        //[AllowAnonymous]
        public IActionResult ChangeUseraction(string userid, int actionid, bool status)

        {
            var useractionname = db.userActionNames.FirstOrDefault(x => x.actionId == actionid
            && x.userId == userid);
            if (status)
            {
                if (useractionname == null)
                {
                    db.userActionNames.Add(
                        new UserActionName { actionId = actionid, userId = userid }
                        );
                    db.SaveChanges();
                }
            }
            else
            {
                if (useractionname != null)
                {
                    db.Remove(useractionname);
                    db.SaveChanges();

                }
            }
            return Json(true);
        }
        //[AllowAnonymous]
        public IActionResult GetAction_Names(int Id,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            var actions = db.action_Names.Where(x => x.Controller_NameId == Id).ToList();
            ViewData["actions"] = actions;
            ViewData["users"] = userManager.Users.ToList();
            ViewData["userActionNames"] = db.userActionNames.ToList();

            return View();
        }
       
        public IActionResult DEL_USERS(string ID)
        {
            var p = db.Find<ApplicationUser>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("DataTable", "AdminPanel");
        }


        public IActionResult Search() => View();



        //Message & Req Call
        public IActionResult Messages()
        {
            return View(db.messages.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult deletemessid(int ID)
        {
            var p = db.Find<Message>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Messages", "AdminPanel");
        }
        public IActionResult Request_call_form()
        {
            return View(db.request_Call_Forms.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult del_Request_call_form(int ID)
        {
            var p = db.Find<Request_call_form>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Request_call_form", "AdminPanel");
        }

        //Branches
        public IActionResult branche()
        {
            return View(db.branches.ToList());
        }
        public IActionResult edit_branches() => View();
        public IActionResult _branches(Branches branchesViewModel)
        {
            Branches b = new Branches
            {
                code = branchesViewModel.code,
                name = branchesViewModel.name,
                state = branchesViewModel.state,
                city = branchesViewModel.city,
                mobile = branchesViewModel.mobile,
                phone = branchesViewModel.phone,
                address = branchesViewModel.address,
                kw = branchesViewModel.kw,
                des = branchesViewModel.des
            };

            db.Add(b);
            db.SaveChanges();
            var status = b;
            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }

            return RedirectToAction("branche", "AdminPanel");
        }
        public IActionResult deleteid(int ID)
        {
            var p = db.Find<Branches>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else if (p == null)
            {
                TempData["msg"] = "لطفا شناسه را وارد کنید";
            }

            return RedirectToAction("branche", "AdminPanel");

            //}
            //public IActionResult update(int ID)
            //{
            //    var p = db.Find<Branches>(ID);
            //    if(p != null)
            //    {
            //        db.Update(p);
            //        db.SaveChanges();
            //    }
            //    else if (p == null)
            //    {
            //        TempData["msg"] = "لطفا شناسه را وارد کنید";
            //    }

            //    return RedirectToAction("_branches", "AdminPanel");

            //}




        }



        public IActionResult form_tamas()
        {
            return View(db.tamasForms.ToList());
        }
        public IActionResult edit_tamas() => View();
        public IActionResult _tamas(TamasFormViewModel tamasFormViewModel)
        {
            TamasForm p = new TamasForm
            {
                id = tamasFormViewModel.id,
                t1 = tamasFormViewModel.t1,
                t2 = tamasFormViewModel.t2,
                t3 = tamasFormViewModel.t3,
                t4 = tamasFormViewModel.t4,
                t5 = tamasFormViewModel.t5,
                t6 = tamasFormViewModel.t6,
                t7 = tamasFormViewModel.t7,
                t8 = tamasFormViewModel.t8,
                t9 = tamasFormViewModel.t9,
                t10 = tamasFormViewModel.t10,

            };






            if (tamasFormViewModel.icon1 != null)
            {
                byte[] b = new byte[tamasFormViewModel.icon1.Length];
                tamasFormViewModel.icon1.OpenReadStream().Read(b, 0, b.Length);
                p.icon1 = b;
            }
            else { }
            if (tamasFormViewModel.icon2 != null)
            {
                byte[] b = new byte[tamasFormViewModel.icon2.Length];
                tamasFormViewModel.icon2.OpenReadStream().Read(b, 0, b.Length);
                p.icon2 = b;
            }
            else { }
            if (tamasFormViewModel.icon3 != null)
            {
                byte[] b = new byte[tamasFormViewModel.icon3.Length];
                tamasFormViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                p.icon3 = b;
            }
            else { }
            if (tamasFormViewModel.icon4 != null)
            {
                byte[] b = new byte[tamasFormViewModel.icon4.Length];
                tamasFormViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                p.icon4 = b;
            }
            else { }


            db.Add(p);
            db.SaveChanges();
            var status = p;
            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }

            return RedirectToAction("form_tamas", "AdminPanel");
        }
        public IActionResult del_tamas(int ID)
        {
            var p = db.Find<TamasForm>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else if (p == null)
            {
                TempData["msg"] = "لطفا شناسه را وارد کنید";
            }

            return RedirectToAction("form_tamas", "AdminPanel");
        }


        //دوره های آموزشی
        //صفر تا صد
        public IActionResult table_sefrtosad_ON()
        {
            return View(db.sefrtosad_ONs.ToList());
        }
        public IActionResult Table_sefrtosad_HOZURI()
        {
            return View(db.sefrtosad_HOZURIs.ToList());
        }
        public IActionResult del_sefrtosad_ON(int ID)
        {
            var p = db.Find<sefrtosad_ON>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_sefrtosad_ON", "AdminPanel");
        }
        public IActionResult del_sefrtosad_HOZURI(int ID)
        {
            var p = db.Find<sefrtosad_HOZURI>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_sefrtosad_HOZURI", "AdminPanel");
        }
        //تابلوخوانی
        public IActionResult table_tablokhani_ON()
        {
            return View(db.tablokhani_ONs.ToList());
        }
        public IActionResult Table_tablokhani_HOZURI()
        {
            return View(db.tablokhani_HOZURIs.ToList());
        }
        public IActionResult del_tablokhani_ON(int ID)
        {
            var p = db.Find<tablokhani_ON>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_tablokhani_ON", "AdminPanel");
        }
        public IActionResult del_tablokhani_HOZURI(int ID)
        {
            var p = db.Find<tablokhani_HOZURI>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_tablokhani_HOZURI", "AdminPanel");
        }
        //نوسان گیری
        public IActionResult table_navasangiri_ON()
        {
            return View(db.navasangiri_ONs.ToList());
        }
        public IActionResult Table_navasangiri_HOZURI()
        {
            return View(db.navasangiri_HOZURIs.ToList());
        }
        public IActionResult del_navasangiri_ON(int ID)
        {
            var p = db.Find<navasangiri_ON>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_navasangiri_ON", "AdminPanel");
        }
        public IActionResult del_navasangiri_HOZURI(int ID)
        {
            var p = db.Find<navasangiri_HOZURI>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_navasangiri_HOZURI", "AdminPanel");
        }
        //تحلیل تکنیکال
        public IActionResult table_tecnical_ON()
        {
            return View(db.tecnical_ONs.ToList());
        }
        public IActionResult Table_tecnical_HOZURI()
        {
            return View(db.tecnical_HOZURIs.ToList());
        }
        public IActionResult del_tecnical_ON(int ID)
        {
            var p = db.Find<tecnical_ON>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_tecnical_ON", "AdminPanel");
        }
        public IActionResult del_tecnical_HOZURI(int ID)
        {
            var p = db.Find<tecnical_HOZURI>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_tecnical_HOZURI", "AdminPanel");
        }
        //تحلیل فاندامنتال
        public IActionResult table_fandamental_ON()
        {
            return View(db.fandamental_ONs.ToList());
        }
        public IActionResult Table_fandamental_HOZURI()
        {
            return View(db.fandamental_HOZURIs.ToList());
        }
        public IActionResult del_fandamental_ON(int ID)
        {
            var p = db.Find<fandamental_ON>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_fandamental_ON", "AdminPanel");
        }
        public IActionResult del_fandamental_HOZURI(int ID)
        {
            var p = db.Find<fandamental_HOZURI>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_fandamental_HOZURI", "AdminPanel");
        }
        //همایش ها
        public IActionResult table_hamayesh()
        {
            return View(db.hamayeshes.ToList());
        }
        public IActionResult del_hamayesh(int ID)
        {
            var p = db.Find<hamayesh>(ID);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("table_hamayesh", "AdminPanel");
        }
        //مطالب آموزشی
        public IActionResult Learn_post()
        {
            return View(db.learn_Posts.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult Add_Learn_post()
        {
            ViewData["date"] = CalendarManager.JulianToPersian(DateTime.Now);
            return View();
        }
        public IActionResult Adding_Learn_post(Learn_PostViewModel learn_Post)
        {
            Learn_Post _Post = new Learn_Post()
            {
                //id = learn_Post.id,
                title = learn_Post.title,
                content = learn_Post.content,
                link = learn_Post.link,
                kw = learn_Post.kw,
                des = learn_Post.des
            };
            if (learn_Post.image != null)
            {
                byte[] b = new byte[learn_Post.image.Length];
                learn_Post.image.OpenReadStream().Read(b, 0, b.Length);
                _Post.image = b;
            }

            db.Add(_Post);
            db.SaveChanges();
            var status = _Post;

            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }


            return RedirectToAction("Learn_post", "AdminPanel");
        }
        public IActionResult deleteid_Learn_post(int id)
        {

            var p = db.Find<Learn_Post>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Learn_post", "AdminPanel");
        }
        //اخبار
        //Gold
        public IActionResult Edit_Gold()
        {
            return View(db.gold_Ns.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult Add_Gold_post() => View();
        public IActionResult Adding_Gold_post(Gold_NViewModel gold_NViewModel, Archive_NViewModel archive_NViewModel)
        {
            Gold_N _Post = new Gold_N()
            {
                id = gold_NViewModel.id,
                Title = gold_NViewModel.Title,
                Content = gold_NViewModel.Content,
                link = gold_NViewModel.link,
                kw = gold_NViewModel.kw,
                des = gold_NViewModel.des
            };

            if (gold_NViewModel.image != null)
            {
                byte[] b = new byte[gold_NViewModel.image.Length];
                gold_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                _Post.image = b;
            }

            Archive_N A_Post = new Archive_N()
            {
                //id = archive_NViewModel.id,
                Title = archive_NViewModel.Title,
                Content = archive_NViewModel.Content,
                link = archive_NViewModel.link,
                kw = archive_NViewModel.kw,
                des = archive_NViewModel.des


            };
            //if (archive_NViewModel.image != null)
            //{
            //    byte[] b = new byte[archive_NViewModel.image.Length];
            //    archive_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
            //    A_Post.image = b;

            //}
            db.Add(A_Post);
            db.Add(_Post);
            db.SaveChanges();
            var status = _Post;

            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }


            return RedirectToAction("Edit_Gold", "AdminPanel");
        }
        public IActionResult deleteid_Gold_post(int id)
        {
            var p = db.Find<Gold_N>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Edit_Gold", "AdminPanel");
        }
        //Bours
        public IActionResult Edit_Bours()
        {
            return View(db.bours_Ns.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult Add_Bours_post()
        {
            ViewData["date"] = CalendarManager.JulianToPersian(DateTime.Now);
            return View();
        }
        public IActionResult editid_Bours_post()
        {
            return View();
        }
        public IActionResult editid_post(Bours_NViewModel vm)
        {
            Bours_N post = new Bours_N();
            post.id = vm.id;
            post.Title = vm.Title;
            post.link_text = vm.link_text;
            post.link = vm.link;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            var status = post;

            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا یکی از فیلدها را پر کنید";
            }
            return RedirectToAction("Edit_Bours", "AdminPanel");
        }
        public IActionResult Adding_Bours_post(Bours_NViewModel bours_NViewModel, Archive_NViewModel archive_NViewModel)
        {
            Bours_N _Post = new Bours_N()
            {
                id = bours_NViewModel.id,
                Title = bours_NViewModel.Title,
                Content = bours_NViewModel.Content,
                link = bours_NViewModel.link,
                kw = bours_NViewModel.kw,
                des = bours_NViewModel.des
                //publishdate = CalendarManager.PersianToJulian(bours_NViewModel.publishdatepersian)

            };
            Archive_N A_Post = new Archive_N()
            {
                //id = archive_NViewModel.id,
                Title = archive_NViewModel.Title,
                Content = archive_NViewModel.Content,
                link = archive_NViewModel.link,
                kw = archive_NViewModel.kw,
                des = archive_NViewModel.des
            };
            //if (archive_NViewModel.image != null)
            //{
            //    byte[] b = new byte[archive_NViewModel.image.Length];
            //    archive_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
            //    A_Post.image = b;

            //}
            db.Add(A_Post);
            if (bours_NViewModel.image != null)
            {
                byte[] b = new byte[bours_NViewModel.image.Length];
                bours_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                _Post.image = b;
            }

            db.Add(_Post);
            db.SaveChanges();
            var status = _Post;

            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }


            return RedirectToAction("Edit_Bours", "AdminPanel");
        }
        public IActionResult deleteid_Bours_post(int id)
        {
            var p = db.Find<Bours_N>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Edit_Bours", "AdminPanel");
        }
        //Quick
        public IActionResult Edit_Quick()
        {
            return View(db.quick_Ns.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult Add_Quick_post()
        {
            ViewData["date"] = CalendarManager.JulianToPersian(DateTime.Now);
            return View();
        }
        public IActionResult Adding_Quick_post(Quick_NViewModel quick_NViewModel, Archive_NViewModel archive_NViewModel)
        {
            Quick_N _Post = new Quick_N()
            {
                id = quick_NViewModel.id,
                Title = quick_NViewModel.Title,
                Content = quick_NViewModel.Content,
                link = quick_NViewModel.link,
                link_text=quick_NViewModel.link_text,
                kw = quick_NViewModel.kw,
                des = quick_NViewModel.des
            };
            Archive_N A_Post = new Archive_N()
            {
                //id = archive_NViewModel.id,
                Title = archive_NViewModel.Title,
                Content = archive_NViewModel.Content,
                link = archive_NViewModel.link,
                kw = archive_NViewModel.kw,
                des = archive_NViewModel.des
            };
            db.Add(A_Post);

            if (quick_NViewModel.image != null)
            {
                byte[] b = new byte[quick_NViewModel.image.Length];
                quick_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                _Post.image = b;
            }
            //if (archive_NViewModel.image != null)
            //{
            //    byte[] b = new byte[archive_NViewModel.image.Length];
            //    archive_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
            //    A_Post.image = b;

            //}

            db.Add(_Post);
            db.SaveChanges();

            var status = _Post;

            if (status != null)
            {
                TempData["msg"] = "اطلاعات ثبت گردید";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            }


            return RedirectToAction("Edit_Quick", "AdminPanel");
        }
        public IActionResult deleteid_Quick_post(int id)
        {
            var p = db.Find<Quick_N>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Edit_Quick", "AdminPanel");
        }
        //Archive
        public IActionResult Edit_Archive()
        {
            return View(db.archive_Ns.OrderByDescending(x => x.id).ToList());
        }
        public IActionResult Add_Archive_post()
        {
            //ViewData["date"] = CalendarManager.JulianToPersian(DateTime.Now);
            return View();
        }
        public IActionResult Adding_Archive_post(Archive_NViewModel archive_NViewModel)
        {
            Archive_N _Post = new Archive_N()
            {
                // id = archive_NViewModel.id,
                Title = archive_NViewModel.Title,
                Content = archive_NViewModel.Content,
                link = archive_NViewModel.link,
                kw = archive_NViewModel.kw,
                des = archive_NViewModel.des

            };

            //if (archive_NViewModel.image != null)
            //{
            //    byte[] b = new byte[archive_NViewModel.image.Length];
            //    archive_NViewModel.image.OpenReadStream().Read(b, 0, b.Length);
            //    _Post.image = b;

            //}
            db.Add(_Post);
            db.SaveChanges();

            //var status = _Post;

            //if (status != null)
            //{
            //    TempData["msg"] = "اطلاعات ثبت گردید";
            //}
            //else if (status == null)
            //{
            //    TempData["msg"] = "لطفا تمامی فیلدها را پر کنید";
            //}


            return RedirectToAction("Edit_Archive", "AdminPanel");
        }
        public IActionResult deleteid_Archive_post(int id)
        {
            var p = db.Find<Archive_N>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            else
            {

            }
            return RedirectToAction("Edit_Archive", "AdminPanel");
        }

        //Edit Banner Of Registrations
        //Edit 0ta100
        public IActionResult jame()
        {
            return View(db.t_James.ToList());
        }
        public IActionResult edit_jame() => View();
        public IActionResult change_jame(t_jameViewModel t_JameViewModel)
        {
            t_jame t = new t_jame()
            {
                header_text = t_JameViewModel.header_text,
                middle_text = t_JameViewModel.middle_text,
                tic1 = t_JameViewModel.tic1,
                tic2 = t_JameViewModel.tic2,
                tic3 = t_JameViewModel.tic3,
                tic4 = t_JameViewModel.tic4,
            };
            if (t_JameViewModel.image != null)
            {
                byte[] b = new byte[t_JameViewModel.image.Length];
                t_JameViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                t.image = b;
            }
            db.Add(t);
            db.SaveChanges();
            return RedirectToAction("jame", "AdminPanel");
        }
        public IActionResult del_jame(int id)
        {
            var p = db.Find<t_jame>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("jame", "AdminPanel");
        }
        //Edit Navasangiri
        public IActionResult navasan()
        {
            return View(db.t_Navasans.ToList());
        }
        public IActionResult edit_navasan() => View();
        public IActionResult change_navasan(t_navasanViewModel t_NavasanViewModel)
        {
            t_navasan t = new t_navasan()
            {
                header_text = t_NavasanViewModel.header_text,
                middle_text = t_NavasanViewModel.middle_text,
                tic1 = t_NavasanViewModel.tic1,
                tic2 = t_NavasanViewModel.tic2,
                tic3 = t_NavasanViewModel.tic3,
                tic4 = t_NavasanViewModel.tic4,
            };
            if (t_NavasanViewModel.image != null)
            {
                byte[] b = new byte[t_NavasanViewModel.image.Length];
                t_NavasanViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                t.image = b;
            }
            db.Add(t);
            db.SaveChanges();
            return RedirectToAction("navasan", "AdminPanel");
        }
        public IActionResult del_navasan(int id)
        {
            var p = db.Find<t_navasan>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("navasan", "AdminPanel");
        }
        //Edit Tecnical
        public IActionResult tecnic()
        {
            return View(db.t_Tecnics.ToList());
        }
        public IActionResult edit_tecnic() => View();
        public IActionResult change_tecnic(t_tecnicViewModel t_TecnicViewModel)
        {
            t_tecnic t = new t_tecnic()
            {
                header_text = t_TecnicViewModel.header_text,
                middle_text = t_TecnicViewModel.middle_text,
                tic1 = t_TecnicViewModel.tic1,
                tic2 = t_TecnicViewModel.tic2,
                tic3 = t_TecnicViewModel.tic3,
                tic4 = t_TecnicViewModel.tic4,
            };
            if (t_TecnicViewModel.image != null)
            {
                byte[] b = new byte[t_TecnicViewModel.image.Length];
                t_TecnicViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                t.image = b;
            }
            db.Add(t);
            db.SaveChanges();
            return RedirectToAction("tecnic", "AdminPanel");
        }
        public IActionResult del_tecnic(int id)
        {
            var p = db.Find<t_tecnic>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("tecnic", "AdminPanel");
        }
        //Edit Fandamental
        public IActionResult fanda()
        {
            return View(db.t_Fandas.ToList());
        }
        public IActionResult edit_fanda() => View();
        public IActionResult change_fanda(t_fandaViewModel t_fandaViewModel)
        {
            t_fanda t = new t_fanda()
            {
                header_text = t_fandaViewModel.header_text,
                middle_text = t_fandaViewModel.middle_text,
                tic1 = t_fandaViewModel.tic1,
                tic2 = t_fandaViewModel.tic2,
                tic3 = t_fandaViewModel.tic3,
                tic4 = t_fandaViewModel.tic4,
            };
            if (t_fandaViewModel.image != null)
            {
                byte[] b = new byte[t_fandaViewModel.image.Length];
                t_fandaViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                t.image = b;
            }
            db.Add(t);
            db.SaveChanges();
            return RedirectToAction("fanda", "AdminPanel");
        }
        public IActionResult del_fanda(int id)
        {
            var p = db.Find<t_fanda>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("fanda", "AdminPanel");
        }
        //Media
        //SMS
        public IActionResult SmsPanel() => View();
        public async Task<IActionResult> SendSms(string mobile, string msg)
        {

            Panel_Sms.SendServiceClient sendServiceClient = new Panel_Sms.SendServiceClient
            {
            };

            SendSMSRequest smsmsg = new SendSMSRequest
            {
                fromNumber = "500011110000330",
                isFlash = false,
                messageContent = msg,
                toNumbers = new[] { mobile },
                userName = "d.09172011371",
                password = "94500"
            };
            var s = await sendServiceClient.SendSMSAsync(smsmsg);
            if (s.SendSMSResult == 1)
            {
                TempData["msg"] = "موفق ! پیامک ارسال شد";
            }
            else
            {
                TempData["msg"] = "خطا ! پیامک ارسال نشد";
            }
            return RedirectToAction("SmsPanel", "AdminPanel");

        }
        //Email
        public IActionResult Email() => View();
        public IActionResult Send_Email(string des, string msg, IFormFile file)
        {

            return RedirectToAction("Email", "AdminPanel");
        }
        //KeyWords & Description Of Page & Backlinks





        //Theme_Edite
       
        public IActionResult header_top_info()
        {
            return View(db.header_Top_Infos.ToList());
        }
        public IActionResult header_top_info_add() => View();
        public IActionResult header_top_info_adding(header_top_info_ViewModel header_Top_info_ViewModel)
        {
            header_top_info q = new header_top_info()
            {
                id = header_Top_info_ViewModel.id,
                email=header_Top_info_ViewModel.email,
                email_link=header_Top_info_ViewModel.email_link,
                tell=header_Top_info_ViewModel.tell,
                tell_link=header_Top_info_ViewModel.tell_link,
                workdays=header_Top_info_ViewModel.workdays               
            };
           
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("header_top_info", "AdminPanel");
        }
        public IActionResult del_header_top_info(int id)
        {
            var p = db.Find<header_top_info>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("header_top_info", "AdminPanel");
        }



        public IActionResult text_slide()
        {
            return View(db.hero_Slides.ToList());
        }
        public IActionResult text_slide_add() => View();
        public IActionResult text_slide_adding(hero_slide_ViewModel hero_Slide_ViewModel)
        {
            hero_slide hero_Slide = new hero_slide()
            {
                alt_image = hero_Slide_ViewModel.alt_image,
                span_text = hero_Slide_ViewModel.span_text,
                span_text_color = hero_Slide_ViewModel.span_text_color,
                b_text = hero_Slide_ViewModel.b_text,
                b_text_color = hero_Slide_ViewModel.b_text_color,
                p_text = hero_Slide_ViewModel.p_text,
                p_text_color = hero_Slide_ViewModel.p_text_color,
                btn1_label = hero_Slide_ViewModel.btn1_label,
                btn1_bg_color = hero_Slide_ViewModel.btn1_bg_color,
                btn1_link = hero_Slide_ViewModel.btn1_link,
                btn2_label = hero_Slide_ViewModel.btn2_label,
                btn2_bg_color = hero_Slide_ViewModel.btn2_bg_color,
                btn2_link = hero_Slide_ViewModel.btn2_link,
            };
            if (hero_Slide_ViewModel.bgimage != null)
            {
                byte[] b = new byte[hero_Slide_ViewModel.bgimage.Length];
                hero_Slide_ViewModel.bgimage.OpenReadStream().Read(b, 0, b.Length);
                hero_Slide.bgimage = b;
            }
            db.Add(hero_Slide);
            db.SaveChanges();
            return RedirectToAction("text_slide", "AdminPanel");
        }
        public IActionResult del_text_slide(int id)
        {
            var p = db.Find<hero_slide>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("text_slide", "AdminPanel");
        }

        public IActionResult quick_news_header()
        {
            return View(db.quick_News_Headers.ToList());
        }
        public IActionResult quick_news_header_add() => View();
        public IActionResult quick_news_header_adding(quick_news_header quick_News_Header)
        {
            quick_news_header q = new quick_news_header()
            {
                text = quick_News_Header.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("quick_news_header", "AdminPanel");
        }
        public IActionResult del_quick_news_header(int id)
        {
            var p = db.Find<quick_news_header>(id);
            db.Remove(p);
            db.SaveChanges();
            return RedirectToAction("quick_news_header", "AdminPanel");
        }

        public IActionResult update_quick_news_header(string   text,int id,quick_news_header_ViewModel quick_News_Header)
        {
            quick_news_header user = new quick_news_header() 
            {
                id = quick_News_Header.id,
                text = quick_News_Header.text
            };

            db.Update(user);
            db.SaveChanges();
            return RedirectToAction("quick_news_header", "AdminPanel");

        }


        public IActionResult quick_news_text()
        {
            return View(db.quick_News_Texts.ToList());
        }
        public IActionResult quick_news_text_add() => View();
        public IActionResult quick_news_text_adding(quick_news_text_ViewModel quick_News_Text_ViewModel)
        {
            quick_news_text q = new quick_news_text()
            {
                text = quick_News_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("quick_news_text", "AdminPanel");
        }
        public IActionResult del_quick_news_text(int id)
        {
            var p = db.Find<quick_news_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("quick_news_text", "AdminPanel");
        }

        public IActionResult about_us_header()
        {
            return View(db.about_Us_Headers.ToList());
        }
        public IActionResult about_us_header_add() => View();
        public IActionResult about_us_header_adding(about_us_header_ViewModel about_Us_Header_View)
        {
            about_us_header q = new about_us_header()
            {
                text = about_Us_Header_View.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("about_us_header", "AdminPanel");
        }
        public IActionResult del_about_us_header(int id)
        {
            var p = db.Find<about_us_header>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("about_us_header", "AdminPanel");
        }

        public IActionResult services_header_text()
        {
            return View(db.services_Header_Texts.ToList());
        }
        public IActionResult services_header_text_add() => View();
        public IActionResult services_header_text_adding(services_header_text_ViewModel services_Header_Text_ViewModel)
        {
            services_header_text q = new services_header_text()
            {
                text = services_Header_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("services_header_text", "AdminPanel");
        }
        public IActionResult del_services_header_text(int id)
        {
            var p = db.Find<services_header_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("services_header_text", "AdminPanel");
        }

        public IActionResult services_text()
        {
            return View(db.services_Texts.ToList());
        }
        public IActionResult services_text_add() => View();
        public IActionResult services_text_adding(services_text_ViewModel services_Text_ViewModel)
        {
            services_text q = new services_text()
            {
                text = services_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("services_text", "AdminPanel");
        }
        public IActionResult del_services_text(int id)
        {
            var p = db.Find<services_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("services_text", "AdminPanel");
        }

        public IActionResult team_member_header_text()
        {
            return View(db.team_Member_Header_Texts.ToList());
        }
        public IActionResult team_member_header_text_add() => View();
        public IActionResult team_member_header_text_adding(team_member_header_text_ViewModel team_Member_Header_Text_ViewModel)
        {
            team_member_header_text q = new team_member_header_text()
            {
                text = team_Member_Header_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("team_member_header_text", "AdminPanel");
        }
        public IActionResult del_team_member_header_text(int id)
        {
            var p = db.Find<team_member_header_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("team_member_header_text", "AdminPanel");
        }

        public IActionResult team_member_text()
        {
            return View(db.team_Member_Texts.ToList());
        }
        public IActionResult team_member_text_add() => View();
        public IActionResult team_member_text_adding(team_member_text_ViewModel team_Member_Text_ViewModel)
        {
            team_member_text q = new team_member_text()
            {
                text = team_Member_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("team_member_text", "AdminPanel");
        }
        public IActionResult del_team_member_text(int id)
        {
            var p = db.Find<team_member_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("team_member_text", "AdminPanel");
        }

        public IActionResult portfolios_header_text()
        {
            return View(db.portfolios_Header_Texts.ToList());
        }
        public IActionResult portfolios_header_text_add() => View();
        public IActionResult portfolios_header_text_adding(portfolios__header_text_ViewModel portfolios__Header_Text_ViewModel)
        {
            portfolios_header_text q = new portfolios_header_text()
            {
                text = portfolios__Header_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("portfolios_header_text", "AdminPanel");
        }
        public IActionResult del_portfolios_header_text(int id)
        {
            var p = db.Find<portfolios_header_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("portfolios_header_text", "AdminPanel");
        }

        public IActionResult portfolios_text()
        {
            return View(db.portfolios_Texts.ToList());
        }
        public IActionResult portfolios_Texts_add() => View();
        public IActionResult portfolios_Texts_adding(portfolios_text_ViewModel portfolios_Text_ViewModel)
        {
            portfolios_text q = new portfolios_text()
            {
                text = portfolios_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("portfolios_text", "AdminPanel");
        }
        public IActionResult del_portfolios_text(int id)
        {
            var p = db.Find<portfolios_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("portfolios_text", "AdminPanel");
        }

        public IActionResult testimonial_header_text()
        {
            return View(db.testimonial_Header_Texts.ToList());
        }
        public IActionResult testimonial_header_text_add() => View();
        public IActionResult testimonial_header_text_adding(testimonial_header_text_ViewModel testimonial_Header_Text_ViewModel)
        {
            testimonial_header_text q = new testimonial_header_text()
            {
                text = testimonial_Header_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("testimonial_header_text", "AdminPanel");
        }
        public IActionResult del_testimonial_header_text(int id)
        {
            var p = db.Find<testimonial_header_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("testimonial_header_text", "AdminPanel");
        }

        public IActionResult testimonial_text()
        {
            return View(db.testimonial_Texts.ToList());
        }
        public IActionResult testimonial_text_add() => View();
        public IActionResult testimonial_text_adding(testimonial_text_ViewModel testimonial_Text_ViewModel)
        {
            testimonial_text q = new testimonial_text()
            {
                text = testimonial_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("testimonial_text", "AdminPanel");
        }
        public IActionResult del_testimonial_text(int id)
        {
            var p = db.Find<testimonial_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("testimonial_text", "AdminPanel");
        }

        public IActionResult blogs_header_text()
        {
            return View(db.blogs_Header_Texts.ToList());
        }
        public IActionResult blogs_header_text_add() => View();
        public IActionResult blogs_header_text_adding(blogs_header_text_ViewModel blogs_Header_Text_ViewModel)
        {
            blogs_header_text q = new blogs_header_text()
            {
                text = blogs_Header_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("blogs_header_text", "AdminPanel");
        }
        public IActionResult del_blogs_header_text(int id)
        {
            var p = db.Find<blogs_header_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("blogs_header_text", "AdminPanel");
        }

        public IActionResult blogs_text()
        {
            return View(db.blogs_Texts.ToList());
        }
        public IActionResult blogs_text_add() => View();
        public IActionResult blogs_text_adding(blogs_text_ViewModel blogs_Text_ViewModel)
        {
            blogs_text q = new blogs_text()
            {
                text = blogs_Text_ViewModel.text
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("blogs_text", "AdminPanel");
        }
        public IActionResult del_blogs_text(int id)
        {
            var p = db.Find<blogs_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("blogs_text", "AdminPanel");
        }

        public IActionResult about_us_image()
        {
            return View(db.about_Us_Images.ToList());
        }
        public IActionResult about_us_image_add() => View();
        public IActionResult about_us_image_adding(about_us_image_ViewModel about_Us_Image)
        {
            about_us_image q = new about_us_image()
            {
                id = about_Us_Image.id,
                alt = about_Us_Image.alt
            };

            if (about_Us_Image.image != null)
            {
                byte[] b = new byte[about_Us_Image.image.Length];
                about_Us_Image.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }


            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("about_us_image", "AdminPanel");
        }
        public IActionResult del_about_us_image(int id)
        {
            var p = db.Find<about_us_image>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("about_us_image", "AdminPanel");
        }

        public IActionResult about_us_button()
        {
            return View(db.about_Us_Buttons.ToList());
        }
        public IActionResult about_us_button_add() => View();
        public IActionResult about_us_button_adding(about_us_button_ViewModel about_Us_Button_ViewModel)
        {
            about_us_button q = new about_us_button()
            {
                text = about_Us_Button_ViewModel.text,
                link = about_Us_Button_ViewModel.link
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("about_us_button", "AdminPanel");
        }
        public IActionResult del_about_us_button(int id)
        {
            var p = db.Find<about_us_button>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("about_us_button", "AdminPanel");
        }
        public IActionResult about_us_text()
        {
            return View(db.about_Us_Texts.ToList());
        }
        public IActionResult about_us_text_add() => View();
        public IActionResult about_us_text_adding(about_us_text_ViewModel about_Us_Text_ViewModel)
        {
            about_us_text q = new about_us_text()
            {
                header = about_Us_Text_ViewModel.header,
                ch1 = about_Us_Text_ViewModel.ch1,
                ch2 = about_Us_Text_ViewModel.ch2,
                ch3 = about_Us_Text_ViewModel.ch3,
                ch4 = about_Us_Text_ViewModel.ch4,
                ch5 = about_Us_Text_ViewModel.ch5,
                id = about_Us_Text_ViewModel.id,

            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("about_us_text", "AdminPanel");
        }
        public IActionResult del_about_us_text(int id)
        {
            var p = db.Find<about_us_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("about_us_text", "AdminPanel");
        }

        public IActionResult single_service()
        {
            return View(db.single_Services.ToList());
        }
        public IActionResult single_service_add() => View();
        public IActionResult single_service_adding(single_service_ViewModel single_Service_ViewModel)
        {
            single_service q = new single_service()
            {
                id = single_Service_ViewModel.id,
                header = single_Service_ViewModel.header,
                button_text = single_Service_ViewModel.button_text,
                button_link = single_Service_ViewModel.button_link,
                content = single_Service_ViewModel.content,
                icon = single_Service_ViewModel.icon
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("single_service", "AdminPanel");
        }
        public IActionResult del_single_service(int id)
        {
            var p = db.Find<single_service>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("single_service", "AdminPanel");
        }

        public IActionResult requset_callback_image()
        {
            return View(db.requset_Callback_Images.ToList());
        }
        public IActionResult requset_callback_image_add() => View();
        public IActionResult requset_callback_image_adding(requset_callback_image_ViewModel requset_Callback_Image_ViewModel)
        {
            requset_callback_image q = new requset_callback_image()
            {
                alt = requset_Callback_Image_ViewModel.alt,
                header = requset_Callback_Image_ViewModel.header,
                id = requset_Callback_Image_ViewModel.id

            };

            if (requset_Callback_Image_ViewModel.image != null)
            {
                byte[] b = new byte[requset_Callback_Image_ViewModel.image.Length];
                requset_Callback_Image_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("requset_callback_image", "AdminPanel");
        }
        public IActionResult del_requset_callback_image(int id)
        {
            var p = db.Find<requset_callback_image>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("requset_callback_image", "AdminPanel");
        }

        public IActionResult Single_funfact()
        {
            return View(db.single_Funfacts.ToList());
        }
        public IActionResult Single_funfact_add() => View();
        public IActionResult Single_funfact_adding(Single_funfact_ViewModel single_Funfact_ViewModel)
        {
            Single_funfact q = new Single_funfact()
            {
                span = single_Funfact_ViewModel.span,
                h5 = single_Funfact_ViewModel.h5,
                id = single_Funfact_ViewModel.id,
                icon = single_Funfact_ViewModel.icon
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Single_funfact", "AdminPanel");
        }
        public IActionResult del_Single_funfact(int id)
        {
            var p = db.Find<Single_funfact>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("Single_funfact", "AdminPanel");
        }

        public IActionResult Team_member()
        {
            return View(db.team_Members.ToList());
        }
        public IActionResult Team_member_add() => View();
        public IActionResult Team_member_adding(Team_member_ViewModel team_Member_ViewModel)
        {
            Team_member q = new Team_member()
            {
                id = team_Member_ViewModel.id,
                name = team_Member_ViewModel.name,
                image_alt = team_Member_ViewModel.image_alt,
                p = team_Member_ViewModel.p,
                socialmedia_icon = team_Member_ViewModel.socialmedia_icon,
                socialmedia_link = team_Member_ViewModel.socialmedia_link,
                socialmedia_title = team_Member_ViewModel.socialmedia_title,
            };
            if (team_Member_ViewModel.image != null)
            {
                byte[] b = new byte[team_Member_ViewModel.image.Length];
                team_Member_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Team_member", "AdminPanel");
        }
        public IActionResult del_Team_member(int id)
        {
            var p = db.Find<Team_member>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("Team_member", "AdminPanel");
        }

        public IActionResult portfolio_single()
        {
            return View(db.portfolio_Singles.ToList());
        }
        public IActionResult portfolio_single_add() => View();
        public IActionResult portfolio_single_adding(portfolio_single_ViewModel portfolio_Single_ViewModel)
        {
            portfolio_single q = new portfolio_single()
            {
                id = portfolio_Single_ViewModel.id,
                top_text = portfolio_Single_ViewModel.top_text,
                top_text_link = portfolio_Single_ViewModel.top_text_link,
                reg_text = portfolio_Single_ViewModel.reg_text,
                reg_link = portfolio_Single_ViewModel.reg_link
            };
            if (portfolio_Single_ViewModel.image != null)
            {
                byte[] b = new byte[portfolio_Single_ViewModel.image.Length];
                portfolio_Single_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("portfolio_single", "AdminPanel");
        }
        public IActionResult del_portfolio_single(int id)
        {
            var p = db.Find<portfolio_single>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("portfolio_single", "AdminPanel");
        }


        public IActionResult testimonial()
        {
            return View(db.testimonials.ToList());
        }
        public IActionResult testimonial_add() => View();
        public IActionResult testimonial_adding(testimonial_ViewModel testimonial_ViewModel)
        {
            testimonial q = new testimonial()
            {
                big_text = testimonial_ViewModel.big_text,
                id = testimonial_ViewModel.id,
                name = testimonial_ViewModel.name,
                place = testimonial_ViewModel.place,
            };
            if (testimonial_ViewModel.image != null)
            {
                byte[] b = new byte[testimonial_ViewModel.image.Length];
                testimonial_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("testimonial", "AdminPanel");
        }
        public IActionResult del_testimonial(int id)
        {
            var p = db.Find<testimonial>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("testimonial", "AdminPanel");
        }

        public IActionResult single_blog()
        {
            return View(db.single_Blogs.ToList());
        }
        public IActionResult single_blog_add() => View();
        public IActionResult single_blog_adding(single_blog_ViewModel single_Blog_ViewModel)
        {
            single_blog q = new single_blog()
            {
                id = single_Blog_ViewModel.id,
                alt = single_Blog_ViewModel.alt,
                author = single_Blog_ViewModel.author,
                date = single_Blog_ViewModel.date,
                header = single_Blog_ViewModel.header,
                small_content = single_Blog_ViewModel.small_content,
                link_more = single_Blog_ViewModel.link_more
            };
            if (single_Blog_ViewModel.image != null)
            {
                byte[] b = new byte[single_Blog_ViewModel.image.Length];
                single_Blog_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }


            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("single_blog", "AdminPanel");
        }
        public IActionResult del_single_blog(int id)
        {
            var p = db.Find<single_blog>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("single_blog", "AdminPanel");
        }




        public IActionResult footer_logo()
        {
            return View(db.footer_Logos.ToList());
        }
        public IActionResult footer_logo_add() => View();
        public IActionResult footer_logo_adding(footer_logo_ViewModel footer_Logo_ViewModel)
        {
            footer_logo q = new footer_logo()
            {
                id = footer_Logo_ViewModel.id,
                link = footer_Logo_ViewModel.link
            };

            if (footer_Logo_ViewModel.image != null)
            {
                byte[] b = new byte[footer_Logo_ViewModel.image.Length];
                footer_Logo_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_logo", "AdminPanel");
        }
        public IActionResult del_footer_logo(int id)
        {
            var p = db.Find<footer_logo>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_logo", "AdminPanel");
        }

        public IActionResult footer_social()
        {
            return View(db.footer_Socials.ToList());
        }
        public IActionResult footer_social_add() => View();
        public IActionResult footer_social_adding(footer_social_ViewModel footer_social_ViewModel)
        {
            footer_social q = new footer_social()
            {
                id = footer_social_ViewModel.id,
                title = footer_social_ViewModel.title,
                link = footer_social_ViewModel.link,
                icon = footer_social_ViewModel.icon
            };

            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_social", "AdminPanel");
        }
        public IActionResult del_footer_social(int id)
        {
            var p = db.Find<footer_social>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_social", "AdminPanel");
        }

        public IActionResult footer_text()
        {
            return View(db.footer_Texts.ToList());
        }
        public IActionResult footer_text_add() => View();
        public IActionResult footer_text_adding(footer_text_ViewModel footer_Text_ViewModel)
        {
            footer_text q = new footer_text()
            {
                id = footer_Text_ViewModel.id,
                text = footer_Text_ViewModel.text,
                color = footer_Text_ViewModel.color,
                font = footer_Text_ViewModel.font
            };


            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_text", "AdminPanel");
        }
        public IActionResult del_footer_Text(int id)
        {
            var p = db.Find<footer_text>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_text", "AdminPanel");
        }

        public IActionResult footer_image()
        {
            return View(db.footer_Images.ToList());
        }
        public IActionResult footer_image_add() => View();
        public IActionResult footer_image_adding(footer_image_ViewModel footer_Image_ViewModel)
        {
            footer_image q = new footer_image();

            if (footer_Image_ViewModel.image != null)
            {
                byte[] b = new byte[footer_Image_ViewModel.image.Length];
                footer_Image_ViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }


            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_image", "AdminPanel");
        }
        public IActionResult del_footer_image(int id)
        {
            var p = db.Find<footer_image>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_image", "AdminPanel");
        }

        public IActionResult footer_quick_link()
        {
            return View(db.footer_Quick_Links.ToList());
        }
        public IActionResult footer_quick_link_add() => View();
        public IActionResult footer_quick_link_adding(footer_quick_link_ViewModel footer_quick_Link_ViewModel)
        {
            footer_quick_link q = new footer_quick_link()
            {
                id = footer_quick_Link_ViewModel.id,
                color = footer_quick_Link_ViewModel.color,
                text = footer_quick_Link_ViewModel.text,
                link = footer_quick_Link_ViewModel.link
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_quick_link", "AdminPanel");
        }
        public IActionResult del_footer_quick_link(int id)
        {
            var p = db.Find<footer_quick_link>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_quick_link", "AdminPanel");
        }

        public IActionResult footer_form()
        {
            return View(db.footer_Forms.ToList());
        }
        public IActionResult footer_form_add() => View();
        public IActionResult footer_form_adding(footer_form_ViewModel footer_Form_ViewModel)
        {
            footer_form q = new footer_form()
            {
                id = footer_Form_ViewModel.id,
                text = footer_Form_ViewModel.text
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("footer_form", "AdminPanel");
        }
        public IActionResult del_footer_form(int id)
        {
            var p = db.Find<footer_form>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("footer_form", "AdminPanel");
        }

        //Description Of Page
        public IActionResult Description_()
        {
            return View(db.descriptions.ToList());
        }
        public IActionResult adddescription() => View();
        public IActionResult addingdes(DescriptionViewModel descriptionViewModel)
        {
            Description p = new Description()
            {
                id = descriptionViewModel.id,
                des = descriptionViewModel.des
            };
            db.Add(p);
            db.SaveChanges();
            return RedirectToAction("Description_", "AdminPanel");
        }
        public IActionResult deldes(int id)
        {
            var p = db.Find<Description>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("Description_", "AdminPanel");
        }

        
        public IActionResult header_logo()
        {
            return View(db.header_Logos.ToList());
        }
        public IActionResult header_logo_add() => View();
        public IActionResult header_logo_adding(header_logo_ViewModel header_Logo_ViewModel)
        {
            header_logo p = new header_logo()
            {
                id = header_Logo_ViewModel.id,
                link = header_Logo_ViewModel.link,               
            };
            if (header_Logo_ViewModel.logo != null)
            {
                byte[] b = new byte[header_Logo_ViewModel.logo.Length];
                header_Logo_ViewModel.logo.OpenReadStream().Read(b, 0, b.Length);
                p.logo = b;
            }
            db.Add(p);
            db.SaveChanges();
            return RedirectToAction("header_logo", "AdminPanel");
        }
        public IActionResult del_header_logo(int id)
        {
            var p = db.Find<header_logo>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("header_logo", "AdminPanel");
        }



        public IActionResult VipImage()
        {
            return View(db.vipImages.ToList());
        }
        public IActionResult VipImage_add() => View();
        public IActionResult VipImage_adding(VipImage_ViewModel vipImageViewModel)
        {
            VipImage q = new VipImage()
            {
                id = vipImageViewModel.id,
                alt = vipImageViewModel.alt
            };

            if (vipImageViewModel.image != null)
            {
                byte[] b = new byte[vipImageViewModel.image.Length];
                vipImageViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();

            return RedirectToAction("VipImage", "AdminPanel");
        }
        public IActionResult del_VipImage(int id)
        {
            var p = db.Find<VipImage>(id);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("VipImage", "AdminPanel");
        }

        public IActionResult UipImage()
        {
            return View(db.uipImages.ToList());
        }
        public IActionResult UipImage_add() => View();
        public IActionResult UipImage_adding(UipImage_ViewModel uipImageViewModel)
        {
            UipImage q = new UipImage()
            {
                id = uipImageViewModel.id,
                alt = uipImageViewModel.alt
            };

            if (uipImageViewModel.image != null)
            {
                byte[] b = new byte[uipImageViewModel.image.Length];
                uipImageViewModel.image.OpenReadStream().Read(b, 0, b.Length);
                q.image = b;
            }
            db.Add(q);
            db.SaveChanges();

            return RedirectToAction("UipImage", "AdminPanel");
        }
        public IActionResult del_UipImage(int iD)
        {
            var q = db.Find<UipImage>(iD);
            if (q != null)
            {
                db.Remove(q);
                db.SaveChanges();
            }
            return RedirectToAction("UipImage", "AdminPanel");
        }

        public IActionResult vip_product()
        {
            return View(db.vip_Products.ToList());
        }
        public IActionResult vip_product_add() => View();
        public IActionResult vip_product_adding(vip_product_ViewModel vip_Product_ViewModel)
        {
            vip_product q = new vip_product()
            {
                id = vip_Product_ViewModel.id,
                about = vip_Product_ViewModel.about,
                header = vip_Product_ViewModel.header,
                link = vip_Product_ViewModel.link,
                link_label = vip_Product_ViewModel.link_label,
                offer = vip_Product_ViewModel.offer,
                price = vip_Product_ViewModel.price,
                price_color = vip_Product_ViewModel.price_color,
            };
            db.Add(q);
            db.SaveChanges();

            return RedirectToAction("vip_product", "AdminPanel");
        }
        public IActionResult del_vip_product(int iD)
        {
            var p = db.Find<vip_product>(iD);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("vip_product", "AdminPanel");
        }

        public IActionResult uip_product()
        {
            return View(db.uip_Products.ToList());
        }
        public IActionResult uip_product_add() => View();
        public IActionResult uip_product_adding(uip_product_ViewModel uip_Product_ViewModel)
        {
            uip_product q = new uip_product()
            {
                id = uip_Product_ViewModel.id,
                about = uip_Product_ViewModel.about,
                header = uip_Product_ViewModel.header,
                link = uip_Product_ViewModel.link,
                link_label = uip_Product_ViewModel.link_label,
                offer = uip_Product_ViewModel.offer,
                price = uip_Product_ViewModel.price,
                price_color = uip_Product_ViewModel.price_color,
            };
            db.Add(q);
            db.SaveChanges();

            return RedirectToAction("uip_product", "AdminPanel");
        }
        public IActionResult del_uip_product(int iD)
        {
            var p = db.Find<uip_product>(iD);
            if (p != null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("uip_product", "AdminPanel");
        }


        public IActionResult Menu()
        {
            return View(db.menus.ToList());
        }
        public IActionResult Menu_add() => View();
        public IActionResult Menu_adding(Menu_ViewModel menu_ViewModel)
        {
            Menu q = new Menu()
            {
                id=menu_ViewModel.id,
                link=menu_ViewModel.link,
                name=menu_ViewModel.name
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Menu","AdminPanel");
        }
        public IActionResult Menu_del(int ID)
        {
            var q = db.Find<Menu>(ID);
            if (q!=null)
            {
                db.Remove(q);
                db.SaveChanges();
            }
            return RedirectToAction("Menu", "AdminPanel");
        }
             
        public IActionResult page()
        {
            return View(db.pages.ToList());
        }
        public IActionResult page_add() => View();
        public IActionResult page_adding(Page_ViewModel page_ViewModel)
        {
            Page p = new Page()
            {
                id = page_ViewModel.id,
                content = page_ViewModel.content,
                des = page_ViewModel.des,
                kw = page_ViewModel.kw,
                name = page_ViewModel.name
            };
            db.Add(p);
            db.SaveChanges();
            return RedirectToAction("page", "AdminPanel");                
        }
        public IActionResult page_del(int Id)
        {
            var p = db.Find<Page>(Id);
            if (p!=null)
            {
                db.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("page", "AdminPanel");
        }

        public IActionResult Action_Name_List()
        {
            return View(db.action_Names.ToList());
        }
        public IActionResult Action_Name_Add() => View();
        public IActionResult Action_Name_Adding(Action_Name_ViewModel action_Name_ViewModel)
        {
            Action_Name q = new Action_Name()
            {
                id = action_Name_ViewModel.id,
                name = action_Name_ViewModel.name,
                Controller_NameId = action_Name_ViewModel.Controller_NameId
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Action_Name_List", "AdminPanel");
        }
        public IActionResult Action_Name_Del(int id)
        {
            var q = db.Find<Action_Name>(id);
            if (q!=null)
            {
                db.Remove(q);
                db.SaveChanges();
                
            }
            return RedirectToAction("Action_Name_List", "AdminPanel");
        }


        public IActionResult Controller_Name_List()
        {
            return View(db.controller_Names.ToList());
        }
        public IActionResult Controller_Name_Add() => View();
        public IActionResult Controller_Name_Adding(Controller_Name_ViewModel controller_Name_ViewModel)
        {
            Controller_Name q = new Controller_Name()
            {
                id = controller_Name_ViewModel.id,
                name = controller_Name_ViewModel.name,
                
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Controller_Name_List", "AdminPanel");
        }
        public IActionResult Controller_Name_Del(int id)
        {
            var q = db.Find<Controller_Name>(id);
            if (q != null)
            {
                db.Remove(q);
                db.SaveChanges();

            }
            return RedirectToAction("Controller_Name_List", "AdminPanel");
        }





        public IActionResult Privacy()
        {
            return View(db.p_Ps.ToList());
        }
        public IActionResult privacy_add() => View();
        public IActionResult privacy_adding(P_P_ViewModel privacy_ViewModel)
        {
            P_P q = new P_P()
            {
                id = privacy_ViewModel.id,
                content = privacy_ViewModel.content
            };
            db.Add(q);
            db.SaveChanges();
            return RedirectToAction("Privacy", "AdminPanel");
        }
        public IActionResult del_privacy(int iD)
        {
            var q = db.Find<P_P>(iD);
            if (q != null)
            {
                db.Remove(q);
                db.SaveChanges();
            }
            return RedirectToAction("Privacy", "AdminPanel");
        }





        //        <hr />
        //<h5>قوانین</h5>
        //<div class="container" style="background-color:lightgoldenrodyellow;border: 1px solid white;border-radius:5px">
        //    <p>
        //        الف ) طول توضحات متا در بهترین حالت جهت سئو 160 کاراکتر میباشد.
        //        <br />
        //        ب ) تعداد کلمات کلیدی در بهترین حالت 3 عدد میباشد.
        //        <hr />
        //        1 - طول متن بالا حداکثر 20 کاراکتر میباشد.(بهترین حالت 15 کاراکاتر است)
        //        <br />
        //        2 - طول متن وسط حداکثر 33 کاراکتر میباشد.(بهترین حالت 33 کاراکاتر است)
        //        <br />
        //        3 - طول متن دکمه حداکثر 70 کاراکتر میباشد.(بهترین حالت 16 کاراکاتر است)
        //        <br />
        //        4 - برای تغییر رنگ متون کافی ست نام رنگ را که در زبان نشانه گذاری HTML5 مرسوم میباشد را وارد کنید.
        //        <br />
        //        مثال :
        //        <br />
        //        رنگ سیاه : Black
        //        <br />
        //        رنگ سفید : White
        //        <br />
        //        رنگ زرد : Yellow
        //    </p>
        //</div>

        //}
        //public IActionResult editdes() => View();

        //public IActionResult editingdes(int id, DescriptionViewModel descriptionViewModel)
        //{
        //    Description p = new Description()
        //    {
        //        id = descriptionViewModel.id,
        //        des = descriptionViewModel.des
        //    };
        //    string newp = db.Find<Description>(id).des.Replace();
        //    if (p != null)
        //    {

        //      //  p.des.Replace(p.des,newp.des);
        //        db.Update(p);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Description_", "AdminPanel");
        //}



        //Call Number


        //public IActionResult Call_Number()
        //{
        //    return View(db.call_Numbers.ToList());
        //}
        //public IActionResult addcallnumber() => View();
        //public IActionResult addingNum(Call_NumberViewModel call_NumberViewModel)
        //{
        //    Call_Number p = new Call_Number()
        //    {              
        //        num = call_NumberViewModel.num
        //    };
        //    db.Add(p);
        //    db.SaveChanges();
        //    return RedirectToAction("Call_Number", "AdminPanel");
        //}
        //public IActionResult delNum(int id)
        //{
        //    var p = db.Find<Call_Number>(id);
        //    if (p != null)
        //    {
        //        db.Remove(p);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Call_Number", "AdminPanel");
        //}

        //public IActionResult Test() => View();

        //backlinks
        //public IActionResult Backlinks()
        //{
        //    return View(db.bACKLINKs.ToList());
        //}
        //public IActionResult addbl() => View();
        //public IActionResult addingbl(BACKLINKViewModel backlinkViewModel)
        //{
        //    BACKLINK p = new BACKLINK()
        //    {
        //        id = backlinkViewModel.id,
        //        link = backlinkViewModel.link
        //    };
        //    db.Add(p);
        //    db.SaveChanges();
        //    return RedirectToAction("Backlinks", "AdminPanel");
        //}
        //public IActionResult delbl(int id)
        //{
        //    var p = db.Find<BACKLINK>(id);
        //    if (p != null)
        //    {
        //        db.Remove(p);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Backlinks", "AdminPanel");
        //}
        ////KeyWords
        //public IActionResult Keyword_()
        //{
        //    return View(db.keywords.ToList());
        //}
        //public IActionResult addkeyword() => View();
        //public IActionResult addingkw(KeywordViewModel keywordViewModel)
        //{
        //    Keyword p = new Keyword()
        //    {
        //        id = keywordViewModel.id,
        //        kw = keywordViewModel.kw
        //    };
        //    db.Add(p);
        //    db.SaveChanges();
        //    return RedirectToAction("Keyword_", "AdminPanel");
        //}
        //public IActionResult delkw(int id)
        //{
        //    var p = db.Find<Keyword>(id);
        //    if (p != null)
        //    {
        //        db.Remove(p);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Keyword_", "AdminPanel");
        //}


































    }
}






//public IActionResult EditPage(string title)
//{
//    var page = db.pages.FirstOrDefault(x => x.Title == title);
//    if (page==null)
//    {
//        return View("Error");
//    }
//    db.pages.Add(page);
//    db.SaveChanges();
//    return RedirectToAction("EditPage","AdminPanel");
//}
//public IActionResult SavePage(string title,string content)
// {
//     var page = db.pages.FirstOrDefault(x => x.Title == title);
//     if (page==null)
//     {
//         return View("Error");
//     }
//     page.Content = content;
//     db.SaveChanges();
//     return RedirectToAction("EditPage", "AdminPanel");
// }

//سروریس پیامک
//public async Task<IActionResult> SendSms(int mobile,string msg)
//{
//    try
//    {
//        int smsLineID = 0;// sabte kardid be shoma shomare tel ersale sms dade khahad shod
//        List<SMSirSentAndReceivedMessages.WebServiceSmsSend> sendDetails = new List<SMSirSentAndReceivedMessages.WebServiceSmsSend>();

//        SMSirSentAndReceivedMessages.SendReceiveSoapClient ws = 
//            new SMSirSentAndReceivedMessages.SendReceiveSoapClient(SMSirSentAndReceivedMessages.SendReceiveSoapClient.EndpointConfiguration.SendReceiveSoap);

//        DateTime sendSince = DateTime.Now; //this.dtmSendSince.Value.Date.AddHours(this.tmSendSince.Value.Hour).AddMinutes(this.tmSendSince.Value.Minute).AddSeconds(this.tmSendSince.Value.Second);

//        string message = string.Empty;

//        sendDetails.Add(new WebServiceSmsSend
//        {
//            MessageBody = "سلام ",
//            MobileNo = 9389709920,
//            IsFlash = false
//        });


//        long[] result = await ws.SendMessageAsync("usernaem","pass", sendDetails.ToArray(), smsLineID, sendSince, ref message);

//    }
//    catch (Exception ex)
//    {
//    }

//    return RedirectToAction("");


