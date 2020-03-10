using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstitutionsAPI.Core.Models
{
    public enum Gender{
        Male, Female
    }

    public enum Status
    {
        Married, Seperated
    }
    public class Student
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Gender")]

        public Gender Sex { get; set; }// male or female
        
        public string Age { get; set; }

        public string Religion { get; set; }

        [Display(Name = "Date Of Birth")]

        public string DateOfBirth { get; set; }// student date of birth

        #region // from this region we may have to consider if its better to have a db for parents and just tie something unique to identify whose parent each person is.
        [Display(Name = "Parent's Name")]
        
        public string NameOfParents { get; set; }// a particular parent, not both

        [Display(Name = "Parent's Birth Date")]

        public string ParentDOB { get; set; }// Just month and Day not year
        
        [Display(Name = "Parent's Occupation")]

        public string ParentOccupation { get; set; } // just for information

        [Display(Name = "State Of Origin")]

        public string StateOfOrigin { get; set; } // child's state of origin

        [Display(Name = "Marital Status")]

        public Status MarriageStatus { get; set; }// married or seperated

        [Display(Name = "Parent's Number")]

        public string ParentNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
        #endregion tings like address and state of origin contained in this region can also be left in the student table. 



    }
}
