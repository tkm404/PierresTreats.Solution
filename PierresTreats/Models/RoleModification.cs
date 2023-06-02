using System.ComponentModel.DataAnnotations;

namespace PierresTreats.Models
{
  public class RoleModification
  {
    [Required]
    public string RoleName { get; set; }
    #nullable enable
    public string[]? AddIds { get; set; }
    public string[]? DeleteIds { get; set; }
    #nullable disable
  }
}