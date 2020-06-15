using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoffieOfNie.Controllers
{
    public class ProductDTO
    {
        public string username { get; set; }
        public product[] products { get; set; }
        public class product
        {
            public string coffeeType { get; set; }
            public string sugar { get; set; }
            public string milk { get; set; }
        }
    }
}
