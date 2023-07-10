using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public abstract class person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public byte Age { get; set; }
        public int Phone { get; set; }
        
    }
}
