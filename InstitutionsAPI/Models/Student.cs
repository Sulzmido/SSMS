using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Models
{
    public enum Gender
    {
        Male, Female
    }

    public enum Status
    {
        Married, Seperated
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Gender Sex { get; set; }

        public string Age { get; set; }

        public string Religion { get; set; }

        public string DateOfBirth { get; set; }

        public Parent Parent { get; set; }
    }

    public class Parent
    {
        public string NameOfParents { get; set; }// a particular parent, not both

        public string ParentDOB { get; set; }// Just month and Day not year

        public string ParentOccupation { get; set; } // just for information

        public string StateOfOrigin { get; set; } // child's state of origin

        public Status MarriageStatus { get; set; }// married or seperated

        public string ParentNumber { get; set; }

        public string Address { get; set; }
    }
}
