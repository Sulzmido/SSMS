﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Core.Models
{
    public class Class
    {
        public int ID { get; set; }

        public string Name { get; set; }

        // [ Level ]
        public Level Level { get; set; }

    }
}
