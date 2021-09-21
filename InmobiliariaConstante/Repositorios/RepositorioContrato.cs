using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Contrato entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contrato (IdInquilino, IdInmueble, FechaDesde, FechaHasta, IdGarante, Cuotas, Estado) " +
                    "VALUES (@IdInquilino, @IdInmueble, @FechaDesde, @FechaHasta, @IdGarante, @Cuotas, @Estado);" +
                    "SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@FechaDesde", entidad.FechaDesde);
                    command.Parameters.AddWithValue("@FechaHasta", entidad.FechaHasta);
                    command.Parameters.AddWithValue("@IdGarante", entidad.IdGarante);
                    command.Parameters.AddWithValue("@Cuotas", entidad.Cuotas);
                    command.Parameters.AddWithValue("@Estado", entidad.Estado);
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
                string sql = $"UPDATE Contrato SET Estado = 0 WHERE Id = {id}";
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
                    ", Estado=@Estado, Cuotas=@Cuotas WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@FechaDesde", entidad.FechaDesde);
                    command.Parameters.AddWithValue("@FechaHasta", entidad.FechaHasta);
                    command.Parameters.AddWithValue("@IdGarante", entidad.IdGarante);
                    command.Parameters.AddWithValue("@Cuotas", entidad.Cuotas);
                    command.Parameters.AddWithValue("@Id", entidad.Id);
                    command.Parameters.AddWithValue("@Estado", entidad.Estado);
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
                    " c.Cuotas, c.Estado ,i.Nombre, i.Apellido," +
                    "m.Direccion, m.Precio," +
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
                            Cuotas = reader.GetInt32(6),
                            Estado = reader.GetBoolean(7),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(1),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            },
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(10),
                                Precio = reader.GetDecimal(11)
                            },
                            garante = new Garante
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(12),
                                Apellido = reader.GetString(13)
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
        {
            Contrato entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, c.IdInquilino, c.IdInmueble, c.FechaDesde, c.FechaHasta, c.IdGarante," +
                    " c.Cuotas, c.Estado, i.Nombre, i.Apellido," +
                    "m.Direccion, m.Precio," +
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
                            Cuotas = reader.GetInt32(6),
                            Estado = reader.GetBoolean(7),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(1),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            },
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(10),
                                Precio = reader.GetDecimal(11)
                            },
                            garante = new Garante
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(12),
                                Apellido = reader.GetString(13)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

        public IList<Inmueble> obtenerInmuebles(DateTime fechaDesde, DateTime fechaHasta, int idActual)
        {
            IList<Inmueble> inmuebles = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud" +
                                $", i.Estado, i.Precio FROM Inmuebles i LEFT JOIN (SELECT c.IdInmueble FROM Contrato c"
                                + " WHERE ((@fechaDesde BETWEEN c.FechaDesde AND c.FechaHasta)"
                                + " OR (@fechaHasta BETWEEN c.FechaDesde AND c.FechaHasta)) " 
                                + " AND c.IdInmueble != @IdActual AND c.Estado = 1) x"
                                + " ON i.Id = x.IdInmueble"
                                + " WHERE x.IdInmueble IS NULL; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
                    command.Parameters.Add("@IdActual", SqlDbType.Int).Value = String.IsNullOrEmpty(idActual.ToString()) ? DBNull.Value : idActual;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble entidad = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetDecimal(4),
                            Longitud = reader.GetDecimal(5),
                            Estado = reader.GetBoolean(6),
                            Precio = reader.GetDecimal(7),
                        };
                        inmuebles.Add(entidad);
                    }
                    connection.Close();

                }
            }
            return inmuebles;
        }
    }
}

