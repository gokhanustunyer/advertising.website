using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.PageEntities;

namespace tatil.business.Abstract.Service
{
    public interface IFormService
    {
        Task<bool> SaveContactForm(SupportFormModel formModel);
        List<SupportFormModel> GetAllContactForms();
    }
}
