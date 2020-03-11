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

        // [ Class ]
        public object Class { get; set; }

        // csv [ Subject ] [ ID's]
        public string Subjects { get; set; }
    }
}
