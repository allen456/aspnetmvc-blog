using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class ModuleController : Controller
    {
        public ModuleController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "Module";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,ModuleName,ModuleController,ModuleAction,ModuleVisibility,ModuleOrder,ModuleIconClass,ModuleTooltip")] Module _module)
        {
            if (ModelState.IsValid)
            {
                _module.ModuleId = Guid.NewGuid();
                _module.ModuleCreateDate = DateTime.Now;
                _context.Add(_module);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index));
            }
            return View(_module);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var _module = await _context.Modules.SingleOrDefaultAsync(m => m.ModuleId == id);
            if (_module == null)
            {
                return NotFound();
            }
            return View(_module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ModuleId,ModuleName,ModuleController,ModuleAction,ModuleVisibility,ModuleOrder,ModuleIconClass,ModuleTooltip")] Module _module)
        {
            if (id != _module.ModuleId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_module);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Modules.Where(e => e.ModuleId == _module.ModuleId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_module);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult LoadDataTables()
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
            var searchdata = (from tempdata in _context.Modules.OrderByDescending(w => w.ModuleOrder).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(Module).GetProperty(sortColumn);
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
                x.ModuleAction.Contains(searchValue) ||
                x.ModuleController.Contains(searchValue) ||
                x.ModuleName.Contains(searchValue) ||
                x.ModuleVisibility.ToString().Contains(searchValue) ||
                x.ModuleId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<Module> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var _module = _context.Modules.FirstOrDefault(a => a.ModuleId == id);
            if (_module == null)
            {
                return NotFound();
            }
            foreach (RoleModule deletusfetus in _context.RoleModules.Where(w => w.ModuleId == id).ToList())
            {
                _context.Remove(deletusfetus);
            }
            foreach (UserModule abortiones in _context.UserModules.Where(w => w.ModuleId == id).ToList())
            {
                _context.Remove(abortiones);
            }
            _context.Remove(_module);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }


        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _module = _context.Modules.FirstOrDefault(a => a.ModuleId == id);
            if (_module == null)
            {
                return NotFound();
            }
            _module.ModuleVisibility = false;
            _context.Update(_module);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> Enable(Guid id)
        {
            var _module = _context.Modules.FirstOrDefault(a => a.ModuleId == id);
            if (_module == null)
            {
                return NotFound();
            }
            _module.ModuleVisibility = true;
            _context.Update(_module);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _module = _context.Modules.FirstOrDefault(a => a.ModuleId == id);
            if (_module == null)
            {
                return NotFound();
            }
            return Json(_module);
        }
         
        [HttpPost]
        public async Task<IActionResult> Clean()
        {
            int counter = 0;
            foreach (Module updated in _context.Modules.Where(w => w.ModuleVisibility == true).ToList())
            {
                counter++;
                updated.ModuleVisibility = false;
                _context.Update(updated);
            }
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = counter });
        }
          
        [HttpPost]
        public async Task<IActionResult> Dedupe()
        {
            int counter = 0;
            foreach (Module updated in _context.Modules)
            {
                if (1 < _context.Modules.Count(w => w.ModuleController == updated.ModuleController && w.ModuleAction == updated.ModuleAction))
                {
                    counter += 1;
                    _context.RemoveRange(_context.UserModules.Where(w => w.ModuleId == updated.ModuleId).ToList());
                    _context.RemoveRange(_context.RoleModules.Where(w => w.ModuleId == updated.ModuleId).ToList());
                    _context.Remove(updated);
                    await _context.SaveChangesAsync();
                }
            }
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = counter });
        }
    }
}
