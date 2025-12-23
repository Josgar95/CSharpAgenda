// See https://aka.ms/new-console-template for more information
//To create new console application use the command: dotnet new console -o ProjectName

using System.Globalization;


Console.WriteLine("Hello to the Agenda Application!");

string connectionString = @"Data Source=C:\Users\Giuse\Desktop\Progetti\CSharpAgenda\agenda.db;";
// Ask for a date (or use today) and print appointments for that day
string choice = "";
do
{
    Console.WriteLine("View appointments for a specific date[1] or all appointments[2] or add new appointment[3]?");
    choice = Console.ReadLine();
    if (choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
    }
} while (choice != "1" && choice != "2" && choice != "3");


switch (choice)
{
    case "1":
        Console.Write("Enter date (yyyy-MM-dd) or press Enter for today: ");
        var input = Console.ReadLine();
        DateTime day;
        if (string.IsNullOrWhiteSpace(input))
        {
            day = DateTime.Today;
        }
        else if (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out day))
        {
            Console.WriteLine("Invalid date format. Use yyyy-MM-dd.");
            return;
        }


        var appointments = Appointment.GetAppointmentsByDate(day, connectionString);
        if (appointments.Count == 0)
        {
            Console.WriteLine($"No appointments for {day:yyyy-MM-dd}.");
        }
        else
        {
            Console.WriteLine($"Appointments for {day:yyyy-MM-dd}:");
            foreach (var a in appointments)
            {
                Console.WriteLine($"- Id: {a.Id}, Start: {a.Start:yyyy-MM-dd HH:mm}, End: {(a.End.HasValue ? a.End.Value.ToString("yyyy-MM-dd HH:mm") : "N/A")}, Title: {a.Title}, Category: {a.Category}, Priority: {(a.Priority.HasValue ? a.Priority.Value.ToString() : "N/A")}, AllDay: {a.IsAllDay}");
            }
        }
        break;
    case "2":
        var allAppointments = Appointment.GetAllAppointments(connectionString);
        if (allAppointments.Count == 0)
        {
            Console.WriteLine("No appointments found.");
        }
        else
        {
            Console.WriteLine("All Appointments:");
            foreach (var a in allAppointments)
            {
                Console.WriteLine($"- Id: {a.Id}, Start: {a.Start:yyyy-MM-dd HH:mm}, End: {(a.End.HasValue ? a.End.Value.ToString("yyyy-MM-dd HH:mm") : "N/A")}, Title: {a.Title}, Category: {a.Category}, Priority: {(a.Priority.HasValue ? a.Priority.Value.ToString() : "N/A")}, AllDay: {a.IsAllDay}");
            }
        }
        break;
    case "3":
        Appointment.AddNewAppointment(connectionString);
        break;
    default:
        Console.WriteLine("Unexpected choice.");
        break;
}


Console.WriteLine("End of Agenda Application!");


