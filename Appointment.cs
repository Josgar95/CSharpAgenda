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
}
