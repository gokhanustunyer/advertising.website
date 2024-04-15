using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.data.Contexts;
using tatil.entity.PageEntities;

namespace tatil.business.Concrete.Service
{
    public class FormService : IFormService
    {
        private readonly TatilDbContext _context;

        public FormService(TatilDbContext tatilDbContext)
        {
            _context = tatilDbContext;
        }

        public List<SupportFormModel> GetAllContactForms()
            => _context.SupportMails.OrderByDescending(s => s.CreateDate).ToList();

        public async Task<bool> SaveContactForm(SupportFormModel formModel)
        {
            formModel.CreateDate = DateTime.Now;
            formModel.UpdateDate = DateTime.Now;
            formModel.Id = Guid.NewGuid();
            await _context.SupportMails.AddAsync(formModel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
