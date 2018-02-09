using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LevelStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public IList<CartLine> Lines { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Введите телефон")]
        public string Phone { get; set; }

        public string NovaPoshta { get; set; }
        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }
        
        public string Address { get; set; }

        public string Comment { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfCreation { get; set; }
    }
}
