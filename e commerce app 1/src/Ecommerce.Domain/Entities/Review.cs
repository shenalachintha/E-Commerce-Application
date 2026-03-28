namespace Ecommerce.Domain.Entities;

/// <summary>
/// Review entity for product reviews and ratings.
/// </summary>
public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; } = false;

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
