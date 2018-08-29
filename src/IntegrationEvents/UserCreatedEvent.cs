using System;
namespace IntegrationEvents
{
    public class UserCreatedEvent
    {

        public UserCreatedEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public override string ToString()
        {
            return $"Message1 : {Id}";
        }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; } // Tenant name of the user

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Picture { get; set; }

        public string ProviderName { get; set; }

        public string ProviderData { get; set; }

        public DateTime? RegistrationDate { get; set; }
    }

}
