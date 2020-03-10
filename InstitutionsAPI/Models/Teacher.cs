using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionsAPI.Models
{
    public class Teacher
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Level Level { get; set; }
        public Class Class { get; set; }

    }
}
