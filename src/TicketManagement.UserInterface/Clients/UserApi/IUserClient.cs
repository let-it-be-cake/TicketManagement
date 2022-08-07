using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Identity;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Clients.UserApi
{
    public interface IUserClient
    {
        [Put("users/update")]
        public Task<string> UpdateAsync([Body] UserModel user,
                           [Header("Authorization")] string token,
                           CancellationToken cancellationToken = default);

        [Get("users/find-by-id/{id}")]
        public Task<User> FindByIdAsync([Path] int id,
                                   [Header("Authorization")] string token,
                                   CancellationToken cancellationToken = default);

        [Get("users/find-by-name/{name}")]
        public Task<User> FindByNameAsync([Path] string name,
                                          [Header("Authorization")] string token,
                                          CancellationToken cancellationToken = default);

        [Get("users/get-role-users/{role}")]
        public Task<List<User>> GetRoleUsers([Path] string role,
                                             [Header("Authorization")] string token,
                                             CancellationToken cancellationToken = default);

        [Get("users/get-all-users")]
        public Task<List<User>> GetAllUsers([Header("Authorization")] string token,
                                             CancellationToken cancellationToken = default);

        [Post("users/password-validate/")]
        public Task PasswordValidate([Body] PasswordValidateModel password,
                                     [Header("Authorization")] string token,
                                     CancellationToken cancellationToken = default);

        [Put("users/password-reset")]
        public Task<string> PasswordReset([Body] PasswordModel password,
                                  [Header("Authorization")] string token,
                                  CancellationToken cancellationToken = default);

        [Delete("users/ban/{id}")]
        public Task BanAsync([Path] int id,
                        [Header("Authorization")] string token,
                        CancellationToken cancellationToken = default);

        [Delete("users/unban/{id}")]
        public Task UnbanAsync([Path] int id,
                          [Header("Authorization")] string token,
                          CancellationToken cancellationToken = default);
    }
}
