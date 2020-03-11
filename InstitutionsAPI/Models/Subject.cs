using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Models
{

    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public object Category { get; set; }
    }
}
