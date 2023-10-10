using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Inmuebles 
{
    [Key]
    public int Id {get; set;}
    public string Longitud {get; set;}
    public string Latitud {get; set;}
    public int CAmbientes {get; set;}
    public string Tipo {get; set;}
    public string Uso {get; set;}
    public decimal Precio {get; set;}
    public bool Disponible {get; set;}
    public int PropietarioId {get; set;}

    [ForeignKey("PropietarioId")] 
    public Propietarios Propietario {get; set;}
    
}