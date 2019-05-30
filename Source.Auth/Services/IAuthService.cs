using Microsoft.AspNetCore.Identity;
using Source.Auth.Midels.AccountViewModels;
using Source.Auth.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Auth.Services
{
    public interface IAuthService
    {
        Task<SignInResult> LoginWithPassword(string email, string password, bool rememberMe);
        Task<BaseUser> GetUserAsync(ClaimsPrincipal user);
        Task Login(BaseUser user);
        Task Logout();
        Task<IdentityResult> ChangePasswordAsync(BaseUser user, string oldPassword, string newPassword);

        Task<ClaimsIdentity> GetIdentity(string username, string password);
        Task<(bool, string)> GetST(string email, string service);

        IQueryable<BaseUser> GetUsers();
        Task<BaseUser> GetUser(string username);
        Task<BaseUser> GetUserByEmailAsync(string email);
        Task<BaseUser> GetUserByEmail(string email);
        BaseUser GetUserByExternalId(string id, int type);

        Task<UserInfoViewModel> GetUserInfo(string username);
        IQueryable<BaseRole> GetRoles();
        Task<IEnumerable<string>> GetUserRoles(string username);
        Task<IList<string>> GetUserRolesByEmail(string email);
        Task<IList<BaseUser>> GetUsersInRoleAsync(string role);
        Task<IdentityResult> AddRolesAsync(string username, IEnumerable<string> roles);
        Task<IdentityResult> RemoveRolesAsync(string username, IEnumerable<string> roles);
        Task<IdentityResult> UpdateRolesAsync(string username, IEnumerable<string> roles);
        Task<IdentityResult> CreateRoleAsync(BaseRole role);
        Task<IdentityResult> DeleteRoleAsync(string role);
        Task<IdentityResult> UpdateRoleAsync(BaseRole role);
        Task<IdentityResult> CreateUser(BaseUser user, string password);
        Task<IdentityResult> UpdateUser(BaseUser user);
        Task<IdentityResult> DeleteUser(BaseUser user);
        IDictionary<string, string> GetGrantedNames(IEnumerable<string> grantedObjects);
        Task<decimal> UpdateCredit(string id, bool isAdd, decimal credit);

    }
}