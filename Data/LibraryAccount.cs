using aspnetmvc_blog.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Data
{
    public class LibraryAccount
    {
        public LibraryAccount(DataContext context) { _context = context; }
        private readonly DataContext _context;

        public bool IsUserAuthorized(Guid? userId, string action, string controller)
        {
            if (!_context.Modules.Where(w => w.ModuleAction == action && w.ModuleController == controller).ToList().Any())
            {
                Module _data = new()
                {
                    ModuleId = Guid.NewGuid(),
                    ModuleCreateDate = DateTime.Now,
                    ModuleAction = action,
                    ModuleController = controller,
                    ModuleName = controller + " " + action,
                    ModuleVisibility = true,
                    ModuleOrder = 0,
                    ModuleIconClass = "fas fa-question-circle",
                    ModuleTooltip = "tooltip"
                };
                _context.Modules.Add(_data);
                _context.Entry(_data).State = EntityState.Added;
                foreach (Role item in _context.Roles.Where(w => w.RoleStatus == true).ToList())
                {
                    Guid _ModuleId = _data.ModuleId;
                    if (!_context.RoleModules.Where(w => w.ModuleId == _ModuleId && w.RoleId == item.RoleId).ToList().Any())
                    {
                        RoleModule newRoleDetail = new RoleModule
                        {
                            RoleModuleId = Guid.NewGuid(),
                            ModuleId = _ModuleId,
                            RoleId = item.RoleId,
                            RoleModuleAccess = false
                        };
                        _context.RoleModules.Attach(newRoleDetail);
                        _context.Entry(newRoleDetail).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
            }

            if (userId == null)
            {
                return false;
            }
            if (!_context.UserRoles.Where(w => w.UserId == userId).ToList().Any())
            {
                return false;
            }
            Guid moduleID = _context.Modules.FirstOrDefault(w => w.ModuleAction == action && w.ModuleController == controller).ModuleId;
            if (_context.UserModules.Where(w => w.UserId == userId && w.ModuleId == moduleID).ToList().Any())
            {
                return _context.UserModules.Where(w => w.UserId == userId && w.ModuleId == moduleID).OrderByDescending(w => w.UserModuleOrder).FirstOrDefault()
                    .UserModuleAccess;
            }
            Guid roleId = Guid.NewGuid();
            foreach (UserRole item in _context.UserRoles.Where(w => w.UserId == userId).OrderByDescending(w => w.UserRoleOrder).ToList())
            {
                roleId = item.RoleId;
                if (_context.Roles.Where(w => w.RoleId == roleId).ToList().FirstOrDefault().AdministratorFlag)
                {
                    return true;
                }
                if (_context.RoleModules.Where(w => w.RoleId == roleId && w.ModuleId == moduleID).ToList().Any())
                {
                    if (_context.RoleModules.Where(w => w.RoleId == roleId && w.ModuleId == moduleID).OrderByDescending(w => w.RoleModuleOrder).ToList().FirstOrDefault().RoleModuleAccess)
                    {
                        return true;
                    }
                }
            }
            if (_context.RoleModules.Where(w => w.RoleId == roleId && w.ModuleId == moduleID).ToList().Any())
            {
                return _context.RoleModules.Where(w => w.RoleId == roleId && w.ModuleId == moduleID).OrderByDescending(w => w.RoleModuleOrder).ToList().FirstOrDefault().RoleModuleAccess;
            }
            return false;
        }

        public bool IsLoginCorrect(string username, string password)
        {
            if (_context.Users.Where(w => w.UserName == username && w.Password == LibraryStatic.ComputeSha512(password) && w.UserStatus == true).ToList().Any())
            {
                return true;
            }
            if (_context.Users.Where(w => w.UserName == username && w.Password == "" && w.UserStatus == true).ToList().Any())
            {
                return true;
            }
            return false;
        }

    }
}
