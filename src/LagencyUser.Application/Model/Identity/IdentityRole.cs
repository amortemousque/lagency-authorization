namespace LagencyUser.Application.Model
{
    using System;
    using System.Collections.Generic;
    using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Attributes;
    using Microsoft.AspNetCore.Identity;

    public class IdentityRole
	{
		public IdentityRole()
		{
            Id = Guid.NewGuid().ToString();
		}

		public IdentityRole(string roleName) : this()
		{
			Name = roleName;
			NormalizedName = Name.ToUpper();
		}


        [BsonIgnoreIfNull]
        [PersonalData]
        public virtual List<string> Permissions { get; set; }

        [PersonalData]
        public string Description { get; set; }

		[BsonId]
		public string Id { get; set; }

		public string Name { get; set; }

		public string NormalizedName { get; set; }

		public override string ToString() => Name;
	}
}