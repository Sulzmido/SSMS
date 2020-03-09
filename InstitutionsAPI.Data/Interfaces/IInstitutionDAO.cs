using InstitutionsAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.Interfaces
{
    public interface IInstitutionDAO
    {
        Institution Create(Institution institution);

        Institution Retrieve(int ID);

        Institution Update(Institution institution);

        int Delete(Institution institution);
    }
}
