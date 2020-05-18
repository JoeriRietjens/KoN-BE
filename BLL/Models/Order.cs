using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL
{
   public class Order
   {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
   }
}
