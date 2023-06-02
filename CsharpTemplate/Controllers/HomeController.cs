using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using [ProjectNameWithoutBracks].Models;

namespace [ProjectNameWithoutBracks].Controllers
{
  public class HomeController : Controller
{
  private readonly [ProjectNameWithoutBracks] Context _db;
    private readonly UserManager<ApplicationUser> _userManager;

  public HomeController(UserManager<ApplicationUser> userManager, [ProjectNameWithoutBracks] Context db)
  {
    _userManager = userManager;
    _db = db;
  }

  [HttpGet("/")]
  public async Task<ActionResult> Index()
  {
    Category[] cats = _db.Categories.ToArray();
    Dictionary<string, object[]> model = new Dictionary<string, object[]>();
    model.Add("categories", cats);
    string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    if (currentUser != null)
    {
      Item[] items = _db.Items
                        .Where(entry => entry.User.Id == currentUser.Id)
                        .ToArray();
      model.Add("items", items);
    }
    return View(model);
  }
}
}