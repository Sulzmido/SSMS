using InstitutionsAPI.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManager.ViewModels
{
    public class SubjectCreateViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public Level Level { get; set; }
    }
}
