using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datapac_Library_Solution.Models;

public class Customer
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string FirstName { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string LastName { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; }
}