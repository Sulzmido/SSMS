﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstitutionsAPI.Core.Models
{
    public class Institution
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
