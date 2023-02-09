using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class RoleModuleController : Controller
    {
        public RoleModuleController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "RoleModule";

        public IActionResult Index(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = id;
            ViewData["SelectedName"] = _role.RoleName;
            return View();
        }

        public IActionResult Create(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = id;
            ViewData["SelectedName"] = _role.RoleName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(a => a.RoleId == id && a.RoleStatus), "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleModuleId,RoleId,ModuleId,RoleModuleOrder,RoleModuleAccess")] RoleModule roleModule)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == roleModule.RoleId);
            if (_role == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                roleModule.RoleModuleId = Guid.NewGuid();
                _context.Add(roleModule);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index), new { id = roleModule.RoleId });
            }
            ViewData["SelectedId"] = _role.RoleId;
            ViewData["SelectedName"] = _role.RoleName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(a => a.RoleId == _role.RoleId && a.RoleStatus), "RoleId", "RoleName", roleModule.RoleId);
            return View(roleModule);
        }

        public IActionResult Edit(Guid id, Guid roleid)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == roleid);
            var _roleModule = _context.RoleModules.FirstOrDefault(m => m.RoleModuleId == id);
            if (_roleModule == null || _role == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = _role.RoleId;
            ViewData["SelectedName"] = _role.RoleName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", _roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(a => a.RoleId == _role.RoleId && a.RoleStatus), "RoleId", "RoleName", _roleModule.RoleId);
            return View(_roleModule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RoleModuleId,RoleId,ModuleId,RoleModuleOrder,RoleModuleAccess")] RoleModule roleModule)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == roleModule.RoleId);
            if (_role == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roleModule);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RoleModules.Where(e => e.RoleModuleId == roleModule.RoleModuleId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index), new { id = roleModule.RoleId });
            }

            ViewData["SelectedId"] = _role.RoleId;
            ViewData["SelectedName"] = _role.RoleName;
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleAction", roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(a => a.RoleId == roleModule.RoleId && a.RoleStatus), "RoleId", "RoleName", roleModule.RoleId);
            return RedirectToAction("Edit", new { id = roleModule.RoleModuleId, roleid = roleModule.RoleId });
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
            IQueryable<RoleModule> searchdata;
            searchdata = (from tempdata in _context.RoleModules.Where(w => w.RoleId == id).Include(r => r.Module).Include(r => r.Role).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(RoleModule).GetProperty(sortColumn);
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
                x.RoleModuleOrder.ToString().Contains(searchValue) ||
                x.RoleModuleAccess.ToString().Contains(searchValue) ||
                x.RoleModuleId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<RoleModule> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var _roleModule = _context.RoleModules.FirstOrDefault(w => w.RoleModuleId == id);
            if (_roleModule == null)
            {
                return NotFound();
            }
            _context.Remove(_roleModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _roleModule = _context.RoleModules.FirstOrDefault(w => w.RoleModuleId == id);
            if (_roleModule == null)
            {
                return NotFound();
            }
            return Json(_roleModule);
        }

        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _roleModule = _context.RoleModules.FirstOrDefault(w => w.RoleModuleId == id);
            if (_roleModule == null)
            {
                return NotFound();
            }
            _roleModule.RoleModuleAccess = false;
            _context.Update(_roleModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> Enable(Guid id)
        {
            var _roleModule = _context.RoleModules.FirstOrDefault(w => w.RoleModuleId == id);
            if (_roleModule == null)
            {
                return NotFound();
            }
            _roleModule.RoleModuleAccess = true;
            _context.Update(_roleModule);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> Populate(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            int count = 0;
            foreach (Module item in _context.Modules.ToList())
            {
                Guid _ModuleId = item.ModuleId;
                if (!_context.RoleModules.Where(w => w.ModuleId == _ModuleId && w.RoleId == id).Any())
                {
                    RoleModule newRoleDetail = new RoleModule
                    {
                        RoleModuleId = Guid.NewGuid(),
                        ModuleId = _ModuleId,
                        RoleId = id,
                        RoleModuleAccess = false,
                        RoleModuleOrder = 0
                    };
                    count++;
                    _context.RoleModules.Attach(newRoleDetail);
                    _context.Entry(newRoleDetail).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
            }
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = count });
        }
    }
}
