using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using Tahlile_Parseh.Areas.Identity.Data;
using Tahlile_Parseh.Data;
using Tahlile_Parseh.Models;
using Tahlile_Parseh.ViewModels;

namespace Tahlile_Parseh.Controllers
{
  
    public class Account : Controller
    {
        //Dependency Injection
     private readonly UserManager<ApplicationUser> userManager;
     private readonly   SignInManager<ApplicationUser> signInManager;
        DBTahlile_Parseh db;

        public Account(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager , DBTahlile_Parseh _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
        }

        
        public IActionResult Index()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            //ViewData["footer_logo"] = db.footer_Logos.ToList();
            ViewData["t_jame"] = db.t_James.ToList();
            ViewData[" t_fanda"] = db.t_Fandas.ToList();
            ViewData[" t_navasan"] = db.t_Navasans.ToList();           
            ViewData[" t_tecnic"] = db.t_Tecnics.ToList();

            
            //ViewData["t_tablokhani"] = db.t_Tablokhanis.ToList();
            //ViewData[" t_hamayesh"] = db.t_Hamayeshes.ToList();
            return View();
        }
        
   

        //LogIn
        //[Route("YS2B7YS2B7")]
        public IActionResult LoginRegistered(string ReturnUrl)
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            ViewData["returnurl"] = ReturnUrl;
            return View();
        }
        public async Task<IActionResult> RegisterConfirm(RegisterViewModel registerViewModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerViewModel.username,
                Email = registerViewModel.username,
                Firstname = registerViewModel.firstname,
                Lastname = registerViewModel.lastname,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (registerViewModel.userpic !=null)
            {
                byte[] b = new byte[registerViewModel.userpic.Length];
                registerViewModel.userpic.OpenReadStream().Read(b, 0, b.Length);
                user.userpic = b;
            }
            var status = await userManager.CreateAsync(user, registerViewModel.password);
            if (status.Succeeded)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید";
            }
            else
            {
                TempData["msg"] = "خطا";
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("LoginRegistered", "Account");
        }
        public async Task<IActionResult> LoginConfirm(LoginViewModel loginviewmodel, string returnurl)
        {
            var user = await userManager.FindByEmailAsync(loginviewmodel.username);
            if (user != null)
            {
                var status = await signInManager.PasswordSignInAsync(user, loginviewmodel.password, true, false);
                if (status.Succeeded)
                {
                    TempData["msg"] = "شما وارد شدید";
                    if (await userManager.IsInRoleAsync(user, "ادمین ها"))
                        return RedirectToAction("index", "adminpanel");
                }
                else if (status.IsNotAllowed)
                {
                    TempData["msg"] = "نام کاربری یا رمز عبور صحیح نمی باشد";
                }

            }
            //return RedirectToAction("LoginRegistered", "Account");
            if (returnurl != null)
                return Redirect(returnurl);
            else
                return RedirectToAction("Index", "Home");

        }
        // LogOut
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //DOREHAYE AMOOZESHI
        //1-sefrtosad
        //[Route("دوره-جامع-بورس")]
        public IActionResult Reg_seftosad()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return   View();
        }
        public IActionResult Reg_seftosad_ON_Con(sefrtosad_ONViewModel sefrtosad_ONViewModel )
        {
            sefrtosad_ON  user = new sefrtosad_ON
            {
                name=sefrtosad_ONViewModel.name,
                lname = sefrtosad_ONViewModel.lname,
                mobile = sefrtosad_ONViewModel.mobile,
                tell = sefrtosad_ONViewModel.tell,
                address = sefrtosad_ONViewModel.address,
                Email = sefrtosad_ONViewModel.Email,
               
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_seftosad", "Account");
        }
        public IActionResult Reg_seftosad_HOZURI_Con(sefrtosad_HOZURIViewModel sefrtosad_HOZURIViewModel)
        {
            sefrtosad_HOZURI user = new sefrtosad_HOZURI
            {
                name = sefrtosad_HOZURIViewModel.name,
                lname = sefrtosad_HOZURIViewModel.lname,
                mobile = sefrtosad_HOZURIViewModel.mobile,
                tell = sefrtosad_HOZURIViewModel.tell,
                address = sefrtosad_HOZURIViewModel.address,
                Email = sefrtosad_HOZURIViewModel.Email,

            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_seftosad", "Account");
        }
        //2-tablokhani
        //[Route("دوره-تابلوخوانی-بورس")]
        public IActionResult Reg_tablokhani()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        public IActionResult Reg_tablokhani_ON_Con(tablokhani_ONViewModel tablokhani_ONViewModel)
        {
            tablokhani_ON user = new tablokhani_ON
            {
                name = tablokhani_ONViewModel.name,
                lname = tablokhani_ONViewModel.lname,
                mobile = tablokhani_ONViewModel.mobile,
                tell = tablokhani_ONViewModel.tell,
                address = tablokhani_ONViewModel.address,
                Email = tablokhani_ONViewModel.Email,
                noa = tablokhani_ONViewModel.noa
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_tablokhani", "Account");
        }
        public IActionResult Reg_tablokhani_HOZURI_Con(tablokhani_HOZURIViewModel tablokhani_HOZURIViewModel)
        {
            tablokhani_HOZURI user = new tablokhani_HOZURI
            {
                name = tablokhani_HOZURIViewModel.name,
                lname = tablokhani_HOZURIViewModel.lname,
                mobile = tablokhani_HOZURIViewModel.mobile,
                tell = tablokhani_HOZURIViewModel.tell,
                address = tablokhani_HOZURIViewModel.address,
                Email = tablokhani_HOZURIViewModel.Email,
                noa=tablokhani_HOZURIViewModel.noa
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_tablokhani", "Account");
        }
        //3-navasangiri
        //[Route("دوره-نوسان-گیری-بورس")]
        public IActionResult Reg_navasangiri()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        public IActionResult Reg_navasangiri_ON_Con(navasangiri_ON navasangiri_ON)
        {
            navasangiri_ON user = new navasangiri_ON
            {
                name = navasangiri_ON.name,
                lname = navasangiri_ON.lname,
                mobile = navasangiri_ON.mobile,
                tell = navasangiri_ON.tell,
                address = navasangiri_ON.address,
                Email = navasangiri_ON.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_navasangiri", "Account");
        }
        public IActionResult Reg_navasangiri_HOZURI_Con(navasangiri_HOZURI navasangiri_HOZURI)
        {
            navasangiri_HOZURI user = new navasangiri_HOZURI
            {
                name = navasangiri_HOZURI.name,
                lname = navasangiri_HOZURI.lname,
                mobile = navasangiri_HOZURI.mobile,
                tell = navasangiri_HOZURI.tell,
                address = navasangiri_HOZURI.address,
                Email = navasangiri_HOZURI.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_navasangiri", "Account");
        }
        //4-tecnical
        //[Route("دوره-تکنیکال-بورس")]
        public IActionResult Reg_tecnical()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        public IActionResult Reg_tecnical_ON_Con(tecnical_ON tecnical_ON)
        {
            tecnical_ON user = new tecnical_ON
            {
                name = tecnical_ON.name,
                lname = tecnical_ON.lname,
                mobile = tecnical_ON.mobile,
                tell = tecnical_ON.tell,
                address = tecnical_ON.address,
                Email = tecnical_ON.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_tecnical", "Account");
        }
        public IActionResult Reg_tecnical_HOZURI_Con(tecnical_HOZURI tecnical_HOZURI)
        {
            tecnical_HOZURI user = new tecnical_HOZURI
            {
                name = tecnical_HOZURI.name,
                lname = tecnical_HOZURI.lname,
                mobile = tecnical_HOZURI.mobile,
                tell = tecnical_HOZURI.tell,
                address = tecnical_HOZURI.address,
                Email = tecnical_HOZURI.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_tecnical", "Account");
        }
        //5-fandamental
       // [Route("دوره-فاندامنتال-بورس")]
        public IActionResult Reg_fandamental()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        public IActionResult Reg_fandamental_ON(fandamental_ON fandamental_ON)
        {
            fandamental_ON user = new fandamental_ON
            {
                name = fandamental_ON.name,
                lname = fandamental_ON.lname,
                mobile = fandamental_ON.mobile,
                tell = fandamental_ON.tell,
                address = fandamental_ON.address,
                Email = fandamental_ON.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_fandamental", "Account");
        }
        public IActionResult Reg_fandamental_HOZURI(fandamental_HOZURI fandamental_HOZURI)
        {
            fandamental_HOZURI user = new fandamental_HOZURI
            {
                name = fandamental_HOZURI.name,
                lname = fandamental_HOZURI.lname,
                mobile = fandamental_HOZURI.mobile,
                tell = fandamental_HOZURI.tell,
                address = fandamental_HOZURI.address,
                Email = fandamental_HOZURI.Email,
            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_fandamental", "Account");
        }
        //6-hamayesh
        public IActionResult Reg_hamayesh()
        {
            ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        public IActionResult Reg_hamayesh_Con(hamayeshViewModel hamayeshViewModel)
        {
            hamayesh user = new hamayesh
            {
                name = hamayeshViewModel.name,
                lname = hamayeshViewModel.lname,
                mobile = hamayeshViewModel.mobile,
                tell = hamayeshViewModel.tell,
                address = hamayeshViewModel.address,
                Email = hamayeshViewModel.Email,

            };
            db.Add(user);
            db.SaveChanges();
            var status = user;
            if (status != null)
            {
                TempData["msg"] = "شما با موفقیت ثبت نام کردید!";
            }
            else if (status == null)
            {
                TempData["msg"] = "لطفا تمامی فیلدها را پر کنید!!";
            }

            return RedirectToAction("Reg_hamayesh", "Account");
        }
       



    }
}




