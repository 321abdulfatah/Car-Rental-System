using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public abstract class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
                
    }
}
