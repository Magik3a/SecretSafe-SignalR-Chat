namespace Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using SecretSafe.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.SecretSafeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(Data.SecretSafeDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            #region Roles

            if (!roleManager.RoleExists("Normal Security"))
            {
                var role = new ApplicationRole();
                role.Name = "Normal Security";
                role.Level = 1;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Medium Security"))
            {
                var role = new ApplicationRole();
                role.Name = "Medium Security";
                role.Level = 2;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Pro Security"))
            {
                var role = new ApplicationRole();
                role.Name = "Pro Security";
                role.Level = 3;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Maximum Security"))
            {
                var role = new ApplicationRole();
                role.Name = "Maximum Security";
                role.Level = 4;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new ApplicationRole();
                role.Name = "Administrator";
                role.Level = 5;
                roleManager.Create(role);
            }
            #endregion

            if (!(context.Users.Any(u => u.UserName == "svetlin.krastanov90@gmail.com")))
            {
                var userStore = new UserStore<SecretSafeUser>(context);
                var userManager = new UserManager<SecretSafeUser>(userStore);
                var userToInsert = new SecretSafeUser { UserName = "svetlin.krastanov90@gmail.com", PhoneNumber = "0888017004", Email = "svetlin.krastanov90@gmail.com", NickName="Svetlin", ExpirationDateForCurrentRole = DateTime.Now.AddYears(20) };
                userManager.Create(userToInsert, "svetlin90");
                userManager.AddToRole(userToInsert.Id, "Administrator");
            }

            #region Security levels

            context.SecurityLevels.AddOrUpdate(
                s => s.SecurityLevelId,
                new SecretSafe.Models.SecurityLevel()
                {
                    SecurityLevelId = 1,
                    Name = "Normal Security",
                    CreatedOn = DateTime.Now,
                    Level = 1,
                    price =0
                },
                 new SecretSafe.Models.SecurityLevel()
                 {
                     SecurityLevelId = 2,
                     Name = "Medium Security",
                     CreatedOn = DateTime.Now,
                     Level = 2,
                     price = 5
                 },
                  new SecretSafe.Models.SecurityLevel()
                  {
                      SecurityLevelId = 3,
                      Name = "Pro Security",
                      CreatedOn = DateTime.Now,
                      Level = 3,
                      price = 10
                  },
                   new SecretSafe.Models.SecurityLevel()
                   {
                       SecurityLevelId = 4,
                       Name = "Maximum Security",
                       CreatedOn = DateTime.Now,
                       Level = 4,
                       price = 15
                   },
                   new SecretSafe.Models.SecurityLevel()
                   {
                       SecurityLevelId = 5,
                       Name = "Administrator",
                       CreatedOn = DateTime.Now,
                       Level = 5,
                       price = 0
                   }
            );

            #endregion
        }
    }
}
