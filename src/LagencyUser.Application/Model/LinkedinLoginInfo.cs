using System;
namespace LagencyUser.Application.Model
{
    public class LinkedinLoginInfo
    {
        public string FirstName { get; set; }
        public string FormattedName { get; set; }
        public string Headline { get; set; }
        public string Id { get; set; }
        public string Industry { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }
        public string PublicProfileUrl { get; set; }

    }

    public class LinkedinJob {
        public string Company { get; set; }
       
    }
}
