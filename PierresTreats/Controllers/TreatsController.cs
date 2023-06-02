using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using PierresTreats.Models;

namespace PierresTreats.Controllers
{
  [Authorize(Roles = "Administrator, Customer")]
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Treats.ToList());
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                              .Include(treat => treat.JoinEntities)
                              .ThenInclude(join => join.Treat)
                              .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

// READ functions ^^^^
//------------------------------------------------------------------
// CREATE functions vvvv
    [Authorize(Roles = "Administrator")]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      if (!ModelState.IsValid)
      {
          return View();
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        treat.User = currentUser;
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [Authorize(Roles = "Administrator")]
    public ActionResult AddTreat(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddTreat(Treat treat, int flavorId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if (joinEntity == null && flavorId !=0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = flavorId, TreatId = treat.TreatId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

// CREATE functions ^^^^
//------------------------------------------------------------------
// UPDATE functions vvvv

    [Authorize(Roles = "Administrator")]
    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// UPDATE functions ^^^^
//------------------------------------------------------------------
// DELETE functions vvvv


    [Authorize(Roles = "Administrator")]
    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(e => e.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // DELETE functions ^^^^
  }
}