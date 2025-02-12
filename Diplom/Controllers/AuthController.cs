using Diplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Npgsql;
using System.Text;
using System.Security.Cryptography;

namespace Diplom.Controllers
{
    public class AuthController : Controller
    {
        #region
        private bool IsLoginExists(string login)
        {
            //добавить подключение
            using (var con = new NpgsqlConnection("Server=localhost;Port=5432;Username=postgres;Password=18082004;Database=users_db"))
            {
                con.Open();
                using (var command = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM users WHERE login = @login)", con))
                {
                    command.Parameters.AddWithValue("@login", login);
                    return (bool)command.ExecuteScalar();
                }
            }
        }
        private byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return hashBytes;
            }
        }
        private bool IsPasswordCorrect(byte[] password)
        {
            using (var con = new NpgsqlConnection("Server=localhost;Port=5432;Username=postgres;Password=18082004;Database=users_db"))
            {
                con.Open();
                using (var command = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM users WHERE password = @password)",con))
                {
                    command.Parameters.AddWithValue("@password", password);
                    return (bool)command.ExecuteScalar();
                }
            }
        }
        #endregion
        //протестировать

        [HttpGet]
        public IActionResult Registration()
        {
            return View("~/Views/Auth/Registration.cshtml");
        }
        [HttpPost]
        public IActionResult Registration(RegistrationModel data)
        {
            if (!ModelState.IsValid)
                return Content("Заполните данные!");
            else 
            {
                if (IsLoginExists(data.Login))
                    return Content("Логин уже занят!");
                else
                {
                    using (var con = new NpgsqlConnection("Server=localhost;Port=5432;Username=postgres;Password=18082004;Database=users_db"))
                    {
                        con.Open();
                        using (var command = new NpgsqlCommand("INSERT INTO users(login, password) VALUES(@Login, @Password)", con))
                        {
                            byte[] hashedPassword = HashPassword(data.Password);
                            command.Parameters.AddWithValue("login", data.Login);
                            command.Parameters.AddWithValue("password", hashedPassword);
                            command.ExecuteNonQuery();
                        }

                    }
                }
                
                return View("~/Views/AINetwork/Scan.cshtml");
            }
                
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }
        [HttpPost]
        public IActionResult Login(LoginModel data)
        {
            using (var con = new NpgsqlConnection("Server=localhost;Port=5432;Username=postgres;Password=18082004;Database=users_db"))
            {
                if (!ModelState.IsValid)
                {
                    return Content("Заполните Данные!");
                }
                else
                {
                    if (IsLoginExists(data.Login))
                    {
                        if (IsPasswordCorrect(HashPassword(data.Password)))
                        {
                            return View("~/Views/AINetwork/Scan.cshtml");
                        }
                        else
                        {                    
                            return Content("Неверный пароль!");
                        }
                    }
                    else
                    {
                        return Content("Неверный логин!");
                    }
                }
            }
        }
    }
}
