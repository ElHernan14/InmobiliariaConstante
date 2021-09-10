using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Pago i)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pagos (NumeroDePago, FechaDePago, Monto, IdContrato) 
				VALUES(@NumeroDePago,@FechaDePago,@Monto,@IdContrato);SELECT SCOPE_IDENTITY();";
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.Parameters.AddWithValue("@NumeroDePago", i.NumeroDePago);
                    com.Parameters.AddWithValue("@FechaDePago", i.FechaDePago);
                    com.Parameters.AddWithValue("@Monto", i.Monto);
                    com.Parameters.AddWithValue("@IdContrato", i.IdContrato);
                    con.Open();
                    res = com.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public int Modificacion(Pago e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Pagos SET " +
                    $"NumeroDePago=@NumeroDePago, FechaDePago=@FechaDePago, Monto=@Monto, IdContrato=@IdContrato " +
                    $"WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@NumeroDePago", e.NumeroDePago);
                    command.Parameters.AddWithValue("@FechaDePago", e.FechaDePago);
                    command.Parameters.AddWithValue("@Monto", e.Monto);
                    command.Parameters.AddWithValue("@IdContrato", e.IdContrato);
                    connection.Open();
                    res = command.ExecuteNonQuery();
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
                string sql = $"DELETE FROM Pagos WHERE Id = {id}";
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

        public IList<Pago> ObtenerTodos(int id)
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, NumeroDePago, FechaDePago, Monto, IdContrato" +
                    $" FROM Pagos WHERE IdContrato = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroDePago = reader.GetInt32(1),
                            FechaDePago = reader.GetDateTime(2),
                            Monto = reader.GetDecimal(3),
                            IdContrato = reader.GetInt32(4),
                            contrato = new Contrato
                            {
                                Id = reader.GetInt32(0)
                            }
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, NumeroDePago, FechaDePago, Monto, IdContrato FROM Pagos" +
                    $" WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroDePago = reader.GetInt32(1),
                            FechaDePago = reader.GetDateTime(2),
                            Monto = reader.GetDecimal(3),
                            IdContrato = reader.GetInt32(4),
                            contrato = new Contrato
                            {
                                Id = reader.GetInt32(0)
                            }
                        };
                        return p;
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Pago ObtenerPorNumeroPago(int id)
        {
            Pago p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, NumeroDePago, FechaDePago, Monto, IdContrato FROM Pagos" +
                    $" WHERE NumeroDePago=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroDePago = reader.GetInt32(1),
                            FechaDePago = reader.GetDateTime(2),
                            Monto = reader.GetDecimal(3),
                            IdContrato = reader.GetInt32(4),
                            contrato = new Contrato
                            {
                                Id = reader.GetInt32(0)
                            }
                        };
                        return p;
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public decimal ObtenerTotal(int id)
        {
            decimal p = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Monto FROM Pagos" +
                    $" WHERE IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p += reader.GetDecimal(0);
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Pago> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
