using InstitutionsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            // POST : Validate all enums, extract id from sub-entities.
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
            // Enum Validation , extract id from sub-entities.
            string sex = student.Sex;

            if (sex != null && !sex.Equals(Gender.Male.ToString()) && !sex.Equals(Gender.Female.ToString()))
            {
                throw new Exception("Invalid Sex Value");
            }

            string status = student.Parent?.MarriageStatus;

            if (status != null && !status.Equals(Status.Married.ToString()) && !status.Equals(Status.Seperated.ToString()))
            {
                throw new Exception("Invalid MarriageStatus Value");
            }

            var entityToUpdate = await FindAsync(student.ID);

            PropertyInfo[] props = typeof(Student).GetProperties();

            foreach(PropertyInfo prop in props)
            {
                if(prop.Name == "ID")
                {
                    continue;
                }

                // Handling Sub-Objects
                if(prop.Name == "Parent")
                {
                    var value = prop.GetValue(student);

                    if(value != null)
                    {
                        foreach (PropertyInfo property in typeof(Parent).GetProperties())
                        {
                            var parentPropertyValue = property.GetValue(student.Parent);

                            if (parentPropertyValue != null)
                            {
                                property.SetValue(entityToUpdate.Parent, parentPropertyValue);
                            }
                        }
                    } 
                }

                else
                {
                    var value = prop.GetValue(student);

                    if (value != null)
                    {
                        prop.SetValue(entityToUpdate, value);
                    }
                }                    
            }            

            await base.UpdateAsync(entityToUpdate);
        }

        public new async Task DeleteAsync(Student student)
        {
            await base.DeleteAsync(student);
        }
    }
}
