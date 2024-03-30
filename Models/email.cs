using Microsoft.Build.Framework;

namespace TimeTrace.Models;

public class email
{
    [Required] public string Subject { get; set; }
    [Required] public string Message { get; set; }

}