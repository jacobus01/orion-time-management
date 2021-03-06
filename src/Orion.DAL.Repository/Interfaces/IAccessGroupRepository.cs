﻿using Orion.DAL.EF.Models.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository.Interfaces
{
    public interface IAccessGroupRepository: IRepository<AccessGroup>
    {
        AccessGroup GetByUserId(int userId);
    }
}
