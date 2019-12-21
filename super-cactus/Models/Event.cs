using System;

namespace super_cactus.Models
{
    public struct Event
    {
        public string Type { get; set; }
        
        public string ClassName { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ulong ServerId { get; set; }
    }
}