using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PierresTreats.Controllers
{
  public class RolesController : Controller
  {
    private RoleManager<IdentityRole> roleManager;
    private UserManager<ApplicationUser> userManager;
    public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
    {
      roleManager = roleMgr;
      userManager = userMgr;
    }

    // [Authorize(Roles = "Administrator")]
    public ActionResult Index()
    {
      try
      {
        return View(roleManager.Roles);
      }
      catch
      {
        return View("Accounts", "AccessDenied");
      }
    }

    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
        ModelState.AddModelError("", error.Description);
    }

    // [Authorize(Roles = "Administrator")]
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

    // [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      else
        ModelState.AddModelError("", "No role found");
      return View("Index", roleManager.Roles);
    }

    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(string id)
    {
      try
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
      catch
      {
        return View("Accounts", "AccessDenied");
      }


    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleModification model)
    {
      IdentityResult result;
      if (ModelState.IsValid)
      {
        foreach (string userId in model.AddIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
              Errors(result);
          }
        }
        foreach (string userId in model.DeleteIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
              Errors(result);
          }
        }
      }
      if (ModelState.IsValid)
        return RedirectToAction(nameof(Index));
      else
        return await Update(model.RoleName);
      //^^^^ Tutorial says RoleId, but that doesn't work. Not super sure why this doesn't break.
    }
  }
}