using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Identity.Create
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "E-Posta Alanını Boş Geçemezsiniz"), MinLength(7, ErrorMessage = "Geçersiz Uzunlukta E-Posta Adresi")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Telefon Numarası Alanını Boş Geçemezsiniz"), MinLength(3, ErrorMessage = "Geçersiz Uzunlukta Telefon Numarası")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "İsim Alanını Boş Geçemezsiniz"), MinLength(2, ErrorMessage = "Geçersiz Uzunlukta İsim")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Soyisim Alanını Boş Geçemezsiniz"), MinLength(2, ErrorMessage = "Geçersiz Uzunlukta Soyisim")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Şifre Alanını Boş Geçemezsiniz"), MinLength(7, ErrorMessage = "Şifreniz minimum 8 karakterden oluşmalıdır") ]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Şifre (Tekrar) Alanını Boş Geçemezsiniz"), MinLength(7, ErrorMessage = "Şifreniz minimum 8 karakterden oluşmalıdır")]
        [Compare("Password", ErrorMessage = "Şifreleriniz Uyuşmuyor")]
        public string PasswordAgn { get; set; }
    }
}
