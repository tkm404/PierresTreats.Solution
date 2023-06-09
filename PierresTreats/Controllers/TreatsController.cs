using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using PierresTreats.Models;

namespace PierresTreats.Controllers
{
  [Authorize]
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
                              .ThenInclude(join => join.Flavor)
                              .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    // READ functions ^^^^
    //------------------------------------------------------------------
    // CREATE functions vvvv
    [Authorize(Roles = "Administrator")]
    public ActionResult Create()
    {
      try
      {
        return View();
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }

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
    public ActionResult AddFlavor(int id)
    {

      try
      {
        Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Kind");
        return View(thisTreat);
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }

    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
#nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
#nullable disable
      if (joinEntity == null && flavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = flavorId, TreatId = treat.TreatId });
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
      try
      {
        Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
        return View(thisTreat);
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }

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
      try
      {
        Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
        return View(thisTreat);
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }

    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      try
      {
        TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(e => e.TreatFlavorId == joinId);
        _db.TreatFlavors.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }

    }

    // DELETE functions ^^^^
  }
}