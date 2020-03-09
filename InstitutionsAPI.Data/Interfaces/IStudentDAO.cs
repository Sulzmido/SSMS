using InstitutionsAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.Interfaces
{
    public interface IStudentDAO
    {
        Student Create(Student student);

        Student Retrieve(int ID);

        Student Update(Student student);

        int Delete(Student student);
    }
}
