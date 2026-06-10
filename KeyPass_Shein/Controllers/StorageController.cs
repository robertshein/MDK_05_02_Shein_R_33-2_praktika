using KeyPass_Shein.Classes;
using KeyPass_Shein.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Shein.Controllers
{
    [Route("/storage")]
    public class StorageController : Controller
    {
        private DatabaseManager databaseManager;

        public StorageController() =>
            this.databaseManager = new DatabaseManager();

        [Route("get")]
        [HttpGet]
        public ActionResult Get([FromHeader] string token)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                if (IdUser == null)
                    return StatusCode(401);

                List<StorageDto> Storages = databaseManager.Storages
                    .Where(x => x.User.Id == IdUser)
                    .Select(s => new StorageDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Url = s.Url,
                        Login = s.Login,
                        Password = PasswordEncryptor.Decrypt(s.Password),
                    })
                    .ToList();
                return Ok(Storages);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        public ActionResult Add([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                if (IdUser == null)
                    return StatusCode(401);

                storage.Password = PasswordEncryptor.Encrypt(storage.Password);

                storage.User = databaseManager.Users
                    .Where(x => x.Id == IdUser)
                    .First();

                databaseManager.Add(storage);
                databaseManager.SaveChanges();

                storage.User = null;
                storage.Password = PasswordEncryptor.Decrypt(storage.Password);
                return StatusCode(200, storage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        [Route("update")]
        [HttpPut]
        public ActionResult Update([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                Storage? uStorage = databaseManager.Storages
                    .Where(x => x.Id == storage.Id)
                    .FirstOrDefault();
                if (IdUser == null)
                    return StatusCode(401);
                if (uStorage == null)
                    return StatusCode(404);

                uStorage.Name = storage.Name;
                uStorage.Url = storage.Url;
                uStorage.Login = storage.Login;
                uStorage.Password = PasswordEncryptor.Encrypt(storage.Password);

                databaseManager.SaveChanges();
                storage.User = null;
                return StatusCode(200, storage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public ActionResult Delete([FromHeader] string token, [FromForm] int id)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                Storage? Storage = databaseManager.Storages
                    .Where(x => x.Id == id && x.User.Id == IdUser)
                    .FirstOrDefault();
                if (IdUser == null)
                    return StatusCode(401);
                if (Storage == null)
                    return StatusCode(404);

                databaseManager.Storages.Remove(Storage);
                databaseManager.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
