using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Entities
{
    [Table("OfertasPromosDsctos")]
    public class OfertaPromoDscto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfertaPromoDsctoId { get; set; }
        public int SuministradorId { get; set; }
        public virtual Suministrador Suministrador { get; set; }
        public string Tipo { get; set; }
        public string NombreCorto { get; set; }
        public string NombreCompleto { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        //public virtual ICollection<Imagen> Imagenes { get; set; }
        
        public int? ImagenPrincipalId { get; set; }
        [ForeignKey("ImagenPrincipalId"), Column(Order = 0)]
        public virtual Imagen ImagenPrincipal { get; set; }
        
        public int? ImagenBannerId { get; set; }
        [ForeignKey("ImagenBannerId"), Column(Order = 1)]
        public virtual Imagen ImagenBanner { get; set; }

        public int IsAdquiribleConLeads { get; set; }
        public int CostoEnLeads { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IsVisible { get; set; }
        public int IsEliminado { get; set; }

        public virtual ICollection<CompraVirtual> ComprasVirtuales { get; set; }
    }
}