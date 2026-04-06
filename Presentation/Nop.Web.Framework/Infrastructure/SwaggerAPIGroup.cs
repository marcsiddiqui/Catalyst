using System.Collections.Generic;

namespace Nop.Web.Framework.Infrastructure
{
    public static class SwaggerAPIGroup
    {
        public const string Application = "application";
    }

    public static class SwaggerAPIGroupCredential
    {
        public static List<SwaggerGroup> Groups
        {
            get => new List<SwaggerGroup>
            {
                new SwaggerGroup(SwaggerAPIGroup.Application, "admin", "admin")
            };
        }
    }

    public class SwaggerGroup
    {
        public SwaggerGroup(string _Name, string _Username, string _Password)
        {
            Name = _Name;
            Username = _Username;
            Password = _Password;
        }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}