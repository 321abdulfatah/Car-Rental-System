﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Customer:Person
    {
        public ICollection<Car> Cars { get; set; }

    }
}
