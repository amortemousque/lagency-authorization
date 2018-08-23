using System;
namespace LagencyUser.Application.Model
{
    public class TenantRegion : Enumeration
    {
        public static TenantRegion EUROPE = new TenantRegion(1, "Europe");


        protected TenantRegion() { }

        public TenantRegion(int id, string name)
            : base(id, name)
        {

        }
    }
}
