using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.DAL.Repository.Interfaces;

namespace Orion.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ImageController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{id}")]
        //GET : /api/ApplicationUser/ProfilePic
        public IActionResult ProfilePic([FromQuery] int id)
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
