using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Models
{
	public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
	{
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, Estado, Precio) " +
					"VALUES (@direccion, @ambientes, @superficie, @latitud, @longitud, @propietarioId, @Estado, @Precio);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@superficie", entidad.Superficie);
					command.Parameters.AddWithValue("@latitud", entidad.Latitud);
					command.Parameters.AddWithValue("@longitud", entidad.Longitud);
					command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
					command.Parameters.AddWithValue("@Estado", entidad.Estado);
					command.Parameters.AddWithValue("@Precio", entidad.Precio);
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
				string sql = $"DELETE FROM Inmuebles WHERE Id = {id}";
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
		public int Modificacion(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Inmuebles SET " +
					"Direccion=@direccion, Ambientes=@ambientes, Superficie=@superficie, Latitud=@latitud, Longitud=@longitud, PropietarioId=@propietarioId, Estado=@estado, Precio=@precio " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@superficie", entidad.Superficie);
					command.Parameters.AddWithValue("@latitud", entidad.Latitud);
					command.Parameters.AddWithValue("@longitud", entidad.Longitud);
					command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT Id, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, Estado, Precio," +
					" p.Nombre, p.Apellido" +
					" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
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
							PropietarioId = reader.GetInt32(6),
							Estado = reader.GetBoolean(7),
							Precio = reader.GetDecimal(8),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(6),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, Estado, Precio, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
					$" WHERE Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Latitud = reader.GetDecimal(4),
							Longitud = reader.GetDecimal(5),
							PropietarioId = reader.GetInt32(6),
							Estado = reader.GetBoolean(7),
							Precio = reader.GetDecimal(8),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(6),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Inmueble> BuscarPorPropietario(int idPropietario)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Direccion, Ambientes, Superficie, Latitud, Longitud, PropietarioId, Estado, Precio, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
					$" WHERE PropietarioId=@idPropietario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Latitud = reader.GetDecimal(4),
							Longitud = reader.GetDecimal(5),
							PropietarioId = reader.GetInt32(6),
							Estado = reader.GetBoolean(7),
							Precio = reader.GetDecimal(8),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(6),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Contrato> ObtenerInmueblesXContrato(int id)
		{
			List<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, c.IdInquilino, c.IdInmueble, c.FechaDesde, c.FechaHasta, c.IdGarante, " +
					" c.Cuotas, c.Estado, i.Nombre, i.Apellido FROM Contrato c " +
					" JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino WHERE c.IdInmueble = @id;";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
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
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
