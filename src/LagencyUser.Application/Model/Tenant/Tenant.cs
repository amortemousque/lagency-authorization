namespace LagencyUser.Application.Model
{
    using System;
    using System.Threading.Tasks;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization.Attributes;
    using LagencyUser.Application.Contracts;

    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long UsersNumber { get; set; }

        public int RegionId { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string LogoUri { get; set; }

        public bool Enabled { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public override string ToString() => Name;

        public void UpdateInfos(string description, string logoUri)
        {
            Description = description;
            LogoUri = logoUri;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }


        public static class Factory
        {
            public static async Task<Tenant> CreateNewEntry(
                ITenantRepository repository,
                string name,
                string description
            )
            {

                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The tenant must be specified", nameof(name));


                if (!await repository.HasUniqName(name))
                    throw new ArgumentException("An other tenant has the same name.", nameof(name));


                var tenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    Enabled = true,
                    RegionId = TenantRegion.EUROPE.Id,
                    UsersNumber = 0,
                    RegisteredDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                return tenant;
            }
        }
    }
}    