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