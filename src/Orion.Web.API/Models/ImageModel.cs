﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.Web.API.Models
{
    public class ImageModel
    {
        public IFormFile file { get; set; }
    }
}
