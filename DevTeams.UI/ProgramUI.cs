using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTeams.Data;
using DevTeams.Repository;
using static System.Console;

public class ProgramUI
{
    // brings in the methods from the devrepository
    private readonly DevRepository _devRepo = new DevRepository();

    // brings in the methods from devteamrepository
    private readonly DevTeamRepository _devrepository = new DevTeamRepository();





    public void Run()
    {
        RunApplication();
    }

    private void RunApplication()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Clear();
            WriteLine("welcome to Komodo Dev teams\n" +
                      "Dev List Controls----------------------------\n" +
                      "1.  Add Developer\n" +
                      "2.  List Developers\n" +
                      "3.  List Developer By ID\n" +
                      "4.  Update Developer By ID\n" +
                      "5.  Delete Developer by ID\n" +
                      "DevTeam Controls-----------------------------\n" +
                      "6.  Create A Team\n" +
                      "7.  Delete A Team\n" +
                      "8.  List Teams\n" +
                      "9.  Update A Team\n" +
                      "10. Add Developer To Team\n" +
                      "11. Remove Developer From Team\n" +
                      "12. List Developers On Team\n" +
                      "13. List Developers That Can Be Added To Team\n" +
                      "14. Update TeamMember On A team\n" +
                      "0.  Close Application\n");
            var UserInput = int.Parse(ReadLine()!);

            switch (UserInput)
            {
                case 1:
                    AddDeveloper();
                    break;
                case 2:
                    ListDevelopers();
                    break;
                case 3:
                    ListDeveloperById();
                    break;
                case 4:
                    UpdateDeveloperById();
                    break;
                case 5:
                    RemoveDeveloperById();
                    break;
                case 6:
                    CreateTeam();
                    break;
                case 7:
                    DeleteTeam();
                    break;
                case 8:
                    ListTeams();
                    break;
                case 9:
                    UpdateTeam();
                    break;
                case 10:
                    AddDeveloperToTeam();
                    break;
                case 11:
                    DeleteDeveloperFromTeam();
                    break;
                case 12:
                    ListDevelopersOnTeam();
                    break;
                case 13:
                    ListDevelopersThatCanBeAdded();
                    break;
                case 14:
                    UpdateTeamMember();
                    break;
                case 0:
                    isRunning = CloseApplication();
                    break;
            }
        }
    }

    private bool CloseApplication()
    {
        Clear();
        WriteLine("thx");
        return false;
    }

    private void PressAnyKey()
    {
        WriteLine("press any key to continue");
        ReadKey();
    }

    private void ListDevelopers()
    {
        Clear();
        List<Developer> devsInDb = _devRepo.GetDevelopers();
        foreach (Developer dev in devsInDb)
        {
            WriteLine(dev);
        }
        PressAnyKey();
    }

    private void ListDeveloperById()
    {
        Clear();
        List<Developer> devsInDb = _devRepo.GetDevelopers();
        bool userSelection = true;
        while (userSelection == true)
        {
            try
            {
                Write("Please input an ID number: ");
                int UserInput = int.Parse(ReadLine()!);

                Developer foundDeveloper = _devRepo.GetDeveloper(UserInput);

                if (foundDeveloper != null)
                {
                    Clear();
                    WriteLine($"Developer found: {foundDeveloper.FullName} (ID: {foundDeveloper.ID} {foundDeveloper.HasPluralSight})");
                    WriteLine("would you like to search for someone else by ID? (yes/no)");
                    string continueInput = ReadLine()!.ToLower();
                    if (continueInput == "no")
                    {
                        userSelection = false;
                    }
                }
                else
                {
                    WriteLine("Developer not found with this ID");
                }
            }
            catch (Exception e)
            {
                WriteLine($"Invalid input, please inputa valid ID: {e.Message}");
            }
            PressAnyKey();
        }
    }

    private void AddDeveloper()
    {
        Clear();

        var developer = new Developer();

        Write("please input a first name: ");
        string UserInput_FirstName = ReadLine()!;
        developer.FirstName = UserInput_FirstName;

        // PressAnyKey();

        Write("Please input a last name: ");
        string UserInput_LastName = ReadLine()!;
        developer.LastName = UserInput_LastName;

        // PressAnyKey();
        bool userSelection = false;
        while (userSelection == false)
        {

            Write("Will they need PluralSight? ");
            string UserInput_HasPluralSight = ReadLine()!.ToLower();

            switch (UserInput_HasPluralSight)
            {
                case "yes":
                    developer.HasPluralSight = true;
                    userSelection = true;
                    break;
                case "no":
                    developer.HasPluralSight = false;
                    userSelection = true;
                    break;
                default:
                    Write("please input either yes or no");
                    break;
            }
        }
        // if (UserInput_HasPluralSight == "yes")
        // {
        //     developer.HasPluralSight = true;
        // }
        // else if (UserInput_HasPluralSight == "no")
        // {
        //     developer.HasPluralSight = false;
        // }
        // else
        // {
        //     Write("please only input yes or no");
        // }


        bool addedSuccessfully = _devRepo.AddDeveloper(developer);

        if (addedSuccessfully)
        {
            Write("Developer has been added");
        }
        else
        {
            Write("Unsuccessful");
        }
    }
    private void UpdateDeveloperById()
    {
        Clear();
        // get developers in DB
        List<Developer> devsInDb = _devRepo.GetDevelopers();
        // gets ID from user they want to update
        Write("what is the ID of the developer you want to update? ");
        string UserInput = ReadLine()!;

        if (int.TryParse(UserInput, out int id))
        {
            if (id >= 0 && id < devsInDb.Count)
            {
                // Developer foundDeveloper = devsInDb[id];
                Developer foundDeveloper = _devRepo.GetDeveloper(id);
                // valid input
                if (foundDeveloper != null)
                {

                    // new up a developer to hold are newDevData
                    var newDevData = new Developer();


                    WriteLine($"current details of developer ID {id}");
                    WriteLine(foundDeveloper);

                    WriteLine("what is the updated users First name? ");
                    string UserInput_FirstName = ReadLine()!;
                    newDevData.FirstName = UserInput_FirstName;

                    WriteLine("what is the updated users Last name? ");
                    string UserInput_LastName = ReadLine()!;
                    newDevData.LastName = UserInput_LastName;

                    bool userSelection = false;
                    while (!userSelection)
                    {
                        WriteLine($"will {newDevData.FirstName} {newDevData.LastName} need PluralSight? (yes/no) ");
                        string UserInput_HasPluralSight = ReadLine()!.ToLower();

                        switch (UserInput_HasPluralSight)
                        {
                            case "yes":
                                newDevData.HasPluralSight = true;
                                userSelection = true;
                                Write("PluralSight has been added");
                                break;
                            case "no":
                                newDevData.HasPluralSight = false;
                                userSelection = true;
                                Write("PluralSight has NOT been added");
                                break;
                            default:
                                Write("please input either yes or no");
                                break;
                        }
                    }
                    if (_devRepo.UpdateDeveloperById(foundDeveloper.ID, newDevData))
                    {
                        System.Console.WriteLine("successfully updated");
                    }
                    else
                    {
                        System.Console.WriteLine("Unsuccessful");
                    }
                }
                else
                {
                    // dev not found
                    Write("developer not found");
                }
            }
            else
            {
                // invalid input
                Write("invalid input please input a valid ID number");
            }
        }
        else
        {
            // not a valid input
            Write("invalid input please input a valid ID number");
        }
    }
    private void RemoveDeveloperById()
    {
        Clear();
        Write("Enter the ID of the developer you want to remove: ");
        string userInput = ReadLine()!;

        if (int.TryParse(userInput, out int id))
        {
            _devRepo.DeleteDeveloperById(id);
        }
        else
        {
            WriteLine("Invalid input. Please enter a valid ID number.");
        }

        PressAnyKey();
    }
    private void AddDeveloperToTeam()
    {
        Clear();
        System.Console.WriteLine("what is the name of the team you want to change? ");
        string teamName = Console.ReadLine()!;
        System.Console.WriteLine($"what is the ID of the developer you want to add to the team {teamName}?");
        int developerId = int.Parse(Console.ReadLine()!);
        _devrepository.AddDeveloperToTeam(teamName, developerId);
        System.Console.WriteLine($"successfully added dev to {teamName}");
    }

    private void DeleteDeveloperFromTeam()
    {
        Clear();
        System.Console.WriteLine("what is the name of the team you want to change?");
        string teamName = Console.ReadLine()!;
        System.Console.WriteLine($"what is the ID of the developer you want to delete from the team {teamName}?");
        int developerId = int.Parse(Console.ReadLine()!);
        _devrepository.DeleteDeveloperFromTeam(teamName, developerId);
        System.Console.WriteLine($"successfully deleted dev from {teamName}");
    }

    private void ListDevelopersOnTeam()
    {
        Clear();
        System.Console.WriteLine("what team are you trying to list the developers for? ");
        string teamName = Console.ReadLine()!;
        _devrepository.ListDevelopersOnTeam(teamName);
    }

    private void ListDevelopersThatCanBeAdded()
    {
        Clear();
        System.Console.WriteLine("what team are you looking to see who can be added? ");
        string teamName = Console.ReadLine()!;
        _devrepository.ListDevelopersThatCanBeAdded(teamName);
    }

    private void UpdateTeamMember()
    {
        Clear();
        // Get the team name
        Write("Enter the name of the team: ");
        string teamName = ReadLine()!;

        // Get the developer ID to update
        Write("Enter the ID of the developer you want to update in the team: ");
        int developerId = int.Parse(ReadLine()!);


        var newDevData = new Developer();

        System.Console.WriteLine("what is the updated First Name: ");
        newDevData.FirstName = Console.ReadLine()!;

        System.Console.WriteLine("what is the updated LasT Name: ");
        newDevData.LastName = Console.ReadLine()!;

        bool userSelection = false;
        while (!userSelection)
        {
            Clear();
            WriteLine($"will {newDevData.FirstName} {newDevData.LastName} need PluralSight? (yes/no) ");
            string UserInput_HasPluralSight = ReadLine()!.ToLower();

            switch (UserInput_HasPluralSight)
            {
                case "yes":
                    newDevData.HasPluralSight = true;
                    userSelection = true;
                    Write("PluralSight has been added");
                    break;
                case "no":
                    newDevData.HasPluralSight = false;
                    userSelection = true;
                    Write("PluralSight has NOT been added");
                    break;
                default:
                    Write("please input either yes or no");
                    break;
            }
        }
        // call updateTeamMember method from devteamrepository
        _devrepository.UpdateTeamMember(teamName, developerId, newDevData);

        PressAnyKey();

    }

    private void CreateTeam()
    {
        Clear();
        System.Console.WriteLine("what is the name of the new team? ");
        string teamName = Console.ReadLine()!;
        if (teamName != null)
        {
            _devrepository.CreateTeam(teamName);
        }
        else
        {
            System.Console.WriteLine("please input a team name");
        }
    }

    private void DeleteTeam()
    {
        Clear();
        System.Console.WriteLine("what is the name of the team you want to delete? ");
        string teamName = Console.ReadLine()!;
        if (teamName != null)
        {
            _devrepository.DeleteTeam(teamName);
        }
        else
        {
            System.Console.WriteLine($"No team found with the name {teamName}");
        }
    }

    private void ListTeams()
    {
        _devrepository.ListTeams();
    }

    private void UpdateTeam()
    {
        Clear();
        System.Console.WriteLine("what is the name of the team you want to update? ");
        string oldTeamName = Console.ReadLine()!;
        if (oldTeamName != null)
        {
            System.Console.WriteLine("what is the new name of this team? ");
            string newTeamName = Console.ReadLine()!;
            if (newTeamName != null)
            {
                _devrepository.UpdateTeam(oldTeamName, newTeamName);
            }
        }
        else
        {
            System.Console.WriteLine("please input a valid team name to update");
        }
    }
}