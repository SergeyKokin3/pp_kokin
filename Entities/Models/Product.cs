using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Product
    {
        [Column("ProductId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")] 
        public string Name { get; set; }
        [Required(ErrorMessage = "Quantity is a required field.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Section is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the section is 20 characters.")]
        public double Price { get; set; }
        [ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
