using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcWebProje.Models.Model
{
    [Table("Kimlik")]
    public class Kimlik
    {
        public int KimlikId { get; set; }
        [DisplayName("Site Başlık")]
        [Required,StringLength(100,ErrorMessage = "Max 100 karakter")]
        public string Title { get; set; }
        [DisplayName("Anahtar Kelimeler")]
        [Required, StringLength(200, ErrorMessage = "Max 200 karakter")]
        public string Keywords { get; set; }
        [DisplayName("Site Açıklama")]
        [Required, StringLength(300, ErrorMessage = "Max 300 karakter")]
        public string Description { get; set; }
        [DisplayName("Site Logo")]
        public string LogoURL { get; set; }
        [DisplayName("Site Unvan")]
        public string Unvan { get; set; }
    }
}