using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Domain.Models;

public abstract class BaseModel
{
    public Guid ID { get;  set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    protected BaseModel () { }

    protected BaseModel(Guid Id) { ID = Id == Guid.Empty ? Guid.NewGuid() : Id; }
}

