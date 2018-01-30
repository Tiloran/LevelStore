using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LevelStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public ICollection<CartLine> Lines { get; set; }
        public bool Shipped { get; set; }

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
    }
}
