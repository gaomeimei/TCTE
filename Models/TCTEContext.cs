using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace TCTE.Models
{
    public class TCTEContext : DbContext
    {
        static TCTEContext()
        {
            Database.SetInitializer(new TCTEDBInitializerForTest());
        }
        public TCTEContext()
            : base("TCTE")
        {

        }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SalesMan> SalesMen { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public DbSet<RegistrationToken> RegistrationTokens { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //[hx][新增][20150630] Fluent Api 设置 一对一 关系
            modelBuilder.Entity<Terminal>().HasOptional(t => t.SalesMan).WithMany();
        }

    }

    public class TCTEDBInitializer : DropCreateDatabaseIfModelChanges<TCTEContext>
    {
        protected override void Seed(TCTEContext context)
        {
            base.Seed(context);
            //1. create super admin
            context.Users.Add(new User()
            {
                UserName = "admin",
                Password = Utility.EncryptHelper.MD5Encrypt("4rekeyAJ"),
                CreatedDate = DateTime.Now
            });
            context.SaveChanges();
        }
    }

    public class TCTEDBInitializerForTest : TCTEDBInitializer
    {
        protected override void Seed(TCTEContext context)
        {
            base.Seed(context);
            //random tool
            Random rand = new Random();
            //1.city and province and roles
            var province = new Province() { Name = "四川", Abbr = "川" };
            var city = new City { Name = "成都", Abbr = "CD", Provice = province };
            context.Provinces.Add(province);
            context.Cities.Add(city);
            var roles = new List<Role>();
            roles.Add(new Role { Name = "超级管理员" });
            roles.Add(new Role { Name = "商家管理员" });
            context.Roles.AddRange(roles);
            context.SaveChanges();
            //set the super admin role to the user named "admin"
            context.Users.FirstOrDefault(a => a.UserName == "admin").Role = roles[0];
            context.SaveChanges();
            //2.companies
            var companies = new List<Company>();
            for (int i = 1; i <= 20; i++)
            {
                var phone = "1390000" + rand.Next(1000, 9999);
                companies.Add(new Company()
                {
                    Name = "测试公司" + i.ToString(),
                    Abbr = "TE" + i.ToString(),
                    CreatedDate = DateTime.Now,
                    ContactName = "联系人某某",
                    Users = new List<User>
                    {
                        new User()
                        {
                            UserName = "test"+i.ToString(),
                            Password = Utility.EncryptHelper.MD5Encrypt("666666"),
                            CreatedDate = DateTime.Now, 
                            Role = roles[1]
                        }
                    },
                    Address = "测试公司" + i.ToString() + "的地址",
                    Phone = phone,
                    City = city
                });
            }
            context.Companies.AddRange(companies);
            context.SaveChanges();
            //2.1 generate company code
            context.Companies.Include(c=>c.City).ToList().All(c=>
            {
                c.Code = string.Format("{0}{1}{2:000}", c.City.Abbr, c.Abbr, c.Id);
                return true;
            });
            context.SaveChanges();
            //3.salesmen
            var salesmen = new List<SalesMan>();
            for (int i = 1; i <= 100; i++)
            {
                salesmen.Add(new SalesMan
                {
                    Company = companies.OrderBy(a => rand.Next()).Take(1).FirstOrDefault(),
                    Comment = "无",
                    Gender = SystemType.Gender.Male,
                    Name = "测试业务员" + i,
                    Address = "测试业务员" + i + "的地址",
                    Phone = "1381111" + new Random().Next(1000, 9999),
                    CreatedDate = DateTime.Now,
                    IdentityCard = "50000000000000" + new Random().Next(1000, 9999),
                    IsLicenced = true,
                    TranningDate = null
                });
            }
            context.SalesMen.AddRange(salesmen);
            context.SaveChanges();
            //3.1 generate client code
            context.SalesMen.Include(c => c.Company).ToList().All(c =>
            {
                c.Code = string.Format("{0}{1:000}",c.Company.Code, c.Id);
                return true;
            });
            context.SaveChanges();
            //4.clients
            var clients = new List<Client>();
            for (int i = 1; i <= 20; i++)
            {
                var company = companies.OrderBy(a => rand.Next()).Take(1).First();
                clients.Add(new Client
                {
                    Company = company,
                    Comment = "无",
                    Gender = SystemType.Gender.Male,
                    Name = "测试客户" + i,
                    PlateNumber = "川A000" + i,
                    Source = SystemType.Source.Phone,
                    Address = "测试客户" + i + "的地址",
                    Phone = "1380000" + new Random().Next(1000, 9999),
                    City = city
                });
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            //4.1 generate client code
            context.Clients.Include(c => c.Company).ToList().All(c =>
            {
                c.Code = string.Format("{0}{1:000}", c.Company.Code, c.Id);
                return true;
            });
            context.SaveChanges();
            //5.terminals
            var terminals = new List<Terminal>();
            for (int i = 1; i <= 100; i++)
            {
                var company = companies.OrderBy(a => rand.Next()).Take(1).First();
                terminals.Add(new Terminal { Company = company, Status = SystemType.TerminalStatus.Normal,CreateDate=DateTime.Now });
            }
            context.Terminals.AddRange(terminals);
            context.SaveChanges();
            //5.1 generate client code
            context.Terminals.Include(t => t.Company).ToList().All(t =>
            {
                t.Code = string.Format("{0}{1:000}", t.Company.Code, t.Id);
                return true;
            });
            context.SaveChanges();
            //6.orders
            var orders = new List<Order>();
            for (int i = 1; i <= 1000; i++)
            {
                var client = clients.OrderBy(a => rand.Next()).Take(1).First();
                var company = companies.OrderBy(a => rand.Next()).Take(1).First();
                orders.Add(new Order { Client = client, Comment = "无", Company = company, CreatedDate = DateTime.Now, Status = SystemType.OrderStatus.Created, StartTime = null, EndTime = null });
            }
            context.Orders.AddRange(orders);
            context.SaveChanges();
            context.Orders.Include(o => o.Company).ToList().All(o =>
            {
                o.Code = string.Format("{0}{1}{2:000}", o.Company.Code, DateTime.Now.ToString("yyyyMMdd"), o.Id);
                return true;
            });
            context.SaveChanges();
            //6.orders
            //7. token
            context.RegistrationTokens.Add(new RegistrationToken()
            {
                Token = "123",
                Category = "dd"
            });
            context.SaveChanges();
        }
    }
}
