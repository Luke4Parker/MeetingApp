using System;
using System.Collections.Generic;
using System.IO;

namespace MeetingApp
{
    public class Meeting
    {
        private string title;
        private string location;
        private DateTime startDateTime;
        private DateTime endDateTime;

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return this.startDateTime;
            }
            set
            {
                this.startDateTime = value;
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return this.endDateTime;
            }
            set
            {
                this.endDateTime = value;
            }
        }
        public Meeting() { }
        public Meeting(string title, string location, DateTime startTime, DateTime endTime)
        {
            Title = title;
            Location = location;
            StartDateTime = startTime;
            EndDateTime = endTime;
        }
        public override string ToString()
        {
            return string.Format("{0} from {1} to {2} | {3} at {4}", StartDateTime.ToString("MM/dd"), StartDateTime.ToString("HH:mm"), EndDateTime.ToString("HH:mm"), Title, Location);
        }
    }
    public class BadInputException : Exception 
    {
        public BadInputException() { }
        public BadInputException(string message) : base(message) { }
        public BadInputException(string message, Exception inner) : base(message, inner) { }
               
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            int input;
            Console.WriteLine("Welcome to the Calendar App");

            do
            {
                input = MainSwitchCase();
            } while (input != 0);

            Console.WriteLine("Thanks and have a great day!");

        }
        static int MainMenuBuilder()
        {
            int input = 3;
            try {
                Console.WriteLine("Select from the following menu choices:\n" +
               "1. Load Calendar\n" +
               "2. Create New Calendar\n" +
               "0. Exit\n");
                input = int.Parse(Console.ReadLine());
                if (input > 2 || input < 0)
                {
                    throw new BadInputException();
                }
            }
            catch(BadInputException)
            {
                Console.WriteLine("You must enter 1, 2, or 0");
            }
            catch(FormatException)
            {
                Console.WriteLine("You must enter 1, 2, or 0");
            }
            return input;
        }
        static int SubMenuBuilder()
        {
            int input = 4;
            try
            {
                Console.WriteLine();
                Console.WriteLine("Select from the following menu choices:\n" +
               "1. Add Meeting to Calendar\n" +
               "2. Remove Meeting from Calendar\n" +
               "3. View Schedule\n" +
               "0. Save and Close Calendar\n");
                input = int.Parse(Console.ReadLine());
                if (input > 3 || input < 0)
                {
                    throw new BadInputException();
                }
            }
            catch (BadInputException)
            {
                Console.WriteLine("You must enter 1, 2, 3, or 0");
            }
            catch (FormatException)
            {
                Console.WriteLine("You must enter 1, 2, 3, or 0");
            }
            return input;
        }
        static int MainSwitchCase()
        {
            int input = 3;
            List<Meeting> calendar = new List<Meeting>();
            do
            {
                input = MainMenuBuilder();
                try
                {
                    switch (input)
                    {
                        case 1:
                            //load calendar takes the filepath and populates the calendar list with Meeting objects
                            calendar = LoadCalendar();
                            SubSwitchCase(calendar);
                            break;
                        case 2:
                            calendar = CreateNewCalendar();
                            SubSwitchCase(calendar);
                            break;
                        case 0:
                            break;
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Your Filepath was not found");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("No file loaded\n");
                    break;
                }
                
            } while (input != 0);
            return input;
        }
        static void SubSwitchCase(List<Meeting> calendar)
        {
            int input = 4;
            do
            {
                input = SubMenuBuilder();
                switch (input)
                {
                    case 1:
                        AddMeeting(calendar);
                        break;
                    case 2:
                        RemoveMeeting(calendar);
                        break;
                    case 3:
                        PrintCalendar(calendar);
                        break;
                    case 0:
                        SaveCalendar(calendar);
                        break;
                }
            } while (input != 0);
     
            
        }
        static List<Meeting> CreateNewCalendar()
        {
            List<Meeting> calendar = new List<Meeting>();
            DateTime date;
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the date for your calendar in format mm/dd/yyyy: ");
                    Console.WriteLine("This is being called at CreateNewCalendar()");
                    string input = Console.ReadLine();
                    date = DateTime.Parse(input + " 8:00");
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect date format. Please enter in mm/dd/yyyy");
                }
            }

            for (int i = 0; i < 9; i++)
            {
                calendar.Add(new Meeting("Free", "N/A", date, date.AddMinutes(60)));
                date = date.AddMinutes(60);
            }

            //foreach (Meeting testMeet in calendar)
            //{
            //    Console.WriteLine(testMeet);
            //}

            return calendar;

        }
        static List<Meeting> LoadCalendar()
        {
            Console.WriteLine("Enter the filepath of the calendar you wish to load\nEnter nothing to abort");
            string filepath = Console.ReadLine();

            List<Meeting> calendar = new List<Meeting>();
            string line = "";
            using (StreamReader reader = File.OpenText(filepath))
            {
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    string[] dateHolder = line.Split(',');
                    calendar.Add(new Meeting(dateHolder[0], dateHolder[1], DateTime.Parse(dateHolder[2]), DateTime.Parse(dateHolder[3])));
                }
            }

            return calendar;
        }
        
        static void PrintCalendar(List<Meeting> calendar)
        {
            int i = 1;
            Console.WriteLine("Current Schedule\n");
            foreach (Meeting meeting in calendar)
            {
                Console.Write(i + ". ");
                Console.WriteLine(meeting);
                i++;
            }
            Console.WriteLine();
        }
        static List<Meeting> AddMeeting(List<Meeting> calendar)
        {
            int intInput;
            int yesNo;
            //foreach (Meeting testMeet in calendar)
            //{
            //    Console.Write(index + ". ");
            //    Console.WriteLine(testMeet);
            //    index++;
            //}

            Console.WriteLine("Enter the timeslot you want to add a meeting to: ");
            while (true)
            {
                try
                {
                    intInput = int.Parse(Console.ReadLine());
                    if (intInput < 1 || intInput > 9)
                    {
                        throw new BadInputException();
                    }
                    else if (calendar[intInput - 1].Title != "Free")
                    {
                        while (true)
                        {
                            Console.WriteLine("Do you want to over write this timeslot? 1 for Yes/2 for No");
                            yesNo = int.Parse(Console.ReadLine());
                            if (yesNo == 2)
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                break;
                            }
                            
                        }

                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("You must enter an int");
                }
                catch (BadInputException)
                {
                    Console.WriteLine("You didn't select an appropriate timeslot");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Select a different timeslot");
                }
            }


            Console.WriteLine("Enter the title of the meeting: ");
            string title = Console.ReadLine();
            Console.WriteLine("Enter the location of the meeting: ");
            string location = Console.ReadLine();


            calendar[intInput - 1].Title = title;
            calendar[intInput - 1].Location = location;

            //foreach (Meeting testMeet in calendar)
            //{
            //    Console.WriteLine(testMeet);
            //}

            List<Meeting> updatedCalendar = new List<Meeting>(calendar);

            return updatedCalendar;
        }
        static List<Meeting> RemoveMeeting(List<Meeting> calendar)
        {
            int intInput;
            int index = 1;
            //foreach (Meeting testMeet in calendar)
            //{
            //    Console.Write(index + ". ");
            //    Console.WriteLine(testMeet);
            //    index++;
            //}

            Console.WriteLine("Enter the timeslot you want to remove: ");
            while (true)
            {
                try
                {
                    intInput = int.Parse(Console.ReadLine());
                    if (intInput < 1 || intInput > 9)
                    {
                        throw new BadInputException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("You must enter an int");
                }
                catch (BadInputException)
                {
                    Console.WriteLine("You didn't select an appropriate timeslot");
                }
            }


            calendar[intInput - 1].Title = "Free";
            calendar[intInput - 1].Location = "N/A";

            Console.WriteLine("Meeting Removed");

            //foreach (Meeting testMeet in calendar)
            //{
            //    Console.WriteLine(testMeet);
            //}

            List<Meeting> updatedCalendar = new List<Meeting>(calendar);

            return updatedCalendar;
        }
        static void SaveCalendar(List<Meeting> calendar)
        {
            Console.WriteLine("Enter the filepath where you wish to save your calendar");
            string filepath = Console.ReadLine();
            if (File.Exists(filepath))
            {
                while (true)
                {
                    Console.WriteLine("Overwrite filepath? 1 for Yes/2 for No");
                    int input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 2)
                    {
                        Console.WriteLine("You must enter a valid input of 1 or 2");
                    }
                    else
                    {
                        switch (input)
                        {
                            case 1:
                                using (StreamWriter writer = new StreamWriter(filepath))
                                {

                                    foreach (Meeting testMeet in calendar)
                                    {
                                        writer.WriteLine(testMeet.Title + ',' + testMeet.Location + ',' + testMeet.StartDateTime.ToString() + ',' + testMeet.EndDateTime.ToString());
                                    }
                                    Console.WriteLine("Overwrite " + filepath);
                                }
                                break;
                            case 2:
                                Console.WriteLine("Choose a different file path");
                                filepath = Console.ReadLine();
                                using (StreamWriter writer = new StreamWriter(filepath))
                                {

                                    foreach (Meeting testMeet in calendar)
                                    {
                                        writer.WriteLine(testMeet.Title + ',' + testMeet.Location + ',' + testMeet.StartDateTime.ToString() + ',' + testMeet.EndDateTime.ToString());
                                    }
                                }
                                break;

                        }
                        break;
                    }
                }
            }
        }
    }
}
