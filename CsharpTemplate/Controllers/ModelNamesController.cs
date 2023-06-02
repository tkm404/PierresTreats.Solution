using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using [ProjectNameWithoutBracks].Models;

namespace [ProjectNameWithoutBracks].Controllers
{
  [Authorize]
  public class ModelsController : Controller
  {
    private readonly [ProjectNameWithoutBracks]Context _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public ModelsController(UserManager<ApplicationUser> userManager, [ProjectNameWithoutBracks]Context db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<ModelName> model = _db.ModelNames
                            .Where(entry => entry.User.Id == currentUser.Id)
                            .Include(model => model.Property)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(ModelName modelname, int CategoryId)
    {
      if (!ModelState.IsValid)
      {
          ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
          return View(modelname);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        modelname.User = currentUser;
        _db.ModelNames.Add(modelname);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
  }
}