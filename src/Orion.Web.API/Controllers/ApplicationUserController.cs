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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Orion.Common.Library.Encryption;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository;
using Orion.DAL.Repository.Interfaces;
using Orion.Web.API.Models;
using Orion.Web.API.Interfaces;

namespace Orion.Web.API.Controllers
{
    //You must use attribute routing for any controllers that you want represented in your Swagger document(s):
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        IHostingEnvironment _hostingEnvironment;
        private ApplicationSettings _appSettings;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public ApplicationUserController(IUnitOfWork uow, IHostingEnvironment hostingEnvironment, IConfiguration config, ITokenService tokenService)
        {
            _uow = uow;
            _config = config;
            ApplicationSettings settings = new ApplicationSettings();

            //TODO fix bug preventing settings issue
            settings.JWT_Secret = _config.GetSection("ApplicationSettings").GetSection("JWT_Secret").Value;
            settings.Client_URL = _config.GetSection("ApplicationSettings").GetSection("Client_URL").Value;
            settings.AllowedServiceURL = _config.GetSection("ApplicationSettings").GetSection("AllowedServiceURL").Value;
            _appSettings = settings;
            _hostingEnvironment = hostingEnvironment;
            _tokenService = tokenService;
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
                var result = _uow.Users.GetAllNotDeleted();
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
                var claims = new List<Claim>();
                claims.Add(new Claim("UserID", user.Id.ToString()));


                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);
                return Ok(new
                {
                    token = accessToken,
                    refreshToken = refreshToken,
                    user = user
                });
            }
            else
                return Unauthorized();
               //return BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var claim = principal.Claims.First<Claim>(); //this is mapped to the UserID claim by default
            var userId = Int32.Parse(claim.Properties["UserID"].ToString());
            var user = _uow.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _uow.Complete();
            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var claim = User.Claims.First<Claim>(); //this is mapped to the UserID claim by default
            var userId = Int32.Parse(claim.Properties["UserID"].ToString());
            var user = _uow.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _uow.Complete();
            return NoContent();
        }


    [HttpGet("{idnt}")]
        //GET : /api/ApplicationUser/ProfilePic
        public IActionResult HasProfilePic([FromQuery] int idnt)
        {
            var user = _uow.Users.GetById(idnt);
            if (user.ProfilePicture != null)
            {
                return Ok(new { message = true });
            }
            else
            {
                return Ok(new { message = false });
            }
        }

        [HttpPost]
        [Route("DeleteUser")]
        [Authorize]
        //POST : /api/ApplicationUser/CreateUser
        public IActionResult DeleteApplicationUser([FromBody] int UserId)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));
            //We need to determine if this is an add or update action


            var applicationUser = _uow.Users.SingleOrDefault(u => u.Id == UserId);

            try
            {
                _uow.Users.Remove(applicationUser);
                _uow.Complete();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("UserNameUnused")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult IsUserNameUnused(object response)
        {
            var username = response.ToString();
            try
            {
                var result = _uow.Users.IsUnusedUserName(username);
                return Ok(new { IsUserNameUnused = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("TotalEmployees")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult TotalUsers()
        {
            try
            {
                var result = _uow.Users.GetTotal();
                return Ok(new { TotalEmployees = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("EmployeesWithSubs")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult UsersWithSubs()
        {
            try
            {
                var result = _uow.Users.GetAllWithSubs();
                var newList = new List<dynamic>();
                foreach (var user in result)
                {
                    newList.Add(new
                    {
                        Id = user.Id,
                        RoleId = user.Role.Id,
                        RoleName = user.Role.RoleName,
                        AccessGroupId = user.AccessGroup.Id,
                        AccessGroupName = user.AccessGroup.AccessGroupName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfilePicUploaded = user.ProfilePicture != null,
                        IsActive = user.IsActive
                    });
                }
                return Ok( newList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}

