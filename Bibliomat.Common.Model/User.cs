using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliomat.Common.Model
{
    public partial class User
    {
        public User()
        {
            Book = new HashSet<Book>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Pass { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Book> Book { get; set; }
    }
}
