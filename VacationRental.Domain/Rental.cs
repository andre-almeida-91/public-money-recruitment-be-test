using System.ComponentModel.DataAnnotations;
using VacationRental.Domain.Common.Base;

namespace VacationRental.Domain
{
    public partial class Rental : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
