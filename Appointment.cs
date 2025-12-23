using System;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Collections.Generic;
public class Appointment
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public int? Priority { get; set; }
    public bool IsAllDay { get; set; }

    public static List<Appointment> GetAppointmentsByDate(DateTime day, string connectionString)
    {
        var results = new List<Appointment>();
        // Limiti del giorno: 00:00–23:59:59
        string start = day.ToString("yyyy-MM-dd 00:00", CultureInfo.InvariantCulture);
        string end = day.ToString("yyyy-MM-dd 23:59", CultureInfo.InvariantCulture);

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string sql = @"
            SELECT Id, StartDateTime, EndDateTime, Title, Category, Priority, IsAllDay
            FROM Appointments
            WHERE StartDateTime BETWEEN @start AND @end
            ORDER BY StartDateTime;";
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appt = new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Start = DateTime.ParseExact(reader["StartDateTime"].ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            End = reader["EndDateTime"] is DBNull ? (DateTime?)null
                                 : DateTime.ParseExact(reader["EndDateTime"].ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Title = reader["Title"].ToString(),
                            Category = reader["Category"]?.ToString(),
                            Priority = reader["Priority"] is DBNull ? (int?)null : Convert.ToInt32(reader["Priority"]),
                            IsAllDay = Convert.ToInt32(reader["IsAllDay"]) == 1
                        };
                        results.Add(appt);
                    }
                }
            }
        }
        return results;
    }

    public static List<Appointment> GetAllAppointments(string connectionString)
    {
        var results = new List<Appointment>();
        // Limiti del giorno: 00:00–23:59:59

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string sql = @"
            SELECT Id, StartDateTime, EndDateTime, Title, Category, Priority, IsAllDay
            FROM Appointments
            ORDER BY StartDateTime;";
            using (var cmd = new SqliteCommand(sql, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appt = new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Start = DateTime.ParseExact(reader["StartDateTime"].ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            End = reader["EndDateTime"] is DBNull ? (DateTime?)null
                                 : DateTime.ParseExact(reader["EndDateTime"].ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Title = reader["Title"].ToString(),
                            Category = reader["Category"]?.ToString(),
                            Priority = reader["Priority"] is DBNull ? (int?)null : Convert.ToInt32(reader["Priority"]),
                            IsAllDay = Convert.ToInt32(reader["IsAllDay"]) == 1
                        };
                        results.Add(appt);
                    }
                }
            }
        }
        return results;
    }

    public static void AddNewAppointment(string connectionString)
    {
        // Limiti del giorno: 00:00–23:59:59

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string start,end,title,desc,location,category;
            int? priority;
            bool isAllDay;


            Console.WriteLine("Adding a new appointment...");
            Console.WriteLine("Enter start time (yyyy-MM-dd HH:mm): ");
            start = Console.ReadLine();
            Console.WriteLine("Enter end time (yyyy-MM-dd HH:mm): ");
            end = Console.ReadLine();
            Console.WriteLine("Enter title: ");
            title = Console.ReadLine();
            Console.WriteLine("Enter description: ");
            desc = Console.ReadLine();
            Console.WriteLine("Enter location: ");
            location = Console.ReadLine();
            Console.WriteLine("Enter category: ");
            category = Console.ReadLine();
            Console.WriteLine("Enter priority (1-5): ");
            priority = int.Parse(Console.ReadLine());
            Console.WriteLine("Is all day? (true/false): ");
            isAllDay = bool.Parse(Console.ReadLine());

            string sql = @"
            INSERT INTO Appointments (StartDateTime, EndDateTime, Title, Description, Location, Category, Priority, IsAllDay)
            VALUES
            (@start, @end, @title, @desc, @location, @category, @priority, @isAllDay);";
            using (var cmd = new SqliteCommand(sql, conn))
            {
                // adding parameters for the SQL command
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@priority", priority.HasValue ? (object)priority.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@isAllDay", isAllDay ? 1 : 0);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Appointment added successfully.");
            }
        }
    }
}
