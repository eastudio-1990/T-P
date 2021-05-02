using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tahlile_Parseh.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            //ViewData["header_top_info"] = db.header_Top_Infos.ToList();
            return View();
        }
        //404 Not Found
        //When a user tries to access a web page that doesn’t exist, they will get a 404 error.This is usually the result of a broken link, a web page that has been moved, the user mistyped the URL, or the page was simply deleted.
        public IActionResult E404() => View();
        //500 Internal Server Error
        //This is the most common error that web users will see.It is a general-purpose error, and can occur any time a web server encounters an internal problem.Error 500 happens most often when a web server is overloaded.
        public IActionResult E500() => View();
        //401 Unauthorized
        //Web users will likely stumble across this error after a failed login attempt. Basically, it means the user tried to access a site they didn’t have access to.
        public IActionResult E401() => View();
        //        400 Bad Request
        //This error message will appear when something has gone wrong with your web browser.It means that your request was corrupted in some way.
        public IActionResult E400() => View();
        //        403 Forbidden
        //When there is no login opportunity on a page, you will get a 403 error on a page if you try to access a forbidden directory on a website.
        public IActionResult E403() => View();
        // 408 Request Timeout
        //A 408 error occurs when the user stops the request before the server finished retrieving information.This error will appear when a user closes the browser, clicks on a link too soon, or hits the stop button.It is also common to see this error when a server is running slow, or a file is very large.
        public IActionResult E408() => View();
        //        501 Not Implemented
        //When this error appears, it means the user has requested a feature that the browser does not support.
        public IActionResult E501() => View();
        //        502 Service Temporarily Overloaded
        //A 502 error occurs when there is server congestion.Usually this error corrects itself, when web traffic decreases.
        public IActionResult E502() => View();
        //        503 Service Unavailable
        //If the site is busy, or the server is down, users may get a 503 error.
        public IActionResult E503() => View();
        //        Connection Refused by Host
        //This error is very similar to the 403 error.It means the user either doesn’t have permission to access the site, or an entered password is not correct.
        public IActionResult E403_2() => View();
        //        File Contains No Data
        //When a page is there, but nothing shows up, users can see a file contains no data error.This error is probably caused by stripped header information or bad table formatting.
        public IActionResult E_File_No_Data() => View();
        //        Cannot Add Form Submission Result to Bookmark List
        //Only a document or a web address can be saved as a bookmark.If a user tries to save any other type of form, they will get this error.
        public IActionResult E_Submission() => View();
        //        Helper Application Not Found
        //If a user tries to download a file that requires the use of a helper program, this particular error may appear, if the browser cannot find the required program.
        public IActionResult E_Helper_Not_Found() => View();
        //        TCP Error Encountered While Sending Request to Server
        //When this error occurs something has gone wrong on the line between the requested site and the user. Sometimes this is hardware related, so all instances of this error should be reported to a network administrator.
        public IActionResult E_TCP() => View();
        //        Failed DNS Look-Up
        //A failed DNS look-up error means the web site’s URL could not be translated.Due to overload, this error is most common on commercial sites.The best thing to do when this occurs is to try again later.
        public IActionResult E_Failed_DNS() => View();
    }
}
