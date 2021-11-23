using System.Collections.Generic;

/**
 * Well I am not going to defend the modeling of the data because 
 * neither me convinces me, but since it is a technical test and 
 * it was over time I left it like that, but this does not mean 
 * that in the future I can get to refactor it.
 */

namespace Climate.Models
{
    public class History
    {
        public int Id { get; set; }
        public virtual ICollection<Location> Location { get; set; } = new List<Location>();
    }

    
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual History History { get; set; }
    }
}