using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UserRoleController : Controller
    {
        public UserRoleController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "UserRole";

        public IActionResult Index(Guid id)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == id);
            if (_user == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = id;
            ViewData["SelectedName"] = _user.UserName;
            return View();
        }

        public IActionResult Create(Guid id)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == id);
            if (_user == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = id;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(w => w.RoleStatus), "RoleId", "RoleName");
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == id && w.UserStatus), "UserId", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserRoleId,UserId,RoleId,UserRoleOrder")] UserRole userRole)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userRole.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                userRole.UserRoleId = Guid.NewGuid();
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index), new { id = userRole.UserId });
            }
            ViewData["SelectedId"] = userRole.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(w => w.RoleStatus), "RoleId", "RoleName", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == userRole.UserId && w.UserStatus), "UserId", "UserName", userRole.UserId);
            return View(userRole);
        }

        public IActionResult Edit(Guid id, Guid userid)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userid);
            var _userRole = _context.UserRoles.FirstOrDefault(m => m.UserRoleId == id);
            if (_userRole == null || _user == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = userid;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(w => w.RoleStatus), "RoleId", "RoleName", _userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == _userRole.UserId && w.UserStatus), "UserId", "UserName", _userRole.UserId);
            return View(_userRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserRoleId,UserId,RoleId,UserRoleOrder")] UserRole userRole)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userRole.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                    _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserRoles.Where(e => e.UserRoleId == userRole.UserRoleId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index), new { id = userRole.UserId });
            }
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(w => w.RoleStatus), "RoleId", "RoleName", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == userRole.UserId && w.UserStatus), "UserId", "UserName", userRole.UserId);
            return RedirectToAction("Edit", new { id = userRole.UserRoleId, userid = userRole.UserId });
        }

        [HttpPost]
        public IActionResult LoadDataTables(Guid id)
        {
            var draw = HttpContext.Request.Form["draw"].First();
            var start = Request.Form["start"].First();
            var length = Request.Form["length"].First();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].First() + "][name]"].First();
            var sortColumnDirection = Request.Form["order[0][dir]"].First();
            var searchValue = Request.Form["search[value]"].First();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            IQueryable<UserRole> searchdata;
            searchdata = (from tempdata in _context.UserRoles.Where(w => w.UserId == id).Include(u => u.Role).Include(u => u.User).OrderByDescending(w => w.UserRoleOrder).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(UserRole).GetProperty(sortColumn);
                if (sortColumnDirection == "desc")
                {
                    searchdata = searchdata.OrderByDescending(n => (convertProperty.GetValue(n) ?? string.Empty).ToString());
                }
                if (sortColumnDirection == "asc")
                {
                    searchdata = searchdata.OrderBy(n => (convertProperty.GetValue(n) ?? string.Empty).ToString());
                }
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchdata = searchdata.Where(x =>
                x.UserRoleOrder.ToString().Contains(searchValue) ||
                x.UserRoleId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<UserRole> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var _userRole = _context.UserRoles.FirstOrDefault(w => w.UserRoleId == id);
            if (_userRole == null)
            {
                return NotFound();
            }
            _context.Remove(_userRole);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _userRole = _context.UserRoles.FirstOrDefault(w => w.UserRoleId == id);
            if (_userRole == null)
            {
                return NotFound();
            }
            return Json(_userRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddPriority(Guid id)
        {
            var _userRole = _context.UserRoles.FirstOrDefault(w => w.UserRoleId == id);
            if (_userRole == null)
            {
                return NotFound();
            }
            _userRole.UserRoleOrder++;
            _context.Update(_userRole);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> LowerPriority(Guid id)
        {
            var _userRole = _context.UserRoles.FirstOrDefault(w => w.UserRoleId == id);
            if (_userRole == null)
            {
                return NotFound();
            }
            if (_userRole.UserRoleOrder == 0)
            {
                return BadRequest();
            }
            _userRole.UserRoleOrder--;
            _context.Update(_userRole);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }
    }
}
