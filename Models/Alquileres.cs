using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Alquileres
{
    [Key]
    public int Id {get; set;}
    public decimal Precio {get; set;}
    public DateTime Fecha_Inicio {get; set;}
    public DateTime Fecha_Fin {get; set;}
    public int InquilinoId {get; set;}

    [ForeignKey("InquilinoId")] 
    public Inquilinos inquilino {get; set;}
    public int InmuebleId {get; set;}

    [ForeignKey("InmuebleId")] 
    public Inmuebles Inmueble {get; set;}
    
}