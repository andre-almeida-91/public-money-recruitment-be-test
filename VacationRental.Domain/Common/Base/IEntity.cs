namespace VacationRental.Domain.Common.Base
{
    public interface IEntity
    {
        int Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }
    }
}
