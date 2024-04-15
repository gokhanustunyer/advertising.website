using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Advert.Create
{
    public class CreateAdvertReference
    {
        public string? userName { get; set; }
        [Required(ErrorMessage = "Telefon Alanı Zorunludur"), MinLength(3, ErrorMessage = "Geçersiz Telefon Uzunluğu")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Başlık Alanı Zorunludur"), MinLength(3, ErrorMessage = "Başlık Uzunluğu Üç Karakterden Büyük Olmalıdır")]
        public string? Title { get; set; }
        public string? Url { get; set; }
        [Required(ErrorMessage = "Fiyat Alanı Zorunludur")]
        public double? Price { get; set; }
        [Required(ErrorMessage = "Boyut Alanı Zorunludur")]
        public int? Size { get; set; }
        [Required(ErrorMessage = "İnsan Kapasitesi Alanı Zorunludur")]
        public int? HumanCapacity { get; set; }
        [Required(ErrorMessage = "Yatak Kapasitesi Alanı Zorunludur")]
        public int? BedCapacity { get; set; }
        [Required(ErrorMessage = "İl Alanı Zorunludur")]
        public string? City { get; set; }
        [Required(ErrorMessage = "İlçe Alanı Zorunludur")]
        public string? District { get; set; }
        [Required(ErrorMessage = "Mahalle Alanı Zorunludur")]
        public string? Neighborhood { get; set; }
        [Required(ErrorMessage = "Kısa Açıklama Alanı Zorunludur"), MinLength(3, ErrorMessage = "Kışa Açıklama Uzunluğu Üç Karakterden Büyük Olmalıdır")]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "Açıklama Alanı Zorunludur"), MinLength(3, ErrorMessage = "Açıklama Uzunluğu Üç Karakterden Büyük Olmalıdır")]
        public string? Description { get; set; }
        public List<string>? FilterIds { get; set; }
        [Required(ErrorMessage = "En Az Bir Kategori Seçmelisiniz")]
        public List<string>? CategoryIds { get; set; }
        [Display(Name = "Image")]
        [Required(ErrorMessage = "Lütfen İlanınız İçin Resim Seçin")]
        public IFormFileCollection PostedFiles { get; set; }
        public IFormFileCollection? DescFiles { get; set; }
    }
}
