using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository
{
    public class CapturedTimeRepository: Repository<CapturedTime>, ICapturedTimeRepository
    {
        public CapturedTimeRepository(OrionContext context) : base(context)
        {
        }
        public OrionContext OrionContext
        {
            get { return Context as OrionContext; }
        }
    }
}
