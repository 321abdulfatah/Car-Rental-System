using Abp.Domain.Entities;
using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Repositories
{
	public class JWTManagerRepository : IJWTManagerRepository
	{
		private readonly IConfiguration _iconfiguration;

		private readonly CarRentalDBContext _CarRentalDBContext;

		private readonly DbSet<Users> _entities;
		public JWTManagerRepository(IConfiguration iconfiguration, CarRentalDBContext context)
		{
			_iconfiguration = iconfiguration;
			_CarRentalDBContext = context ?? throw new ArgumentNullException(nameof(context));
			_entities = context.Set<Users>();
		}

		public Tokens Authenticate(Users users)
		{
			var entity = _entities.FirstOrDefault(s => s.Name == users.Name && s.Password == users.Password);
			
			if (entity == null)
			{
				return null;
			}
			// Else we generate JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			 new Claim(ClaimTypes.Name, users.Name)
			  }),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return new Tokens { Token = tokenHandler.WriteToken(token) };

		}
	}
}