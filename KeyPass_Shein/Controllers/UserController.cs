using KeyPass_Shein.Classes;
using KeyPass_Shein.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Shein.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private DatabaseManager databaseManager;

        public UserController()
        {
            this.databaseManager = this.databaseManager = new DatabaseManager();
        }

        [Route("register")]
        [HttpPost]
        public ActionResult Register([FromForm] string login, [FromForm] string password)
        {
            try
            {
                bool exists = databaseManager.Users.Any(x => x.Login == login);
                if (exists)
                    return StatusCode(409, "Пользователь с таким логином уже существует");

                User newUser = new User
                {
                    Login = login,
                    Password = PasswordHasher.Hash(password)
                };
                databaseManager.Users.Add(newUser);
                databaseManager.SaveChanges();
                return StatusCode(201, "Пользователь создан");
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromForm] string login, [FromForm] string password)
        {
            try
            {
                string hashedPassword = PasswordHasher.Hash(password);
                User? AuthUser = databaseManager.Users
                    .Where(x => x.Login == login && x.Password == hashedPassword)
                    .FirstOrDefault();
                if (AuthUser == null)
                {
                    return StatusCode(401);
                }
                else
                {
                    string Token = JwtToken.Generate(AuthUser);
                    AuthUser.LastAuth = DateTime.Now;
                    databaseManager.SaveChanges();
                    return Ok(new { token = Token });
                }
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
