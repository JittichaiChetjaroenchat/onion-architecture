using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Customer
    {
        [Key]
        [Column(Constants.Customer.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.Customer.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.Customer.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(Constants.Customer.Column.Name, TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(Constants.Customer.Column.Age)]
        public int Age { get; set; }
    }
}