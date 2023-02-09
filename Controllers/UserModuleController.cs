using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UserModuleController : Controller
    {
        public UserModuleController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "UserModule";

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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == id && w.UserStatus), "UserId", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserModuleId,UserId,ModuleId,UserModuleAccess,UserModuleOrder")] UserModule userModule)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userModule.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                userModule.UserModuleId = Guid.NewGuid();
                _context.Add(userModule);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index), new { id = userModule.UserId });
            }
            ViewData["SelectedId"] = userModule.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", userModule.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == userModule.UserId && w.UserStatus), "UserId", "FirstName", userModule.UserId);
            return View(userModule);
        }

        public IActionResult Edit(Guid id, Guid userid)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userid);
            var _userModule = _context.UserModules.FirstOrDefault(m => m.UserModuleId == id);
            if (_userModule == null || _user == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = userid;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", _userModule.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == _userModule.UserId && w.UserStatus), "UserId", "UserName", _userModule.UserId);
            return View(_userModule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserModuleId,UserId,ModuleId,UserModuleAccess,UserModuleOrder")] UserModule userModule)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userModule.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModule);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserModules.Where(e => e.UserModuleId == userModule.UserModuleId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index), new { id = userModule.UserId });
            }
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", userModule.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == userModule.UserId && w.UserStatus), "UserId", "FirstName", userModule.UserId);
            return RedirectToAction("Edit", new { id = userModule.UserModuleId, userid = userModule.UserId });
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
            IQueryable<UserModule> searchdata;
            searchdata = (from tempdata in _context.UserModules.Where(w => w.UserId == id).Include(u => u.Module).Include(u => u.User).OrderByDescending(w => w.UserModuleOrder).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(UserModule).GetProperty(sortColumn);
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
                x.UserModuleOrder.ToString().Contains(searchValue) ||
                x.UserModuleAccess.ToString().Contains(searchValue) ||
                x.UserModuleId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<UserModule> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            _context.Remove(_userModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            return Json(_userModule);
        }

        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            _userModule.UserModuleAccess = false;
            _context.Update(_userModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> Enable(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            _userModule.UserModuleAccess = true;
            _context.Update(_userModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPriority(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            _userModule.UserModuleOrder++;
            _context.Update(_userModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> LowerPriority(Guid id)
        {
            var _userModule = _context.UserModules.FirstOrDefault(w => w.UserModuleId == id);
            if (_userModule == null)
            {
                return NotFound();
            }
            if (_userModule.UserModuleOrder == 0)
            {
                return BadRequest();
            }
            _userModule.UserModuleOrder--;
            _context.Update(_userModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }
    }
}
