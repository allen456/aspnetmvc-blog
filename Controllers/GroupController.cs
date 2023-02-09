using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
 
namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class GroupController : Controller
    {
        public GroupController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "Group";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus == true).ToList(), "GroupId", "GroupDescription");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupCode,GroupDescription")] Group group)
        {
            if (ModelState.IsValid)
            {
                group.GroupId = Guid.NewGuid();
                group.GroupCreateDate = DateTime.Now;
                group.GroupStatus = true;
                _context.Add(group);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus == true).ToList(), "GroupId", "GroupDescription");
            return View(group);
        }

        public IActionResult Edit(Guid id)
        {
            var _data = _context.Groups.Where(m => m.GroupId == id).ToList().FirstOrDefault();
            if (_data == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus == true && w.GroupId != id).ToList(), "GroupId", "GroupDescription");
            return View(_data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GroupId,GroupCode,GroupDescription")] Group group)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    group.GroupStatus = true;
                    _context.Update(group);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Groups.Where(e => e.GroupId == group.GroupId).ToList().Any())
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
            ViewData["ParentId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus == true && w.GroupId != id).ToList(), "GroupId", "GroupDescription");
            return View(group);
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
            var searchdata = (from tempdata in _context.Groups.Where(x => x.GroupStatus == true).Include(w => w.UserGroup).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(Group).GetProperty(sortColumn);
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
                x.GroupCode.Contains(searchValue) ||
                x.GroupDescription.Contains(searchValue) ||
                x.GroupId.ToString().Contains(searchValue) ||
                x.GroupStatus.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<Group> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _group = _context.Groups.FirstOrDefault(a => a.GroupId == id);
            if (_group == null)
            {
                return NotFound();
            }
            _group.GroupStatus = false;
            _context.Update(_group);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _group = _context.Groups.FirstOrDefault(a => a.GroupId == id);
            if (_group == null)
            {
                return NotFound();
            }
            return Json(_group);
        }
    }
}
