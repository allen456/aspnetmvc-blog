using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class RoleController : Controller
    {
        public RoleController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "Role";

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
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,AdministratorFlag,RegisterFlag,DefaultController,DefaultAction")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.RoleId = Guid.NewGuid();
                role.RoleStatus = true;
                role.RoleCreateDate = DateTime.Now;
                _context.Add(role);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            return View(_role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RoleId,RoleName,AdministratorFlag,RegisterFlag,DefaultController,DefaultAction")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    role.RoleStatus = true;
                    _context.Update(role);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Roles.Where(e => e.RoleId == role.RoleId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
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
            var searchdata = (from tempdata in _context.Roles.Where(x => x.RoleStatus == true).Include(ur => ur.UserRole).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(Role).GetProperty(sortColumn);
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
                x.AdministratorFlag.ToString().Contains(searchValue) ||
                x.RoleName.Contains(searchValue) ||
                x.RoleStatus.ToString().Contains(searchValue) ||
                x.RoleId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<Role> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            _role.RoleStatus = false;
            _context.Update(_role);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> Enable(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            _role.RoleStatus = true;
            _context.Update(_role);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _role = _context.Roles.FirstOrDefault(a => a.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }
            return Json(_role);
        }
    }
}
