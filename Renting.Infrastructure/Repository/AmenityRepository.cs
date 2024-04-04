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
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext _db;

        public AmenityRepository(ApplicationDbContext db) :base(db) 
        {
            _db = db;
        }
       

        public void Update(Amenity entity)
        {
            _db.Amenities.Update(entity);
        }
    }
}
