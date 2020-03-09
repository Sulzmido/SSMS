using InstitutionsAPI.Core.Models;
using InstitutionsAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.DAL
{
    public class InstitutionDAO : IInstitutionDAO
    {
        public Institution Create (Institution institution)
        {
            return new Institution() { };
        }

        public Institution Retrieve(int ID)
        {
            return new Institution() { };
        }

        public Institution Update(Institution institution)
        {
            return new Institution() { };
        }

        public int Delete(Institution institution)
        {
            return 0;
        }
    }
}
