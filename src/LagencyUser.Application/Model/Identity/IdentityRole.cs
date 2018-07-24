namespace LagencyUserApplication.Model
{
    using System;
    using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Attributes;

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

		[BsonId]
		public string Id { get; set; }

		public string Name { get; set; }

		public string NormalizedName { get; set; }

		public override string ToString() => Name;
	}
}