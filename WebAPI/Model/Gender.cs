using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebAPI.Commons;

namespace WebAPI.Model
{
    [Table("Gender")]
    public class Gender : Entity
    {
        [Key]
        [Column("gender_id")]
        public override int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("description")]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("description_m")]
        public string DescriptionM { get; set; }

        [Required]
        [Column("date_rec_started")]
        public DateTime DateRecStarted { get; set; }

        [Required]
        [Column("login_rec_started")]
        public DateTime LoginRecStarted { get; set; }

        [Column("date_rec_ended")]
        public DateTime? DateRecEnded { get; set; }

        [MaxLength(8)]
        [Column("login_rec_ended")]
        internal string _LoginRecEnded { get; set; }
        [NotMapped]
        public DateTime? LoginRecEnded
        {
            get => DateTimeStringConverter.ToDateTimeNullable(_LoginRecEnded);
            set => _LoginRecEnded = DateTimeStringConverter.ToString(value);
        }
    }
}