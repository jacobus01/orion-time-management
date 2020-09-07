using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orion.Common.Library.Encryption;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository;
using Orion.DAL.Repository.Interfaces;
using Orion.Web.API.Models;

namespace Orion.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        IHostingEnvironment _hostingEnvironment;
        ApplicationSettings _appSettings;

        public ApplicationUserController(IUnitOfWork uow, IHostingEnvironment hostingEnvironment)
        {
            _uow = uow;
            ApplicationSettings settings = new ApplicationSettings();

            //TODO fix bug preventing settings issue
            settings.JWT_Secret = "1234567890123456";
            settings.Client_URL = "http://localhost:4200";
            _appSettings = settings;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("CreateUpdateUser")]
        [Authorize]
        //POST : /api/ApplicationUser/CreateUser
        public IActionResult PostApplicationUser(ApplicationUserModel model)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));
            //We need to determine if this is an add or update action


            var applicationUser = model.Id != 0 ? _uow.Users.SingleOrDefault(u => u.Id == model.Id) : new User();


            applicationUser.UserName = model.UserName;
            applicationUser.Email = model.Email;
            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.PasswordHash = model.Id.HasValue ? Cipher.Encrypt(model.Password, Cipher.orionSalt) : applicationUser.PasswordHash;
            applicationUser.ChangePasswordOnNextLogin = model.ChangePasswordOnNextLogin;
            applicationUser.EmployeeNumber = model.EmployeeNumber;
            applicationUser.IsActive = model.IsActive;
            applicationUser.LockoutEnabled = model.LockoutEnabled;
            applicationUser.AppointmentDate = DateTime.Today;
            applicationUser.RoleId = model.RoleId;
            applicationUser.AccessGroupId = model.AccessGroupId;


            try
            {
                if (model.Id == 0)
                    _uow.Users.Add(applicationUser);
                _uow.Complete();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("Users")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetApplicationUserList()
        {
            try
            {
                var result = _uow.Users.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("User")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetApplicationUser([FromBody] int UserId)
        {
            try
            {
                var result = _uow.Users.GetById(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel model)
        {
            var user = _uow.Users.GetByEmail(model.Email);

            if (user != null && _uow.Users.CheckPassword(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token, user });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }
        [HttpGet("{id}")]
        [Route("HasProfilePic")]
        //GET : /api/ApplicationUser/ProfilePic
        public IActionResult HasProfile([FromQuery] int id)
        {
            var user = _uow.Users.GetById(id);
            if (user.ProfilePicture != null)
            {
                return Ok(new { message = true });
            }
            else
            {
                return Ok(new { message = false });
            }
        }

        [HttpGet("{id}")]
        [Route("ProfilePic")]
        //GET : /api/ApplicationUser/ProfilePic
        public IActionResult GetProfile([FromQuery]int id)
        {
            var user = _uow.Users.GetById(id);
            if (user.ProfilePicture != null)
            {
                return File(user.ProfilePicture, "image/jpeg");
            }
            else
            {
                return Ok(new { message = false });
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("UploadImage")]
        [Authorize]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var user = _uow.Users.GetById(Int32.Parse(file.FileName));
                if (file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        user.ProfilePicture = stream.ToArray();
                    }

                    //TODO:Just Check that the update takes place. For Unit testing to fix if there is an issue
                    _uow.Complete();

                    return Ok(new { result = "success" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}

