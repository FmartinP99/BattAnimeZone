using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Policies
{
	public static class Policies
	{
		public const string IsAuthenticated = "IsAuthenticated";
		public static AuthorizationPolicy IsAuthenticatedPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
												   .RequireRole("Admin", "User")
												   .Build();
		}
	}
}
