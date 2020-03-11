using InstitutionsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionsAPI.DAL
{
    public class StudentDAO : BaseDAO<Student>
    {        
        public StudentDAO(string tableName = "Students") : base(tableName)
        {
        }

        public new async Task<Student> InsertAsync(Student student)
        {
            string sex = student.Sex;

            if(sex != null && !sex.Equals(Gender.Male.ToString()) && !sex.Equals(Gender.Female.ToString()))
            {
                throw new Exception("Invalid Sex Value");
            }
            
            string status = student.Parent?.MarriageStatus;

            if(status != null && !status.Equals(Status.Married.ToString()) && !status.Equals(Status.Seperated.ToString()))
            {
                throw new Exception("Invalid MarriageStatus Value");
            }

            var result = await base.InsertAsync(student);
            return result;
        }

        public new async Task<Student> FindAsync(int ID)
        {
            var result = await base.FindAsync(ID);
            return result;
        }

        public new IEnumerable<Student> GetAll()
        {
            var result = base.GetAll();
            return result;
        }

        public new async Task UpdateAsync(Student student)
        {
            await base.UpdateAsync(student);
        }

        public new async Task DeleteAsync(Student student)
        {
            await base.DeleteAsync(student);
        }
    }
}
