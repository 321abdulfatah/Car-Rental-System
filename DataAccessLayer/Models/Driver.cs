﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Driver:Person
    {

        public double Salary { get; set; }

        public bool isAvailable { get; set; }
    }
}
