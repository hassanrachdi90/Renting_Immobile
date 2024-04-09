using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renting.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price")] //Annotation
        [Range(10,10000)]
        public double Price { get; set; }
        [Range(1, 10)]
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        [Display(Name="Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; }=true;

    }
}
