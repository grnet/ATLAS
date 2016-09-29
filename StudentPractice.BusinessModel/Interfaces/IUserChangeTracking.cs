using System;

namespace StudentPractice.BusinessModel
{
    internal interface IUserChangeTracking
    {
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedAtDateOnly { get; set; }
    }
}
