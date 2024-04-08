using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Models;

[Table("Books")]
public class Book
{
    [Key]
    public int Id { get; set; }
    public string BookName { get; set; } = string.Empty;
    public string Assessment { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Pages { get; set; }
    public bool Concluded { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
