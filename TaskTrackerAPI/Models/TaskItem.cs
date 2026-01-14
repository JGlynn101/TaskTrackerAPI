
using System.Runtime.InteropServices.Swift;

namespace TaskTrackerAPI
{
    
   public class TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Todo";

    public TaskItem() {} 

    public TaskItem(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
}