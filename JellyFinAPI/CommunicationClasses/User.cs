using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class User
    {
        public string? Name { get; set; }
        public string? ServerId { get; set;  }
        public string? Id { get; set; }
        public bool HasPassword { get; set; }
        public bool HasConfiguredPassword { get; set; }
        public bool HasConfiguredEasyPassword { get; set; }
        public bool EnableAutoLogin { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public Configuration? Configuration { get; set; }
        public Policy? Policy { get; set; }
    }
}
