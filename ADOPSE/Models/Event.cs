using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class Event
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string GoogleCalendarID { get; set; }
    public Module Module { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public DateTime Starts { get; set; }
    public DateTime Ends { get; set; }
}