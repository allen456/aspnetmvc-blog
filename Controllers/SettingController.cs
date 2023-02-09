using aspnetmvc_blog.Data;
using aspnetmvc_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace aspnetmvc_blog.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class SettingController : Controller
    {
        public SettingController(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            var _setting = _context.AppSettings.FirstOrDefault(a => a.AppSettingId == id);
            if (_setting == null)
            {
                return NotFound();
            }
            return View(_setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppSettingId,AppSettingCode,AppSettingValue")] AppSetting _data)
        {
            if (id != _data.AppSettingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AppSettings.Where(e => e.AppSettingId == _data.AppSettingId).ToList().Any())
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
            return View(_data);
        }

        [HttpPost]
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
            var searchdata = (from tempdata in _context.AppSettings.ToList().AsQueryable() select tempdata);
            if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
            {
                var convertProperty = typeof(AppSetting).GetProperty(sortColumn);
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
                x.AppSettingCode.Contains(searchValue) ||
                x.AppSettingValue.Contains(searchValue));
            }
            recordsTotal = searchdata.Count();
            List<AppSetting> data = searchdata.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        public IActionResult GetInfoJSON(Guid id)
        {
            var _setting = _context.AppSettings.FirstOrDefault(a => a.AppSettingId == id);
            if (_setting == null)
            {
                return NotFound();
            }
            return Json(_setting);
        }
    }
}
