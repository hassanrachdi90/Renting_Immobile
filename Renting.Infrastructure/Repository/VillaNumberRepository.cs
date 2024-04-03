using Microsoft.EntityFrameworkCore;
using Renting.Application.Common.Interfaces;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Renting.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) :base(db) 
        {
            _db = db;
        }
       

        public void Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);
        }
    }
}
