using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Models
{
    public class RepositorioGarante : RepositorioBase
    {
		public RepositorioGarante(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Garante i)
		{
			int res = -1;
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Garante (Nombre,Apellido,Dni,Telefono,Email) VALUES(@nombre,@apellido,@dni,@telefono,@mail);SELECT SCOPE_IDENTITY();";
				using (SqlCommand com = new SqlCommand(sql, con))
				{
					com.Parameters.AddWithValue("@mail", i.Email);
					com.Parameters.AddWithValue("@dni", i.Dni);
					com.Parameters.AddWithValue("@nombre", i.Nombre);
					com.Parameters.AddWithValue("@apellido", i.Apellido);
					com.Parameters.AddWithValue("@telefono", i.Telefono);
					con.Open();
					res = com.ExecuteNonQuery();
					con.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Garante WHERE Id = {id}";
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
		public int Modificacion(Garante e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Garante SET " +
					$"Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@dni", e.Dni);
					command.Parameters.AddWithValue("@telefono", e.Telefono);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Garante> ObtenerTodos()
		{
			IList<Garante> res = new List<Garante>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email" +
					$" FROM Garante" +
					$" ORDER BY Apellido, Nombre";/* +
					$" OFFSET 0 ROWS " +
					$" FETCH NEXT 10 ROWS ONLY ";*/
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Garante p = new Garante
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Garante ObtenerPorId(int id)
		{
			Garante p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Garante" +
					$" WHERE Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Garante
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
						};
						return p;
					}
					connection.Close();
				}
			}
			return p;
		}
	}
}
