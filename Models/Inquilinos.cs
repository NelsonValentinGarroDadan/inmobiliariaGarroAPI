using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI.Models;
public class Inquilinos
{
    [Key]
    public int Id {get; set;}
    public int PersonaId {get; set;}

    [ForeignKey("PersonaId")]
    public Personas Persona {get; set;}
    public string Longitud {get; set;}
    public string Latitud {get; set;}
}