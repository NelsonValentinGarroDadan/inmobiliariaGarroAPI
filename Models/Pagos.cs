using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Pagos
{
    [Key]
    public int Id {get; set;}
    public int NroPago {get; set;}
    public int AlquilerId {get; set;}

    [ForeignKey("AlquilerId")] 
    public Alquileres Alquiler {get; set;}
    public DateTime Fecha{get; set;}
    public decimal Importe {get; set;}
    
    
}