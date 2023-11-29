using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTeams.Data
{
    public class DevTeam
    {
        public DevTeam() {}
        public DevTeam(string teamName, List<Developer> teamMembers) 
        {
            TeamName = teamName;
            TeamMembers = teamMembers;
        }
        public string TeamName {get; set;} = string.Empty;
        public List<Developer> TeamMembers { get; set; } = new List<Developer>();
        public int TeamID {get; set;}
    }
}