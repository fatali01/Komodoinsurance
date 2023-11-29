using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTeams.Data;

namespace DevTeams.Repository
{
    public class DevRepository
    {
        // Fake database : collection List<T>
        private readonly List<Developer> _devDBContext;
        private int _idCount;
        public DevRepository()
        {
            _devDBContext = new List<Developer>();
            Seed();
        }

        private void Seed()
        {
            Developer daymon = new Developer("Daymon", "Wayans", false);
            Developer george = new Developer("George", "Carlin", true);
            Developer burr = new Developer("Bill", "Burr", false);

            AddDeveloper(daymon);
            AddDeveloper(george);
            AddDeveloper(burr);
        }

        // C.R.U.D

        public bool AddDeveloper(Developer developer)
        {
            // if (developer is null)
            // {
            //     return false;
            // }
            // else
            // {

            //     AddToDatabase(developer);
            // }
            return (developer is null) ? false : AddToDatabase(developer);
        }

        private bool AddToDatabase(Developer developer)
        {
            IncrementID(developer);
            _devDBContext.Add(developer);
            return true;
        }

        private void IncrementID(Developer developer)
        {
            _idCount++;
            developer.ID = _idCount;
        }

        public List<Developer> GetDevelopers()
        {
            return _devDBContext;
        }

        public Developer GetDeveloper(int id)
        {
            return _devDBContext.FirstOrDefault(dev => dev.ID == id)!;
        }
        public bool UpdateDeveloperById(int id, Developer newDevData)
        {
            var oldData = GetDeveloper(id);
            if (oldData != null)
            {
                oldData.FirstName = newDevData.FirstName;
                oldData.LastName = newDevData.LastName;
                oldData.HasPluralSight = newDevData.HasPluralSight;
                return true;

            }
            return false;
        }

        public void DeleteDeveloperById(int id)
        {
            Developer developerToRemove = _devDBContext.FirstOrDefault(dev => dev.ID == id)!;

            if (developerToRemove != null)
            {
                _devDBContext.Remove(developerToRemove);
                Console.WriteLine($"Developer with ID {id} removed successfully.");
            }
            else
            {
                Console.WriteLine($"Developer with ID {id} not found.");
            }
        }

        public List<Developer> GetDevelopersWithoutPs()
        {
            // start with empty list
            List<Developer> developersWoPs = new List<Developer>();

            // loop through database
            foreach (Developer dev in _devDBContext)
            {
                if (dev.HasPluralSight is false)
                {
                    // add to empty list
                    developersWoPs.Add(dev);
                }
            }
            return developersWoPs;
        }
    }
}


