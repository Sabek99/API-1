using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class Review
{
    public User Reviewer { get; set; }
    public int ReviewerId { get; set; }

    public User Reviewee { get; set; }
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