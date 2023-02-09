using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class UserController : Controller
    {
        public UserController(DataContext context, LibrarySocket socketlib)
        {
            _context = context;
            _socketlib = socketlib;
        }

        private readonly DataContext _context;
        private readonly LibrarySocket _socketlib;
        private const string _reloadtoken = "User"; 

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
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,EmailAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserId = Guid.NewGuid();
                user.UserStatus = true;
                user.PasswordChange = true;
                user.UserCreateDate = DateTime.Now;
                user.PasswordChangeDate = DateTime.Now;
                user.Password = LibraryStatic.ComputeSha512(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                _socketlib.SendEveryoneWebsocket(_reloadtoken);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Edit(Guid id)
        {
            var _user = _context.Users.FirstOrDefault(w => w.UserId == id);
            if (_user == null)
            {
                return NotFound();
            }
            return View(_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,UserName,Password,EmailAddress")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = LibraryStatic.ComputeSha512(user.Password);
                    user.UserStatus = true;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    _socketlib.SendEveryoneWebsocket(_reloadtoken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Where(e => e.UserId == user.UserId).ToList().Any())
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
            return View(user);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult LoadDataTables()
        {
            var draw = HttpContext.Request.Form["draw"].First();
            var start = HttpContext.Request.Form["start"].First();
            var length = HttpContext.Request.Form["length"].First();
            var sortCol = HttpContext.Request.Form["order[0][column]"].First();
            var sortColumn = HttpContext.Request.Form["columns[" + sortCol + "][name]"].First();
            var sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].First();
            var searchValue = HttpContext.Request.Form["search[value]"].First();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var searchdata = (from tempdata in
            _context.Users
                .Where(x => x.UserStatus)
                .Include(ug => ug.UserGroup)
                .ThenInclude(ug => ug.Group)
                .Include(ur => ur.UserRole)
                .ThenInclude(ur => ur.Role)
                .Select(q => new User
                {
                    UserId = q.UserId,
                    UserStatus = q.UserStatus,
                    UserName = q.UserName,
                    EmailAddress = q.EmailAddress,
                    UserGroup = q.UserGroup.Select(w => new UserGroup
                    {
                        UserGroupId = w.UserGroupId,
                        Group = new Group
                        {
                            GroupDescription = w.Group.GroupDescription,
                            GroupCode = w.Group.GroupCode,
                            GroupStatus = w.Group.GroupStatus
                        }
                    }).ToList(),
                    UserRole = q.UserRole.Select(w => new UserRole
                    {
                        UserRoleId = w.UserRoleId,
                        Role = new Role
                        {
                            RoleName = w.Role.RoleName,
                            RoleStatus = w.Role.RoleStatus
                        }
                    }).ToList()
                }).ToList().AsQueryable()
                              select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(User).GetProperty(sortColumn);
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
                x.EmailAddress.Contains(searchValue.Trim()) ||
                x.UserName.Contains(searchValue.Trim()));
            }
            recordsTotal = searchdata.Count();
            List<User> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _user = _context.Users
                .Select(q => new User
                {
                    EmailAddress = q.EmailAddress,
                    UserName = q.UserName,
                    UserStatus = q.UserStatus,
                    UserId = q.UserId,
                }).FirstOrDefault(w => w.UserId == id);
            if (_user == null)
            {
                return NotFound();
            }
            return Json(_user);
        }

        [HttpPost]
        public async Task<IActionResult> Disable(Guid id)
        {
            var _user = _context.Users.FirstOrDefault(w => w.UserId == id);
            if (_user == null)
            {
                return NotFound();
            }
            _user.UserStatus = false;
            _context.Update(_user);
            await _context.SaveChangesAsync();
            _socketlib.SendEveryoneWebsocket(_reloadtoken);
            return Json(new { r = id });
        }
    }
}
