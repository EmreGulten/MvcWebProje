using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcWebProje.Models.Model
{
    [Table("Iletisim")]
    public class Iletisim
    {
        [Key]
        public int IletisimId { get; set; }
        [StringLength(250,ErrorMessage = "'max 250 karakter")]
        public string Adres { get; set; }
        [StringLength(25, ErrorMessage = "'max 25 karakter")]
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Whatsapp { get; set; }
        public string Facebook { get; set; }
        public string Tiwitter { get; set; }
        public string Instagram { get; set; }
    }
}