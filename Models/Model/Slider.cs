﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcWebProje.Models.Model
{
    [Table("Slider")]
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }
        [DisplayName("Slider Başlık"),StringLength(30,ErrorMessage = "Max 30 karakter")]
        public string Baslik { get; set; }
        [DisplayName("Slider Başlık"), StringLength(150, ErrorMessage = "Max 150 karakter")]
        public string Aciklama { get; set; }
        [DisplayName("Slider Başlık"), StringLength(250)]
        public string ResimURL { get; set; }
    }
}