using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using TestTaskVK.Helpers;
using TestTaskVK.Models;


namespace TestTaskVK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        ApplicationContext db;

        public HomeController(ApplicationContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<PagingHelper.IndexViewModel>> GetUsers(int page = 1)
        {
            int pageSize = 3;

            var users = await db.Users
                .Where(x => x.UserState!.Code == CodeState.Active)
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .AsQueryable()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            PagingHelper.PageInfo pageInfo = new PagingHelper.PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Users.Where(x => x.UserState!.Code == CodeState.Active).Count() };
            PagingHelper.IndexViewModel ivm = new PagingHelper.IndexViewModel { PageInfo = pageInfo, Users = users };
            return ivm;
        }

        [HttpGet("userById")]
        public async Task<ActionResult<User>> GetUserById()
        {
            string idString = Request.QueryString.Value!.Trim(new char[] { '?', 'i', 'd', '=' });
            int id = 0;
            int.TryParse(idString, out id);
            if (id == 0)
                return BadRequest();
            var user = await db.Users
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            if (user == null)
                return BadRequest();
            if (user.UserGroup!.Code == CodeGroup.Admin)
                if (db.Users.Any(x => x.UserGroup!.Code == CodeGroup.Admin && x.UserState!.Code == CodeState.Active))
                    return BadRequest("Admin already exists");
            UserHelper.Add(user.Login!);
            await Task.Delay(5000);
            if (UserHelper.Check(user.Login!))
                return BadRequest("Attempt to create more than one user with the same username");
            if(user.CreatedDate.Year == 1)
                user.CreatedDate = DateTime.Now;
            db.Users.Add(user);
            await db.SaveChangesAsync();
            UserHelper.Clear();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (id < 1)
                return BadRequest();
            User? user = await db.Users
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            user.UserState!.Code = CodeState.Blocked;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
