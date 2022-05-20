using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class Interest
{
    public virtual int UserId {set; get;}
    public  virtual  User User { set; get; }
    public virtual int TagId {set; get;}
    public  virtual  Tag Tag { set; get; }
}