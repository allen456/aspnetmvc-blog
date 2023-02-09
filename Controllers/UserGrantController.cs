using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
 
namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UserGrantController : Controller
    {
        public UserGrantController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "UserGrant";

        public IActionResult Index()
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
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            return View();
        }


        public IActionResult Create()
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
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["TargetId"] = new SelectList(_context.Users.Where(w => w.UserStatus && w.UserId != _user.UserId), "UserId", "UserName");
            ViewData["SourceId"] = new SelectList(_context.Users.Where(w => w.UserId == _user.UserId), "UserId", "UserName", _user.UserId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserGrantId,SourceId,TargetId")] UserGrant UserGrant)
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
                UserGrant.UserGrantId = Guid.NewGuid();
                UserGrant.UserGrantDate = DateTime.Now; 
                _context.Add(UserGrant);
                await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["TargetId"] = new SelectList(_context.Users.Where(w => w.UserStatus && w.UserId != _user.UserId), "UserId", "UserName", UserGrant.TargetId);
            ViewData["SourceId"] = new SelectList(_context.Users.Where(w => w.UserId == _user.UserId), "UserId", "UserName", _user.UserId);
            return View(UserGrant);
        }


        public async Task<IActionResult> Edit(Guid id)
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
            var UserGrant = await _context.UserGrants.SingleOrDefaultAsync(m => m.UserGrantId == id);
            if (UserGrant == null)
            {
                return NotFound();
            }
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["TargetId"] = new SelectList(_context.Users.Where(w => w.UserStatus && w.UserId != _user.UserId), "UserId", "UserName");
            ViewData["SourceId"] = new SelectList(_context.Users.Where(w => w.UserId == _user.UserId), "UserId", "UserName", _user.UserId);

            return View(UserGrant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserGrantId,SourceId,TargetId")] UserGrant UserGrant)
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
                try
                {
                    UserGrant.UserGrantDate = DateTime.Now;
                    _context.Update(UserGrant);
                    await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserGrants.Where(e => e.UserGrantId == UserGrant.UserGrantId).ToList().Any())
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
            ViewData["SelectedId"] = _user.UserId;
            ViewData["SelectedName"] = _user.UserName;
            ViewData["TargetId"] = new SelectList(_context.Users.Where(w => w.UserStatus && w.UserId != _user.UserId), "UserId", "UserName", UserGrant.TargetId);
            ViewData["SourceId"] = new SelectList(_context.Users.Where(w => w.UserId == _user.UserId), "UserId", "UserName", _user.UserId);
            return View(UserGrant);
        }

        [HttpPost]
        public IActionResult LoadDataTables()
        {
            var _userSid = LibraryStatic.GetIdentitySid(User);
            if (_userSid == null)
            {
                return Unauthorized();
            }
            var draw = HttpContext.Request.Form["draw"].First();
            var start = Request.Form["start"].First();
            var length = Request.Form["length"].First();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].First() + "][name]"].First();
            var sortColumnDirection = Request.Form["order[0][dir]"].First();
            var searchValue = Request.Form["search[value]"].First();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            IQueryable<UserGrant> searchdata;
            searchdata = (from tempdata in _context.UserGrants.Where(w => w.SourceUser.UserId == _userSid).Include(u => u.SourceUser).Include(u => u.TargetUser).ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(UserGrant).GetProperty(sortColumn);
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
                x.UserGrantId.ToString().Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<UserGrant> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid? id)
        {
            var _userGrant = _context.UserGrants.FirstOrDefault(w => w.UserGrantId == id);
            if (_userGrant == null)
            {
                return NotFound();
            }
            _context.Remove(_userGrant);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _userGrant = _context.UserGrants.FirstOrDefault(w => w.UserGrantId == id);
            if (_userGrant == null)
            {
                return NotFound();
            }
            return Json(_userGrant);
        }
    }
}
