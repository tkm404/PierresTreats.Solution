using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PierresTreats.Controllers
{
  public class RoleController : Controller
  {
    private RoleManager<IdentityRole> roleManager;
    private UserManager<ApplicationUser> userManager;
    public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
    {
      roleManager = roleMgr;
      userManager = userMgr;
    }
    public ViewResult Index() => View(roleManager.Roles);

    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
        ModelState.AddModelError("", error.Description);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([Required] string name)
    {
      if (ModelState.IsValid)
      {
        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          Errors(result);
        }
      }
      return View(name);
    }

    public async Task<IActionResult> Update(string id)
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      List<ApplicationUser> allUsers = await userManager.Users.ToListAsync();

      List<ApplicationUser> members = allUsers
                                      .Where(user => userManager.IsInRoleAsync(user, role.Name).Result)
                                      .ToList();
      List<ApplicationUser> nonMembers = allUsers.Except(members).ToList();
      return View(new RoleEdit
      {
        Role = role,
        Members = members,
        NonMembers = nonMembers
      });

    }


  }
}