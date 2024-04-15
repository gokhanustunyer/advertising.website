using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.PageEntities
{
    public class SupportFormModel : BaseEntity.BaseEntity
    {
        [Required(ErrorMessage = "İsim Alanını Boş Geçemezsiniz"), MinLength(2, ErrorMessage = "Geçersiz Uzunlukta İsim")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Telefon Alanını Boş Geçemezsiniz"), MinLength(3, ErrorMessage = "Geçersiz Uzunlukta Telefon")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "E-Posta Alanını Boş Geçemezsiniz"), MinLength(7, ErrorMessage = "Geçersiz Uzunlukta E-Posta")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Konu Alanını Boş Geçemezsiniz"), MinLength(3, ErrorMessage = "Geçersiz Uzunlukta Konu")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Mesaj Alanını Boş Geçemezsiniz"), MinLength(3, ErrorMessage = "Geçersiz Uzunlukta Mesaj")]
        public string Messsage { get; set; }
    }
}
