using aspnetmvc_blog.Models;

namespace aspnetmvc_blog.Data
{
    public class DataInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            AppSetting _appsetting = new()
            {
                AppSettingId = Guid.NewGuid(),
                AppSettingCode = "ecryptkey",
                AppSettingValue = "SuperSecretKeyPleaseChange"
            };
            context.AppSettings.Add(_appsetting);
            AppLog _applog = new()
            {
                AppLogId = Guid.NewGuid(),
                AppLogAddress = "",
                AppLogAction = "Database and System Initializeded",
                AppLogController = "DbInitializer",
                AppLogDate = DateTime.Now,
                AppLogUser = "System",
                AppLogQuery = "",
            };
            context.AppLogs.Add(_applog);

            string defaultPassword = "pass";
            List<User> userlist = new()
            {
                new User
                {
                    UserId = new Guid("93074a04-c9d4-4754-9d20-08d734c48ace"),
                    UserStatus = true,
                    UserCreateDate= DateTime.Now,
                    UserName = "user1",
                    EmailAddress = "email@website.com",
                    Password = LibraryStatic.ComputeSha512(defaultPassword),
                    PasswordChange = false,
                    PasswordChangeDate= DateTime.Now,
                },
                new User
                {
                    UserId  = new Guid("0fcc4fad-d80f-4e36-9d21-08d734c48ace"),
                    UserStatus = true,
                    UserCreateDate= DateTime.Now,
                    UserName = "user2",
                    EmailAddress = "email@website.com",
                    Password = LibraryStatic.ComputeSha512(defaultPassword),
                    PasswordChange = false,
                    PasswordChangeDate= DateTime.Now,
                },
                new User
                {
                    UserId  = new Guid("6183e88e-a852-4949-9d22-08d734c48ace"),
                    UserStatus = true,
                    UserCreateDate= DateTime.Now,
                    UserName = "user3",
                    EmailAddress = "email@website.com",
                    Password = LibraryStatic.ComputeSha512(defaultPassword),
                    PasswordChange = false,
                    PasswordChangeDate= DateTime.Now,
                },
                new User
                {
                    UserId  = new Guid("9b4a18b5-ac49-4b71-c963-08d73724472f"),
                    UserStatus = true,
                    UserCreateDate= DateTime.Now,
                    UserName = "admin",
                    EmailAddress = "email@website.com",
                    Password = LibraryStatic.ComputeSha512(defaultPassword),
                    PasswordChange = false,
                    PasswordChangeDate= DateTime.Now,
                }
            };
            context.Users.AddRange(userlist);

            List<Group> grouplist = new()
            {
                new Group
                {
                    GroupId = new Guid("fffb1f1c-5ba3-421f-87eb-00050bb93998"),
                    GroupCode = "GROUPONE",
                    GroupDescription = "Group One",
                    GroupStatus = true,
                    GroupCreateDate = DateTime.Now,
                },
                new Group
                {
                    GroupId  = new Guid("bc2e62ae-ffe1-493d-b6db-004a472e5d88"),
                    GroupCode = "GROUPTWO",
                    GroupDescription = "Group Two",
                    GroupStatus = true,
                    GroupCreateDate = DateTime.Now,
                }
            };
            context.Groups.AddRange(grouplist);

            List<UserGroup> usergrouplist = new()
            {
                new UserGroup
                {
                    UserGroupId = Guid.NewGuid(),
                    UserId = new Guid("93074a04-c9d4-4754-9d20-08d734c48ace"), // user1
                    GroupId = new Guid("bc2e62ae-ffe1-493d-b6db-004a472e5d88"), // group two
                    UserGroupOrder = 0
                },
                new UserGroup
                {
                    UserGroupId = Guid.NewGuid(),
                    UserId  = new Guid("0fcc4fad-d80f-4e36-9d21-08d734c48ace"), // user2
                    GroupId  = new Guid("bc2e62ae-ffe1-493d-b6db-004a472e5d88"), // group two
                    UserGroupOrder = 0
                },
                new UserGroup
                {
                    UserGroupId = Guid.NewGuid(),
                    UserId  = new Guid("6183e88e-a852-4949-9d22-08d734c48ace"), // user3
                    GroupId  = new Guid("bc2e62ae-ffe1-493d-b6db-004a472e5d88"), // group two
                    UserGroupOrder = 0
                },
                new UserGroup
                {
                    UserGroupId = Guid.NewGuid(),
                    UserId  = new Guid("9b4a18b5-ac49-4b71-c963-08d73724472f"), // admin
                    GroupId  = new Guid("fffb1f1c-5ba3-421f-87eb-00050bb93998"), // group one
                    UserGroupOrder = 0
                },
            };
            context.UserGroups.AddRange(usergrouplist);

            List<Role> rolelist = new()
            {
                new Role
                {
                    RoleId  = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                    RoleStatus = true,
                    RoleName = "User",
                    AdministratorFlag = false,
                    RegisterFlag = true,
                    DefaultController = "Account",
                    DefaultAction = "Index",
                    RoleCreateDate = DateTime.Now,
                },
                new Role
                {
                    RoleId  = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                    RoleStatus = true,
                    RoleName = "Administrator",
                    AdministratorFlag = true,
                    RegisterFlag = false,
                    DefaultController = "User",
                    DefaultAction = "Index",
                    RoleCreateDate = DateTime.Now,
                }
            };
            context.Roles.AddRange(rolelist);

            List<UserRole> userrolelist = new()
            {
                new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId = new Guid("93074a04-c9d4-4754-9d20-08d734c48ace"), // user1
                    RoleId  = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"), // user
                    UserRoleOrder = 0
                },
                new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId  = new Guid("0fcc4fad-d80f-4e36-9d21-08d734c48ace"), // user2
                    RoleId  = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"), // user
                    UserRoleOrder = 0
                },
                new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId  = new Guid("6183e88e-a852-4949-9d22-08d734c48ace"), // user3
                    RoleId  = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"), // user
                    UserRoleOrder = 0
                },
                new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId  = new Guid("9b4a18b5-ac49-4b71-c963-08d73724472f"), // admin
                    RoleId  = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"), // admin
                    UserRoleOrder = 0
                },
            };
            context.UserRoles.AddRange(userrolelist);

            List<Module> modulelist = new()
            {
                new Module
                {
                    ModuleId=new Guid("2d0b861a-ce5e-4673-8d4f-f929039969d6"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Blog Create",
                    ModuleController="Blog",
                    ModuleAction="Create",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-blog",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("d58312f0-a11d-4117-9f4a-7d9a648215d0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Blog Edit",
                    ModuleController="Blog",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                {
                    ModuleId=new Guid("17b0c489-04a5-4c5f-9ea8-02ebb70436ee"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Setting LoadDataTables",
                    ModuleController="Setting",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("6d919041-f90a-4470-9904-068124159300"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="User Disable",
                    ModuleController="User",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("c1d73b0a-8752-47f5-ba41-0b501c496f3e"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Role LoadDataTables",
                    ModuleController="Role",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("bab35156-800c-401a-bbd0-0cb7b51e9975"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole AddPriority",
                    ModuleController="UserRole",
                    ModuleAction="AddPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("62ba6f56-9326-4985-86dd-1129081952c0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="User Edit",
                    ModuleController="User",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("b803cf19-465d-45c1-81a5-12492f01898e"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant LoadDataTables",
                    ModuleController="UserGrant",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("65326361-d8af-47ff-bb84-1482de4e4674"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup LoadDataTables",
                    ModuleController="UserGroup",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("2383f732-4acc-4d7b-bef4-169a360cd792"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Dedupe",
                    ModuleController="Module",
                    ModuleAction="Dedupe",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("6fbb4d78-cedb-40df-aa1f-18169d86c17e"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module",
                    ModuleController="Module",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-hotdog",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("f2981018-7974-4f04-9571-190a9737d5dc"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant Edit",
                    ModuleController="UserGrant",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("b94a00a4-b992-49a1-b17f-1996b08ba07e"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Disable",
                    ModuleController="Module",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("979b4800-f40d-499b-8bde-1e2a00e9f5b8"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup Remove",
                    ModuleController="UserGroup",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("b3c47a71-1c8d-4fa4-aee0-1f05002fd192"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Group Edit",
                    ModuleController="Group",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("19113d96-8fb4-4bea-8df7-201133f77917"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Role Disable",
                    ModuleController="Role",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("6b73f46e-3cd7-4dc6-bddf-2531ed0aeb1d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Remove",
                    ModuleController="Module",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("1c686431-0d08-47ba-9ed1-255c21659bc3"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Clean",
                    ModuleController="Module",
                    ModuleAction="Clean",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("b7f5d288-da49-4d51-b04c-2884fc168c76"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module LoadDataTables",
                    ModuleController="Module",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3ee015e6-dd04-409e-ab58-294322b51c43"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Index",
                    ModuleController="RoleModule",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("cadf9317-7f13-47cc-89a5-2ffb5cf0162d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Edit",
                    ModuleController="RoleModule",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("e299e78e-41cb-4955-9392-32b314230df9"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Account LoadUserRoleDataTables",
                    ModuleController="Account",
                    ModuleAction="LoadUserRoleDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("b312c5d0-fc97-40c1-a847-397e0ce4a85e"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant Create",
                    ModuleController="UserGrant",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("de1f99e8-d59f-413c-9eed-3c3714448db5"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant GetInfoJSON",
                    ModuleController="UserGrant",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("ef2d13ad-1ddd-471e-8e34-3ca3297d2925"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule LoadDataTables",
                    ModuleController="RoleModule",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("689484a2-636e-45d7-8fef-3cd023ed1750"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole LoadDataTables",
                    ModuleController="UserRole",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("d86c3c6d-a43f-421f-8ce7-42eceb19acf5"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Remove",
                    ModuleController="UserModule",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("2a6e7c2b-bf07-4e42-aad8-4e8b4e678f84"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Group",
                    ModuleController="Group",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-people-group",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("9153943b-799b-462e-bff8-5347402ef12d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Enable",
                    ModuleController="RoleModule",
                    ModuleAction="Enable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("2df8bc82-b58d-4ceb-9319-578a8e233d24"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Disable",
                    ModuleController="UserModule",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("6f1c5d29-623a-41d6-bed2-57a133d32185"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Edit",
                    ModuleController="UserModule",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3535f911-697e-4074-b591-5a02f229cb5b"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule AddPriority",
                    ModuleController="UserModule",
                    ModuleAction="AddPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("0fae5924-50fe-4316-9c14-65751143c83f"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup Edit",
                    ModuleController="UserGroup",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("2bd6dc9a-506c-4258-8bbd-67d0189206e4"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Create",
                    ModuleController="User",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("93cf26eb-89dc-48de-b5d8-6ac95157bc2a"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole LowerPriority",
                    ModuleController="UserRole",
                    ModuleAction="LowerPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("a444b4f3-d695-430c-930c-6cda6d1421ab"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule LoadDataTables",
                    ModuleController="UserModule",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("2ffc5c08-9a56-460a-9836-6e99bba6e9aa"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="User LoadDataTables",
                    ModuleController="User",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("dadbcf5a-44be-4a3a-945b-6ee3f332bbd9"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole Edit",
                    ModuleController="UserRole",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("93a0c9f3-f938-4f4a-945f-6f6a858a9dd2"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Account Password",
                    ModuleController="Account",
                    ModuleAction="Password",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("bae30048-005d-4992-a161-733ef7e1501c"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole Create",
                    ModuleController="UserRole",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("829694ae-f530-473e-bbe7-7511b5237d02"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module GetInfoJSON",
                    ModuleController="Module",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("6f4870cf-ef9f-4596-8934-7b1a3c9d1c05"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup Index",
                    ModuleController="UserGroup",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("bcd0fd50-72b7-4200-b386-7b8f67b364ab"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Log",
                    ModuleController="Log",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-clipboard-list",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("7b46547b-04bb-46e1-8989-7d4f148aac5f"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Log LoadDataTables",
                    ModuleController="Log",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("7ffe38a6-653f-4f61-9c30-814a10a5ec34"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Disable",
                    ModuleController="RoleModule",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("bbb7d06d-269b-49e4-8359-82637c14e8d3"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Group Disable",
                    ModuleController="Group",
                    ModuleAction="Disable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("c43e9bde-4067-4769-9b57-842ddd3ac40f"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Create",
                    ModuleController="UserModule",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("4e81f402-cbf2-45da-935f-8b7ba19b4b35"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="My Account",
                    ModuleController="Account",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("8810677a-0e75-4b65-98a7-8c809aeb086b"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Account LoadUserGroupDataTables",
                    ModuleController="Account",
                    ModuleAction="LoadUserGroupDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("873c0956-ca72-49a8-9d0f-912a198c6d9f"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="User GetInfoJSON",
                    ModuleController="User",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("69fc14a8-4cb0-4daa-bcf2-93aee9a5cd14"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Setting GetInfoJSON",
                    ModuleController="Setting",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("c603b0e4-a67d-4103-80ca-956716a2aaab"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup AddPriority",
                    ModuleController="UserGroup",
                    ModuleAction="AddPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("d85d5782-89df-4055-af24-9a693410685d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant Remove",
                    ModuleController="UserGrant",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("f43f045e-6813-4e0a-a7a8-a0036a32c8de"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Log GetInfoJSON",
                    ModuleController="Log",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("5e201be5-3342-408d-8e5e-a1cea3658540"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Index",
                    ModuleController="UserModule",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("96126c64-0e5e-432c-9f80-a40851d2a5ce"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup LowerPriority",
                    ModuleController="UserGroup",
                    ModuleAction="LowerPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("64746b22-edbe-4e82-b9c5-a4118cd18c36"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Remove",
                    ModuleController="RoleModule",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3c386912-b081-4c95-9f04-a4b1e6534ebe"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Enable",
                    ModuleController="Module",
                    ModuleAction="Enable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("26b7cd86-d1b0-4269-8865-aa962a81b9be"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Role Edit",
                    ModuleController="Role",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("f6228461-b025-4157-af98-afa132832e67"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule Enable",
                    ModuleController="UserModule",
                    ModuleAction="Enable",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("ef6523a7-62ac-4b1b-8066-b28e0aa59ed5"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Setting Edit",
                    ModuleController="Setting",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("71d48155-2315-439c-b35a-b7a11501e6e0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGrant Index",
                    ModuleController="UserGrant",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("34c0728c-5c8b-4f0c-9cea-b920fe69d97a"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Create",
                    ModuleController="Group",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("d5f3bee2-bb92-4f31-8572-bfe7afef5ea0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserModule LowerPriority",
                    ModuleController="UserModule",
                    ModuleAction="LowerPriority",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("86b990e0-3089-4f17-ad54-c8441bc394b1"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Role",
                    ModuleController="Role",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-hat-wizard",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("0b21bde0-41f2-4cb1-91c6-ca2998c254f0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Create",
                    ModuleController="RoleModule",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("8087439f-83a4-45aa-8867-cd187fa6ff58"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserGroup Create",
                    ModuleController="UserGroup",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("1bd2e428-fe3e-4e6a-b04a-ce2bfb01fb75"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule GetInfoJSON",
                    ModuleController="RoleModule",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("dbc3ab5d-f9fd-40b9-9cb5-de9aa4f05b46"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="User",
                    ModuleController="User",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-users",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("0ac154d7-efba-4381-bc43-e01db13bb20b"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Create",
                    ModuleController="Module",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("9f7bac5d-e5b7-497e-b73f-e559eb090086"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Account LoadUserGrantDataTables",
                    ModuleController="Account",
                    ModuleAction="LoadUserGrantDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("48e27118-093d-401a-9458-e73b6b15ea2d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="RoleModule Populate",
                    ModuleController="RoleModule",
                    ModuleAction="Populate",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3620e4ad-b402-48c4-ab70-e86a21f61dd0"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Role GetInfoJSON",
                    ModuleController="Role",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("be1e0b3f-05c3-474d-b870-e9f5c5a816cf"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole Index",
                    ModuleController="UserRole",
                    ModuleAction="Index",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("61c0b225-dae3-433f-8a80-efcc8e996d46"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Setting",
                    ModuleController="Setting",
                    ModuleAction="Index",
                    ModuleVisibility= true,
                    ModuleIconClass ="fa-solid fa-gear",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("1441e8cb-bde1-4716-9263-f20ebe4579e8"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Create",
                    ModuleController="Role",
                    ModuleAction="Create",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("be76a60b-0e4f-46cd-94c7-f2bb07d18778"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="UserRole Remove",
                    ModuleController="UserRole",
                    ModuleAction="Remove",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3338b4f5-638a-4aeb-b7e4-f764c9c5ee35"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Module Edit",
                    ModuleController="Module",
                    ModuleAction="Edit",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("cfd4a5bc-15e3-4786-902b-f9c75d44a63f"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Group LoadDataTables",
                    ModuleController="Group",
                    ModuleAction="LoadDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("18190067-4298-46bc-b9c2-faf6e62c210d"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Account LoadUserModuleDataTables",
                    ModuleController="Account",
                    ModuleAction="LoadUserModuleDataTables",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  },
                new Module
                  {
                    ModuleId=new Guid("3cb533b2-a040-4891-a963-fb1f24b1c915"),
                    ModuleCreateDate=DateTime.Now,
                    ModuleOrder=0,
                    ModuleName ="Group GetInfoJSON",
                    ModuleController="Group",
                    ModuleAction="GetInfoJSON",
                    ModuleVisibility= false,
                    ModuleIconClass ="fas fa-question-circle",
                    ModuleTooltip="tooltip"
                  }

            };
            context.Modules.AddRange(modulelist);

            List<RoleModule> rolemodulelist = new()
            {
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("be76a60b-0e4f-46cd-94c7-f2bb07d18778")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("979b4800-f40d-499b-8bde-1e2a00e9f5b8")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("829694ae-f530-473e-bbe7-7511b5237d02")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("c43e9bde-4067-4769-9b57-842ddd3ac40f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("b312c5d0-fc97-40c1-a847-397e0ce4a85e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("9153943b-799b-462e-bff8-5347402ef12d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("6fbb4d78-cedb-40df-aa1f-18169d86c17e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("cadf9317-7f13-47cc-89a5-2ffb5cf0162d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("dadbcf5a-44be-4a3a-945b-6ee3f332bbd9")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3cb533b2-a040-4891-a963-fb1f24b1c915")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3338b4f5-638a-4aeb-b7e4-f764c9c5ee35")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("7b46547b-04bb-46e1-8989-7d4f148aac5f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("e299e78e-41cb-4955-9392-32b314230df9")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("69fc14a8-4cb0-4daa-bcf2-93aee9a5cd14")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3535f911-697e-4074-b591-5a02f229cb5b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("bbb7d06d-269b-49e4-8359-82637c14e8d3")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("2a6e7c2b-bf07-4e42-aad8-4e8b4e678f84")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("b94a00a4-b992-49a1-b17f-1996b08ba07e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("b94a00a4-b992-49a1-b17f-1996b08ba07e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("c603b0e4-a67d-4103-80ca-956716a2aaab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("62ba6f56-9326-4985-86dd-1129081952c0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("2ffc5c08-9a56-460a-9836-6e99bba6e9aa")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("4e81f402-cbf2-45da-935f-8b7ba19b4b35")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("4e81f402-cbf2-45da-935f-8b7ba19b4b35")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("be1e0b3f-05c3-474d-b870-e9f5c5a816cf")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("ef6523a7-62ac-4b1b-8066-b28e0aa59ed5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("71d48155-2315-439c-b35a-b7a11501e6e0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("5e201be5-3342-408d-8e5e-a1cea3658540")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("5e201be5-3342-408d-8e5e-a1cea3658540")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("7b46547b-04bb-46e1-8989-7d4f148aac5f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("26b7cd86-d1b0-4269-8865-aa962a81b9be")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("f2981018-7974-4f04-9571-190a9737d5dc")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("48e27118-093d-401a-9458-e73b6b15ea2d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("f2981018-7974-4f04-9571-190a9737d5dc")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("689484a2-636e-45d7-8fef-3cd023ed1750")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("7ffe38a6-653f-4f61-9c30-814a10a5ec34")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("d86c3c6d-a43f-421f-8ce7-42eceb19acf5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("b7f5d288-da49-4d51-b04c-2884fc168c76")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("8087439f-83a4-45aa-8867-cd187fa6ff58")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3c386912-b081-4c95-9f04-a4b1e6534ebe")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("c1d73b0a-8752-47f5-ba41-0b501c496f3e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("8810677a-0e75-4b65-98a7-8c809aeb086b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("dbc3ab5d-f9fd-40b9-9cb5-de9aa4f05b46")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("dadbcf5a-44be-4a3a-945b-6ee3f332bbd9")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("1c686431-0d08-47ba-9ed1-255c21659bc3")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("be1e0b3f-05c3-474d-b870-e9f5c5a816cf")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("d5f3bee2-bb92-4f31-8572-bfe7afef5ea0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("2df8bc82-b58d-4ceb-9319-578a8e233d24")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("6f1c5d29-623a-41d6-bed2-57a133d32185")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("b312c5d0-fc97-40c1-a847-397e0ce4a85e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("93a0c9f3-f938-4f4a-945f-6f6a858a9dd2")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("61c0b225-dae3-433f-8a80-efcc8e996d46")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("b7f5d288-da49-4d51-b04c-2884fc168c76")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("9f7bac5d-e5b7-497e-b73f-e559eb090086")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("d5f3bee2-bb92-4f31-8572-bfe7afef5ea0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("71d48155-2315-439c-b35a-b7a11501e6e0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("0ac154d7-efba-4381-bc43-e01db13bb20b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("0ac154d7-efba-4381-bc43-e01db13bb20b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("26b7cd86-d1b0-4269-8865-aa962a81b9be")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("86b990e0-3089-4f17-ad54-c8441bc394b1")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("6f4870cf-ef9f-4596-8934-7b1a3c9d1c05")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("bae30048-005d-4992-a161-733ef7e1501c")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("e299e78e-41cb-4955-9392-32b314230df9")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("93cf26eb-89dc-48de-b5d8-6ac95157bc2a")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("0b21bde0-41f2-4cb1-91c6-ca2998c254f0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3cb533b2-a040-4891-a963-fb1f24b1c915")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("62ba6f56-9326-4985-86dd-1129081952c0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("b803cf19-465d-45c1-81a5-12492f01898e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("18190067-4298-46bc-b9c2-faf6e62c210d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("f6228461-b025-4157-af98-afa132832e67")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("dbc3ab5d-f9fd-40b9-9cb5-de9aa4f05b46")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("61c0b225-dae3-433f-8a80-efcc8e996d46")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("93cf26eb-89dc-48de-b5d8-6ac95157bc2a")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("873c0956-ca72-49a8-9d0f-912a198c6d9f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("bbb7d06d-269b-49e4-8359-82637c14e8d3")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("979b4800-f40d-499b-8bde-1e2a00e9f5b8")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("1bd2e428-fe3e-4e6a-b04a-ce2bfb01fb75")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("18190067-4298-46bc-b9c2-faf6e62c210d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("19113d96-8fb4-4bea-8df7-201133f77917")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("6b73f46e-3cd7-4dc6-bddf-2531ed0aeb1d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("1441e8cb-bde1-4716-9263-f20ebe4579e8")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("17b0c489-04a5-4c5f-9ea8-02ebb70436ee")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("1441e8cb-bde1-4716-9263-f20ebe4579e8")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("f43f045e-6813-4e0a-a7a8-a0036a32c8de")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("a444b4f3-d695-430c-930c-6cda6d1421ab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("2ffc5c08-9a56-460a-9836-6e99bba6e9aa")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("b3c47a71-1c8d-4fa4-aee0-1f05002fd192")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3c386912-b081-4c95-9f04-a4b1e6534ebe")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("2bd6dc9a-506c-4258-8bbd-67d0189206e4")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("6f4870cf-ef9f-4596-8934-7b1a3c9d1c05")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("6f1c5d29-623a-41d6-bed2-57a133d32185")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("2df8bc82-b58d-4ceb-9319-578a8e233d24")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("bab35156-800c-401a-bbd0-0cb7b51e9975")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("8087439f-83a4-45aa-8867-cd187fa6ff58")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("be76a60b-0e4f-46cd-94c7-f2bb07d18778")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("96126c64-0e5e-432c-9f80-a40851d2a5ce")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("0fae5924-50fe-4316-9c14-65751143c83f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("de1f99e8-d59f-413c-9eed-3c3714448db5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("65326361-d8af-47ff-bb84-1482de4e4674")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("2383f732-4acc-4d7b-bef4-169a360cd792")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("f6228461-b025-4157-af98-afa132832e67")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("b803cf19-465d-45c1-81a5-12492f01898e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("1bd2e428-fe3e-4e6a-b04a-ce2bfb01fb75")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("48e27118-093d-401a-9458-e73b6b15ea2d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("17b0c489-04a5-4c5f-9ea8-02ebb70436ee")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("cfd4a5bc-15e3-4786-902b-f9c75d44a63f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("7ffe38a6-653f-4f61-9c30-814a10a5ec34")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("6d919041-f90a-4470-9904-068124159300")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("2a6e7c2b-bf07-4e42-aad8-4e8b4e678f84")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("93a0c9f3-f938-4f4a-945f-6f6a858a9dd2")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("f43f045e-6813-4e0a-a7a8-a0036a32c8de")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("6fbb4d78-cedb-40df-aa1f-18169d86c17e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("9f7bac5d-e5b7-497e-b73f-e559eb090086")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("d85d5782-89df-4055-af24-9a693410685d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("96126c64-0e5e-432c-9f80-a40851d2a5ce")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("ef6523a7-62ac-4b1b-8066-b28e0aa59ed5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("cadf9317-7f13-47cc-89a5-2ffb5cf0162d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("2bd6dc9a-506c-4258-8bbd-67d0189206e4")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("19113d96-8fb4-4bea-8df7-201133f77917")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("64746b22-edbe-4e82-b9c5-a4118cd18c36")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3535f911-697e-4074-b591-5a02f229cb5b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("2383f732-4acc-4d7b-bef4-169a360cd792")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("34c0728c-5c8b-4f0c-9cea-b920fe69d97a")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("b3c47a71-1c8d-4fa4-aee0-1f05002fd192")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("d86c3c6d-a43f-421f-8ce7-42eceb19acf5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("bab35156-800c-401a-bbd0-0cb7b51e9975")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("ef2d13ad-1ddd-471e-8e34-3ca3297d2925")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("65326361-d8af-47ff-bb84-1482de4e4674")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("9153943b-799b-462e-bff8-5347402ef12d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3620e4ad-b402-48c4-ab70-e86a21f61dd0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("6b73f46e-3cd7-4dc6-bddf-2531ed0aeb1d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("d85d5782-89df-4055-af24-9a693410685d")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("8810677a-0e75-4b65-98a7-8c809aeb086b")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("829694ae-f530-473e-bbe7-7511b5237d02")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("c1d73b0a-8752-47f5-ba41-0b501c496f3e")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("cfd4a5bc-15e3-4786-902b-f9c75d44a63f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("ef2d13ad-1ddd-471e-8e34-3ca3297d2925")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("34c0728c-5c8b-4f0c-9cea-b920fe69d97a")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("0fae5924-50fe-4316-9c14-65751143c83f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("689484a2-636e-45d7-8fef-3cd023ed1750")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("c43e9bde-4067-4769-9b57-842ddd3ac40f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3338b4f5-638a-4aeb-b7e4-f764c9c5ee35")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("69fc14a8-4cb0-4daa-bcf2-93aee9a5cd14")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3620e4ad-b402-48c4-ab70-e86a21f61dd0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("bae30048-005d-4992-a161-733ef7e1501c")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("0b21bde0-41f2-4cb1-91c6-ca2998c254f0")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("6d919041-f90a-4470-9904-068124159300")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("c603b0e4-a67d-4103-80ca-956716a2aaab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("a444b4f3-d695-430c-930c-6cda6d1421ab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("3ee015e6-dd04-409e-ab58-294322b51c43")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("873c0956-ca72-49a8-9d0f-912a198c6d9f")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("bcd0fd50-72b7-4200-b386-7b8f67b364ab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("bcd0fd50-72b7-4200-b386-7b8f67b364ab")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("3ee015e6-dd04-409e-ab58-294322b51c43")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("de1f99e8-d59f-413c-9eed-3c3714448db5")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = true,
                      RoleId = new Guid("173198b3-3c6d-4bcd-a490-ed46fb5311b7"),
                      ModuleId  = new Guid("86b990e0-3089-4f17-ad54-c8441bc394b1")
                    },
                new RoleModule
                    {
                      RoleModuleId = Guid.NewGuid(),
                      RoleModuleOrder = 0,
                      RoleModuleAccess = false,
                      RoleId = new Guid("4c0c2379-afa7-40a9-bd9d-e622ff4ffc60"),
                      ModuleId  = new Guid("1c686431-0d08-47ba-9ed1-255c21659bc3")
                    }

            };
            context.RoleModules.AddRange(rolemodulelist);

            context.SaveChanges();
        }
    }
}
