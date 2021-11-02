using InmobiliariaConstante.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inmobiliaria_Tests
{
	class Helper
	{
		public Helper()
		{
			var valoresConfig = new Dictionary<string, string>
			{
				{"ConnectionStrings:Testing", "Server=(localdb)\\MSSQLLocalDB;Database=BDInmoTest;Trusted_Connection=True;MultipleActiveResultSets=true"},
				{"ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=BDInmobiliaria;Trusted_Connection=True;MultipleActiveResultSets=true"},
				{"Salt", "Salada"},
				{"TokenAuthentication:SecretKey", "Super_Secreta_es_la_clave_de_esta_APP_shhh"},
				{"TokenAuthentication:Issuer", "inmobiliariaULP"},
				{"TokenAuthentication:Audience", "mobileAPP"},
				{"TokenAuthentication:TokenPath", "/api/token"},
				{"TokenAuthentication:CookieName", "access_token"},
			};
			Config = new ConfigurationBuilder()
				.AddInMemoryCollection(valoresConfig)
				.Build();
			DataContext = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(Config["ConnectionStrings:Testing"]).Options);
		}

		internal IConfiguration Config { get; set; }
		internal DataContext DataContext { get; set; }

		internal ClaimsPrincipal MockLogin(string email, string rol)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, email),
				new Claim(ClaimTypes.Role, rol),
			};
			var identity = new ClaimsIdentity(claims, "TestAuthType");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			return claimsPrincipal;
		}
	}
}
