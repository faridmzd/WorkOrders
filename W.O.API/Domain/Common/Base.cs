using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W.O.API.Domain.Common
{

    public abstract class Base
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}