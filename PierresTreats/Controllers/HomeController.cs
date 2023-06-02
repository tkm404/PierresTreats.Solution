using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using PierresTreats.Models;

namespace PierresTreats.Controllers
{
  public class HomeController : Controller
{
  private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

  public HomeController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  // [HttpGet("/")]
  // public async Task<ActionResult> Index()
  // {
  //   Flavor[] flav = _db.Flavors.ToArray();
  //   Dictionary<string, object[]> model = new Dictionary<string, object[]>();
  //   model.Add("categories", cats);
  //   string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
  //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
  //   if (currentUser != null)
  //   {
  //     Item[] items = _db.Items
  //                       .Where(entry => entry.User.Id == currentUser.Id)
  //                       .ToArray();
  //     model.Add("items", items);
  //   }
  //   return View(model);
  // }

  [HttpGet("/")]
  public ActionResult Index()
  {
    Flavor[] flavs = _db.Flavors.ToArray();
    Treat[] trts = _db.Treats.ToArray();
    Dictionary<string, object[]> model = new Dictionary<string, object[]>();
    model.Add("Flavors", flavs);
    model.Add("Treats", trts);

    return View(model);
  }
}
}