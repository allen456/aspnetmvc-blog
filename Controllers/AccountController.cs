using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models.Views;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "system";

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return View();
        }

        [ServiceFilter(typeof(AuthenticationFilter))]
        [ServiceFilter(typeof(LoggingFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.DisplayError = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            LibraryAccount _account = new(_context);
            if (_account.IsLoginCorrect(user.UserName, user.Password))
            {
                var _user = _context.Users.FirstOrDefault(w => w.UserName == user.UserName);
                if (_user == null)
                {
                    return NotFound();
                }
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.Sid, _user.UserId.ToString()),
                };
                ClaimsIdentity userIdentity = new(claims, "login");
                ClaimsPrincipal principal = new(userIdentity);
                await HttpContext.SignInAsync(principal);

                if (_context.Users.Where(w => w.UserId == _user.UserId && w.PasswordChange == true).ToList().Any())
                {
                    return RedirectToAction("Password", "Account");
                }
                var userRoles = _context.UserRoles.Where(a => a.UserId == _user.UserId).OrderBy(w => w.UserRoleOrder).Include(q => q.Role).ToList();
                foreach (UserRole item in userRoles)
                {
                    var _redirectPage = _context.Roles.Where(w => w.RoleId == item.RoleId).ToList().FirstOrDefault();
                    return RedirectToAction(_redirectPage.DefaultAction, _redirectPage.DefaultController);
                }
                return RedirectToAction("Index", "Account");
            }
            ViewData["DisplayError"] = true;
            return View();
        }

        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult Password()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var _user = _context.Users.FirstOrDefault(w => w.UserId == _userSid);
            if (_user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel changepass = new()
            {
                UserId = _user.UserId,
                UserName = _user.UserName,
                OldPassword = _user.Password
            };
            return View(changepass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult Password(ChangePasswordViewModel changepass)
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var _user = _context.Users.FirstOrDefault(w => w.UserId == _userSid);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _user.Password = LibraryStatic.ComputeSha512(changepass.NewPassword);
                _user.UserStatus = true;
                _user.PasswordChange = false;
                _user.PasswordChangeDate = DateTime.Now;
                _context.Users.Update(_user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Logout));
            }
            return View(changepass);
        }

        [HttpGet]
        public JsonResult MenuJSON()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            List<Module> _modules = new();
            if (_userSid == null)
            {
                _userSid = new Guid("00000000-0000-0000-0000-000000000000");
                return Json(_modules);
            }
            LibraryAccount _account = new(_context);
            foreach (Module _module in _context.Modules
                .Where(w => w.ModuleVisibility == true)
                .Select(w => new Module
                {
                    ModuleAction = w.ModuleAction,
                    ModuleController = w.ModuleController,
                    ModuleName = w.ModuleName,
                    ModuleId = w.ModuleId,
                    ModuleOrder = w.ModuleOrder,
                    ModuleVisibility = w.ModuleVisibility,
                    ModuleIconClass = w.ModuleIconClass,
                    ModuleTooltip = w.ModuleTooltip
                }).OrderByDescending(w => w.ModuleOrder))
            {
                if (_account.IsUserAuthorized(_userSid, _module.ModuleAction, _module.ModuleController))
                {
                    _modules.Add(_module);
                }
            }
            return Json(_modules);
        }


        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult LoadUserGrantDataTables()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var draw = HttpContext.Request.Form["draw"].First();
            int recordsTotal = 0;
            IQueryable<UserGrant> searchdata;
            searchdata = (from tempdata in _context.UserGrants.Where(w => w.SourceUser.UserId == _userSid).Include(u => u.SourceUser).Include(u => u.TargetUser).ToList().AsQueryable() select tempdata);
            recordsTotal = searchdata.Count();
            List<UserGrant> data = searchdata.ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult LoadUserGroupDataTables()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var draw = HttpContext.Request.Form["draw"].First();
            int recordsTotal = 0;
            IQueryable<UserGroup> searchdata;
            searchdata = (from tempdata in _context.UserGroups.Where(w => w.UserId == _userSid).Include(u => u.Group).Include(u => u.User).ToList().AsQueryable() select tempdata);
            recordsTotal = searchdata.Count();
            List<UserGroup> data = searchdata.ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult LoadUserModuleDataTables()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var _user = _context.Users.FirstOrDefault(w => w.UserId == _userSid);
            if (_user == null)
            {
                return NotFound();
            }
            var draw = HttpContext.Request.Form["draw"].First();
            int recordsTotal = 0;
            IQueryable<UserModule> searchdata;
            searchdata = (from tempdata in _context.UserModules.Where(w => w.UserId == _userSid).Include(u => u.Module).Include(u => u.User).ToList().AsQueryable() select tempdata);
            recordsTotal = searchdata.Count();
            List<UserModule> data = searchdata.ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public IActionResult LoadUserRoleDataTables()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var _user = _context.Users.FirstOrDefault(w => w.UserId == _userSid);
            if (_user == null)
            {
                return NotFound();
            }
            var draw = HttpContext.Request.Form["draw"].First();
            int recordsTotal = 0;
            IQueryable<UserRole> searchdata;
            searchdata = (from tempdata in _context.UserRoles.Where(w => w.UserId == _userSid).Include(u => u.Role).Include(u => u.User).ToList().AsQueryable() select tempdata);
            recordsTotal = searchdata.Count();
            List<UserRole> data = searchdata.ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        public IActionResult GetInfoJSON()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var _user = _context.Users
                   .Select(q => new User
                   {
                       EmailAddress = q.EmailAddress,
                       UserName = q.UserName,
                       UserStatus = q.UserStatus,
                       UserId = q.UserId,
                       UserGroup = q.UserGroup
                   }).FirstOrDefault(w => w.UserId == _userSid);
            if (_user == null)
            {
                return NotFound();
            }
            return Json(_user);
        }

        [HttpGet]
        public JsonResult GetWebSocketURL()
        {
            return Json($"{this.Request.Host}{this.Request.PathBase}");
        }
    }
}
