using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItConsultations.Business.Entities.Event;

public class Event : Entity<long>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Entity<long> Assignee { get; set; }
    public DateTime BeginDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
