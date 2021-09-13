using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public RoommateRepository GetRoommateById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.FirstName, rm.RentPortion, rm.RoomId, r.Name
                                        FROM Roommate rm
                                        INNER JOIN Room r
                                        ON rm.RoomId = r.Id
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    RoommateRepository roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = new Room
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public static implicit operator RoommateRepository(Roommate v)
        {
            throw new NotImplementedException();
        }
    }
}
// work on this tonight 