using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class Review
{
    public virtual User User { get; set; }
    public int UserId { get; set; }

    public virtual User Reviewee { get; set; }
    public int RevieweeId { get; set; }
    public string ReviewBody { get; set; }
    public DateTime CreationTime { get; set; }
    
    public enum Stars
    {
        Star1=1,
        Star2=2,
        Star3=3,
        Star4=4,
        Star5=5
    }
}