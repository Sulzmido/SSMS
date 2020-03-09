using InstitutionsAPI.Core.Models;
using InstitutionsAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.DAL
{
    public class StudentDAO : IStudentDAO
    {
        public Student Create(Student student)
        {
            return new Student { Name = "Ahmed Sulaiman" };
        }

        public int Delete(Student student)
        {
            return 0;
        }

        public Student Retrieve(int ID)
        {
            return new Student { Name = "Ahmed Sulaiman" };
        }

        public Student Update(Student student)
        {
            return new Student { Name = "Ahmed Sulaiman" };
        }
    }
}
