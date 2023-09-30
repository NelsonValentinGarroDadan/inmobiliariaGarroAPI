using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaGarroAPI;
public class Inquilinos
{
    [Key]
    public int Id {get; set;}
    public int PersonaId {get; set;}

    [ForeignKey("PersonaId")]
    public Personas Persona {get; set;}
    public string Direccion {get; set;}
}