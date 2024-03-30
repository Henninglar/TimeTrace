using System.ComponentModel.DataAnnotations;

namespace TimeTrace.Models;

public class Export
{
    [Microsoft.Build.Framework.Required] public string ProjectToSearch { get; set; }
    [DataType(DataType.Date)]
    public DateOnly From { get; set; }
  
    
    [DataType(DataType.Date)]
    public DateOnly To { get; set; }
}