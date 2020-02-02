using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Contexts;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    [Route("wedding")]
    public class WeddingController : Controller
    {
        private HomeContext dbContext;

        public WeddingController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewBag.User = userInDb;
            List<Wedding> AllWeddings = dbContext.Weddings.Include(w => w.GuestList).ThenInclude(r => r.Guest).Include(w => w.Planner).ToList();
            return View(AllWeddings);
        }

        [HttpGet("new/wedding")]
        public IActionResult NewWedding()
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewBag.User = userInDb;
            return View();
        }

        [HttpPost("process")]
        public IActionResult Process(Wedding newWed)
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            if(ModelState.IsValid)
            {
                if(userInDb.PlannedWeddings.Any(w => w.Date > newWed.Date && w.EndTime < newWed.EndTime))
                {
                    ModelState.AddModelError("Date", "You are already booked at this time.");
                    ViewBag.User = userInDb;
                    return View("NewWedding");
                }
                else
                {
                
                dbContext.Weddings.Add(newWed);
                dbContext.SaveChanges();
                return Redirect($"/wedding/{newWed.WeddingId}");
                }
            }
            else
            {
                ViewBag.User = userInDb;
                return View("NewWedding");
            }
        }


        [HttpGet("{weddingId}")]
        public IActionResult ShowWedding(int weddingId)
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            else
            {
                Wedding show = dbContext.Weddings.Include(w => w.GuestList).ThenInclude(r => r.Guest).Include(w => w.Planner).FirstOrDefault(w => w.WeddingId == weddingId);
                ViewBag.User = userInDb;
                return View(show);
            }

        }

        [HttpGet("delete/{weddingId}")]

        public IActionResult DeleteWedding(int weddingId)
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            Wedding removeWed = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            dbContext.Weddings.Remove(removeWed);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");

        }

        [HttpGet("response/{weddingId}/{userId}/{status}")]

        public IActionResult Rsvp(int weddingId, int userId, string status)
        {
            User userInDb = UserWtihWeds();
            if(userInDb == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            if(status == "Rsvp")
            {
                Rsvp going = new Rsvp();
                going.UserId = userId;
                going.WeddingId = weddingId;

                
                // List<Wedding> RsvpedWeddings = dbContext.Weddings.Include(w => w.GuestList).ThenInclude(r => r.Attending).Include(w => w.UserId == userId).Where(u => u.UserId == userId).ToList();

                // if(RsvpedWeddings.Any(w => w.Date == going.Attending.Date))
                // {
                //     ModelState.AddModelError("Rsvp", "You are already booked at this time.");
                //     return RedirectToAction("Dashboard");
                // }


                dbContext.Rsvps.Add(going);
                dbContext.SaveChanges();
            }
            else if(status == "Un-Rsvp")
            {
                Rsvp notGoing = dbContext.Rsvps.FirstOrDefault(r => r.WeddingId == weddingId && r.UserId == userId);
                dbContext.Rsvps.Remove(notGoing);
                dbContext.SaveChanges();
            }
            else
            {
                return RedirectToAction("Logout", "Home");
            }
            return RedirectToAction("Dashboard");

        }

        private User UserWtihWeds()
        {
            User LogIn =  dbContext.Users.Include(u => u.PlannedWeddings).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            
            return LogIn;
        }




    }
}