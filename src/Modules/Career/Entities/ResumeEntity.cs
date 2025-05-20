using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class ResumeEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public float Salary { get; set; }
    public byte SalaryRule { get; set; }
    public byte JobType { get; set; }
    public byte OnAnytime { get; set; }
    public int WorkDate { get; set; }
    public byte WeeklyDays { get; set; }
    public byte EmployPeriod { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}