using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datapac_Library_Solution.Models;

public class Book
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string Author { get; set; }
}