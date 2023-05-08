using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskVK.Controllers;
using TestTaskVK.Helpers;
using TestTaskVK.Models;
using Xunit;


namespace TestTaskVK.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Tests()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptions<ApplicationContext>();
            DbContextOptionsBuilder<ApplicationContext> builder = new DbContextOptionsBuilder<ApplicationContext>(options);
            builder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=12wq");
            ApplicationContext context = new ApplicationContext(builder.Options);
            HomeController homeController = new HomeController(context);
            Task<ActionResult<PagingHelper.IndexViewModel>> result = homeController.GetUsers();
            Assert.NotNull(result);
        }
    }
}
