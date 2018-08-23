using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LagencyUser.Application.Contracts;
using LagencyUser.Application.Model;
using System.Linq.Expressions;

namespace LagencyUser.Application.Queries
{
    public class UserQueries
    {
        private readonly IUserRepository _userRepository;

        public UserQueries(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<IdentityUser>> GetUsersAsync(string name, string email)
        {

            var response = await _userRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameUpper = name.ToUpper();
                response = response.Where(a => a.NormalizedFullName.StartsWith(nameUpper));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailUpper = name.ToUpper();
                response = response.Where(a => a.NormalizedEmail.StartsWith(emailUpper));
            }


            return response.ToList();
        }

    }

}
