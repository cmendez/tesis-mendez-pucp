using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("Imagenes")]
    public class Imagen
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ImagenId { get; set; }
        public byte[] Data { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Mime { get; set; }
    }
}