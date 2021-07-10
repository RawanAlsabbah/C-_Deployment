using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Alsabbah_Rawan_CsharpExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alsabbah_Rawan_CsharpExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _db;
        private int? uid
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
        }
        private bool isLoggedIn
        {
            get { return uid != null; }
        }

        public HomeController(MyContext context)
        {
            _db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (!isLoggedIn)
            {
                return View();
            }
            return RedirectToAction("Dashboard");
        }
        [HttpPost("singin")]
        public IActionResult Register(User user)
        {

            if (ModelState.IsValid)
            {
                if (_db.Users.Any(u => u.Email == user.Email))
                {

                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _db.Users.Add(user);
                _db.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        [HttpPost("letmein")]
        public IActionResult LetMeIn(LoginUser lu)
        {
            if (ModelState.IsValid)
            {

                User getUser = _db.Users.FirstOrDefault(u => u.Email == lu.LoginEmail);
                if (getUser == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(lu, getUser.Password, lu.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", getUser.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        //// Dashborad
        [HttpGet("home")]
        public IActionResult Dashboard()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index");
            }

            List<Actvity> allActivities = _db
                .Activities
                .Include(m => m.HostedBy)
                .Include(c => c.JoinList)
                .OrderBy(d => d.Date)
                .ToList();

            User u = _db.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View(allActivities);
        }

        /// New Activity 

        [HttpGet("new")]
        public IActionResult NewActivity()
        {

            User u = _db.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View();
        }
        ////   Post Activity
        [HttpPost("postactivity")]
        public IActionResult PostActivity(Actvity activity)
        {
            // check release date if in future
            if (activity.Date < DateTime.Now)
            {
                ModelState.AddModelError("Date", "Date must be in the future");
            }
            // run validation
            if (ModelState.IsValid)
            {

                activity.UserId = (int)uid;
                _db.Activities.Add(activity);
                _db.SaveChanges();
                return Redirect($"/activity/{activity.ActvityId}");

            }
            User u = _db.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View("NewActivity");
        }
        //// Info Actinity
        [HttpGet("activity/{activityId}")]
        public IActionResult Activities(int activityId)
        {

            Actvity thisActivity = _db
            .Activities
            .Include(m => m.HostedBy)
            .Include(m => m.JoinList)
            .ThenInclude(f => f.join)
            .FirstOrDefault(m => m.ActvityId == activityId);
            User u = _db.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View(thisActivity);
        }

        //// Delete Activity 
        [HttpGet("delete/{activityId}")]
        public IActionResult Delete(int activityId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index");
            }
            Actvity delActivity = _db.Activities.FirstOrDefault(m => m.ActvityId == activityId);
            _db.Activities.Remove(delActivity);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        //// Join     
        [HttpGet("Join/{activityId}")]
        public IActionResult Join(int activityId)
        {
            Partspent part = new Partspent();
            part.UserId = (int)uid;
            part.ActvityId = activityId;
            _db.Partspents.Add(part);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        ////  Leave 
        [HttpGet("Leave/{activityId}")]
        public IActionResult Leave(int activityId)
        {

            Partspent leave = _db.Partspents.FirstOrDefault(l => l.jointo.ActvityId == activityId && l.join.UserId == (int)uid);
            _db.Partspents.Remove(leave);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}