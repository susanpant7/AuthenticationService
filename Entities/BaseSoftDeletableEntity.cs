namespace AuthenticationSystem.Entities;

public abstract class BaseSoftDeletableEntity : BaseAuditableEntity, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}