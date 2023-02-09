using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UserGroupController : Controller
    {
        public UserGroupController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "UserGroup";

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
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus), "GroupId", "GroupDescription");
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == id && w.UserStatus), "UserId", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserGroupId,UserId,GroupId,UserGroupOrder")] UserGroup userGroup)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userGroup.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                userGroup.UserGroupId = Guid.NewGuid();
                _context.Add(userGroup);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index), new { id = userGroup.UserId });
            }
            ViewData["SelectedId"] = userGroup.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus), "GroupId", "GroupDescription", userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == userGroup.UserId && w.UserStatus), "UserId", "UserName", userGroup.UserId);
            return View(userGroup);
        }

        public IActionResult Edit(Guid id, Guid userid)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userid);
            var _userGroup = _context.UserGroups.FirstOrDefault(m => m.UserGroupId == id);
            if (_userGroup == null || _user == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = userid;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus), "GroupId", "GroupDescription", _userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(w => w.UserId == _userGroup.UserId && w.UserStatus), "UserId", "UserName", _userGroup.UserId);
            return View(_userGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserGroupId,UserId,GroupId,UserGroupOrder")] UserGroup userGroup)
        {
            var _user = _context.Users.FirstOrDefault(a => a.UserId == userGroup.UserId);
            if (_user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userGroup);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserGroups.Where(e => e.UserGroupId == userGroup.UserGroupId).ToList().Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index), new { id = userGroup.UserId });
            }
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(w => w.GroupStatus), "GroupId", "GroupCode", userGroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(a => a.UserId == userGroup.UserId && a.UserStatus), "UserId", "UserName", userGroup.UserId);
            return RedirectToAction("Edit", new { id = userGroup.UserGroupId, userid = userGroup.UserId });
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
            IQueryable<UserGroup> searchdata;
            searchdata = (from tempdata in _context.UserGroups.Where(w => w.UserId == id).Include(u => u.Group).Include(u => u.User).OrderByDescending(w => w.UserGroupOrder).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(UserGroup).GetProperty(sortColumn);
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
                x.UserGroupOrder.ToString().Contains(searchValue) ||
                x.UserGroupId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<UserGroup> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var _userGroup = _context.UserGroups.FirstOrDefault(w => w.UserGroupId == id);
            if (_userGroup == null)
            {
                return NotFound();
            }
            _context.Remove(_userGroup);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _userGroup = _context.UserGroups.FirstOrDefault(w => w.UserGroupId == id);
            if (_userGroup == null)
            {
                return NotFound();
            }
            return Json(_userGroup);
        }

        [HttpPost]
        public async Task<IActionResult> AddPriority(Guid id)
        {
            var _userGroup = _context.UserGroups.FirstOrDefault(w => w.UserGroupId == id);
            if (_userGroup == null)
            {
                return NotFound();
            }
            _userGroup.UserGroupOrder++;
            _context.Update(_userGroup);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpPost]
        public async Task<IActionResult> LowerPriority(Guid id)
        {
            var _userGroup = _context.UserGroups.FirstOrDefault(w => w.UserGroupId == id);
            if (_userGroup == null)
            {
                return NotFound();
            }
            if (_userGroup.UserGroupOrder == 0)
            {
                return BadRequest();
            }
            _userGroup.UserGroupOrder--;
            _context.Update(_userGroup);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }
    }
}
