using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DevTeams.Data;

namespace DevTeams.Repository
{
    public class DevTeamRepository
    {
        private readonly DevRepository _devRepository = new DevRepository();
        private readonly List<DevTeam> _teams = new List<DevTeam>();

        private int _count = 0;
        public DevTeamRepository()
        {
            _devRepository = new DevRepository();
            _teams = new List<DevTeam>();
        }


        private DevTeam GetOrCreateTeam(string teamName)
        {
            // Check if the team already exists in the list
            DevTeam existingTeam = _teams.Find(t => t.TeamName == teamName)!;

            if (existingTeam != null)
            {
                return existingTeam;
            }
            else
            {
                System.Console.WriteLine($"this team does not exist, would you like to create a new team with the name {teamName}? (yes/no) ");
                string userInput = Console.ReadLine()!.ToLower();
                switch (userInput)
                {
                    case "yes":
                        CreateTeam(teamName);
                        return _teams.Find(t => t.TeamName == teamName)!;

                    default:
                        System.Console.WriteLine("moving on");
                        return null!;
                }
            }

        }
        public DevTeam CreateTeam(string teamName)
        {

            System.Console.WriteLine($"are you sure you want to create a new team with the name {teamName}? (yes/no) ");
            string userInput = Console.ReadLine()!.ToLower();
            switch (userInput)
            {
                case "yes":
                    // create team
                    DevTeam newTeam = new DevTeam { TeamName = teamName, TeamMembers = new List<Developer>() };
                    _teams.Add(newTeam);
                    System.Console.WriteLine($"{teamName} has been added to list of teams");
                    return newTeam;
                case "no":
                    System.Console.WriteLine("moving on then");
                    return null!;
                default:
                    System.Console.WriteLine($"{userInput} is not a valid input");
                    return null!;
            }
        }

        public void DeleteTeam(string teamName)
        {
            DevTeam teamToDelete = _teams.Find(t => t.TeamName == teamName)!;

            System.Console.WriteLine($"are you sure you want to delete a team with the name {teamName}? ");
            string userInput = Console.ReadLine()!.ToLower();
            if (teamToDelete != null)
            {
                bool running = true;
                while (running)
                {
                    switch (userInput)
                    {
                        case "yes":
                            _teams.Remove(teamToDelete);
                            running = false;
                            break;
                        case "no":
                            System.Console.WriteLine("moving on");
                            running = false;
                            break;
                        default:
                            System.Console.WriteLine("please input either yes or no");
                            break;
                    }
                }
            }
            else
            {
                System.Console.WriteLine($"Team '{teamName}' not found.");
            }
        }

        public void ListTeams()
        {
            foreach (DevTeam team in _teams)
            {
                System.Console.WriteLine(team);
            }
        }
        public void UpdateTeam(string oldTeamName, string newTeamName)
        {
            // The t represents each individual team in the list of _teams,
            // and t.TeamName == oldTeamName is a condition that checks if the name of the team (TeamName) is equal to the oldTeamName you provided.
            DevTeam teamToUpdate = _teams.Find(t => t.TeamName == oldTeamName)!;

            if (teamToUpdate != null)
            {
                teamToUpdate.TeamName = newTeamName;
                System.Console.WriteLine($"successfully updated {oldTeamName} to {newTeamName}");
            }
            else
            {
                System.Console.WriteLine($"No team found with the name {oldTeamName}");
            }
        }
        public void AddDeveloperToTeam(string teamName, int developerId)
        {
            // Get the team from the list or create a new one if it doesn't exist
            DevTeam team = GetOrCreateTeam(teamName);

            // Get the Developer object from the repository using the ID
            Developer developer = _devRepository.GetDeveloper(developerId);

            // Add the developer to the team's list of members
            team.TeamMembers.Add(developer);
        }


        public void DeleteDeveloperFromTeam(string teamName, int developerId)
        {
            // Find the team with the given name in the list
            DevTeam team = GetOrCreateTeam(teamName);

            if (team != null)
            {
                // Check if the developer with the given ID is in the team
                int indexOfDeveloper = team.TeamMembers.FindIndex(dev => dev.ID == developerId);

                if (indexOfDeveloper != -1)
                {
                    team.TeamMembers.RemoveAt(indexOfDeveloper);
                }
                else
                {
                    Console.WriteLine($"Developer with ID {developerId} not found in the team.");
                }
            }
            else
            {
                Console.WriteLine($"Team {teamName} not found in the teams.");
            }
        }

        public void ListDevelopersOnTeam(string teamName)
        {
            // Find the team with the given name in the list
            DevTeam team = GetOrCreateTeam(teamName);

            if (team != null)
            {
                // Print the list of developers on the team
                foreach (Developer developer in team.TeamMembers)
                {
                    Console.WriteLine(developer);
                }
            }
            else
            {
                Console.WriteLine($"Team {teamName} not found in the teams.");
            }
        }

        public void ListDevelopersThatCanBeAdded(string teamName)
        {
            // get team
            DevTeam team = GetOrCreateTeam(teamName);

            if (team != null)
            {
                // get all developers
                List<Developer> allDevelopers = _devRepository.GetDevelopers();
                // new empty list of developers to check devs not on the team
                List<Developer> developersNotInTeam = new List<Developer>();
                // check if devs in are developer list are in are team and if not add them to are list of developersNotInTeam
                // opposite of lambda expression
                foreach (Developer dev in allDevelopers)
                {
                    bool isAlreadyInTeam = false;

                    foreach (Developer member in team.TeamMembers)
                    {
                        if (member.ID == dev.ID)
                        {
                            isAlreadyInTeam = true;
                        }
                    }
                    if (!isAlreadyInTeam)
                    {
                        developersNotInTeam.Add(dev);
                    }

                }
                // print the list of devs
                foreach (Developer dev in developersNotInTeam)
                {
                    System.Console.WriteLine(dev);
                }

            }
            Console.WriteLine($"{teamName} not found");
        }

        public void UpdateTeamMember(string teamName, int developerId, Developer newDevData)
        {
            // get team
            DevTeam team = GetOrCreateTeam(teamName);
            // find developer to replace
            Developer oldDevData = _devRepository.GetDeveloper(developerId);

            // replace with newdev
            if (oldDevData != null)
            {
                _devRepository.UpdateDeveloperById(developerId, newDevData);
                System.Console.WriteLine($"the developer at ID {developerId}: \n{oldDevData}\n has been updated with {newDevData}");
            }
            else
            {
                System.Console.WriteLine($"developer not found at {developerId}");
            }
        }
    }
}