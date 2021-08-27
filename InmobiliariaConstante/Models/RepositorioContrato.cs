using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contrato (IdInquilino, IdInmueble, FechaDesde, FechaHasta, IdGarante) " +
                    "VALUES (@IdInquilino, @IdInmueble, @FechaDesde, @FechaHasta, @IdGarante);" +
                    "SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@FechaDesde", entidad.FechaDesde);
                    command.Parameters.AddWithValue("@FechaHasta", entidad.FechaHasta);
                    command.Parameters.AddWithValue("@IdGarante", entidad.IdGarante);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    entidad.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Contrato WHERE Id = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Contrato SET " +
                    "IdInquilino=@IdInquilino, IdInmueble=@IdInmueble, FechaDesde=@FechaDesde, FechaHasta=@FechaHasta, IdGarante=@IdGarante " +
                    "WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@FechaDesde", entidad.FechaDesde);
                    command.Parameters.AddWithValue("@FechaHasta", entidad.FechaHasta);
                    command.Parameters.AddWithValue("@IdGarante", entidad.IdGarante);
                    command.Parameters.AddWithValue("@id", entidad.Id);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT c.Id, c.IdInquilino, c.IdInmueble, c.FechaDesde, c.FechaHasta, c.IdGarante," +
                    " i.Nombre, i.Apellido," +
                    "m.Direccion," +
                    "g.Nombre, g.Apellido" +
                    " FROM Contrato c INNER JOIN Inquilinos i ON i.IdInquilino = C.IdInquilino " +
                    "INNER JOIN Inmuebles m ON m.Id = c.IdInmueble" +
                    " INNER JOIN Garante g ON g.Id = c.IdGarante";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato entidad = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(3),
                            FechaHasta = reader.GetDateTime(4),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            IdGarante = reader.GetInt32(5),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(8)
                            },
                            garante = new Garante
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10)
                            }
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato ObtenerPorId(int id)
        {Contrato entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, c.IdInquilino, c.IdInmueble, c.FechaDesde, c.FechaHasta, c.IdGarante," +
                    " i.Nombre, i.Apellido," +
                    "m.Direccion," +
                    "g.Nombre, g.Apellido" +
                    " FROM Contrato c INNER JOIN Inquilinos i ON i.IdInquilino = C.IdInquilino" +
                    " INNER JOIN Inmuebles m ON m.Id = c.IdInmueble" +
                    " INNER JOIN Garante g ON g.Id = c.IdGarante WHERE c.Id=@Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(3),
                            FechaHasta = reader.GetDateTime(4),
                            IdInquilino = reader.GetInt32(1),
                            IdInmueble = reader.GetInt32(2),
                            IdGarante = reader.GetInt32(5),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(8)
                            },
                            garante = new Garante
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

      
    }
}

