using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Model
{
    [Table("Lawyer")]
    public class Lawyer : Entity
    {
        [Key]
        [Column("lawyer_id")]
        public override int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("surname")]
        public string Surname { get; set; }

        [MaxLength(4)]
        [Column("initials")]
        public string Initials { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [ForeignKey("Gender")]
        [Column("gender")]
        public int GenderRefId { get; set; }
        public Gender Gender { get; set; }

        [ForeignKey("Title")]
        [Column("title")]
        public int? TitleRefId { get; set; }
        public Title Title { get; set; }

        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; }

        [Column("active")]
        public bool? Active { get; set; }

    }
}