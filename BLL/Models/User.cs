using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Order> Orders { get; set; }
    }
}