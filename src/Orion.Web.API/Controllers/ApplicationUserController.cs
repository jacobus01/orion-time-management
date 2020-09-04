using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        ApplicationSettings _appSettings;

        public ApplicationUserController(IUnitOfWork uow)
        {
            _uow = uow;
            ApplicationSettings settings = new ApplicationSettings();

            //TODO fix bug preventing settings issue
            settings.JWT_Secret = "1234567890123456";
            settings.Client_URL = "http://localhost:4200";
            _appSettings = settings;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize]
        //POST : /api/ApplicationUser/CreateUser
        public IActionResult PostApplicationUser(ApplicationUserModel model)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));
            //We need to determine if this is an add or update action
            var applicationUser = new User()
            {
                Id = model.Id,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = Cipher.Encrypt(model.Password, Cipher.orionSalt),
                ChangePasswordOnNextLogin = model.ChangePasswordOnNextLogin,
                EmployeeNumber = model.EmployeeNumber,
                IsActive = model.IsActive,
                LockoutEnabled = model.LockoutEnabled,
                AppointmentDate = DateTime.Today,
                RoleId = model.RoleId,
                AccessGroupId = model.AccessGroupId
            };

            try
            {
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
        [Route("ProfilePic")]
        //GET : /api/ApplicationUser/ProfilePic
        public async Task<IActionResult> GetProfile([FromQuery]int id)
        {
            var user = _uow.Users.GetById(id);
            return File(user.ProfilePicture, "image/jpeg");
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
                    _uow.Users.Add(user);
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

