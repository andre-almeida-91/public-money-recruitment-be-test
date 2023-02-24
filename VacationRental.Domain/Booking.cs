using System.ComponentModel.DataAnnotations;
using VacationRental.Domain.Common.Base;

namespace VacationRental.Domain
{
    public partial class Booking : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }     
    }
}
