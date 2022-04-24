﻿using System.Reflection.Metadata.Ecma335;

namespace WebApi.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    //navigation properties 
    public ICollection<Question> Questions { get; set; }
}