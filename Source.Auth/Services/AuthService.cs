using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Source.Auth.Midels.AccountViewModels;
using Source.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<BaseUser> _userManager;
        private readonly SignInManager<BaseUser> _signInManager;
        private readonly RoleManager<BaseRole> _roleManager;

        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<BaseUser> userManager,
            SignInManager<BaseUser> signInManager,
            RoleManager<BaseRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<AuthService>();
        }

        public async Task<SignInResult> LoginWithPassword(string username, string password, bool rememberMe)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Login(BaseUser user)
        {
            try
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<BaseUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(BaseUser user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> CreateRoleAsync(BaseRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<IdentityResult> UpdateRoleAsync(BaseRole role)
        {
            var _role = _roleManager.Roles.FirstOrDefault(x => x.Id == role.Id && x.ConcurrencyStamp == role.ConcurrencyStamp);
            if (_role != null)
            {
                _role.Name = role.Name;
                _role.RoleName = role.RoleName;
                _role.Description = role.Description;
                var result = await _roleManager.UpdateAsync(_role);
                return result;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteRoleAsync(string role)
        {
            var entity = _roleManager.Roles.FirstOrDefault(x => x.Name == role);
            if (entity == null)
            {
                return IdentityResult.Failed();
            }
            return await _roleManager.DeleteAsync(entity); ;
        }

        public Task<bool> GetCASInfoAsync(object userdata)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetUserRolesByEmail(string email)
        {
            IList<string> roles = new List<string>();
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user != null)
            {
                roles = _userManager.GetRolesAsync(user).Result;
            }
            return Task.FromResult(roles);
        }

        public IQueryable<BaseRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<IList<BaseUser>> GetUsersInRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<BaseUser> GetUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<BaseUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<BaseUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public IQueryable<BaseUser> GetUsers()
        {
            return _userManager.Users;
        }

        public async Task<IdentityResult> AddRolesAsync(string username, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> RemoveRolesAsync(string username, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> UpdateRolesAsync(string username, IEnumerable<string> roles)
        {
            var result = IdentityResult.Success;

            var user = await _userManager.FindByNameAsync(username);
            var userRoles = await _userManager.GetRolesAsync(user);

            var removeRoles = userRoles.Except(roles);
            var addRoles = roles.Except(userRoles);

            if (removeRoles.Any())
            {
                result = await _userManager.RemoveFromRolesAsync(user, removeRoles);
            }

            if (addRoles.Any() && result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, addRoles);
            }

            return result;
        }

        public Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<(bool, string)> GetST(string email, string service)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfoViewModel> GetUserInfo(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;
            var role = await _userManager.GetRolesAsync(user);

            user.LastLoginTime = DateTime.Now;
            var updateLoginTime = await _userManager.UpdateAsync(user);
            var userInfo = new UserInfoViewModel()
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                FullName = user.FullName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                RoleList = role
            };
            return userInfo;
        }

        public async Task<IdentityResult> CreateUser(BaseUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUser(BaseUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUser(BaseUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public IDictionary<string, string> GetGrantedNames(IEnumerable<string> grantedObjects)
        {
            throw new NotImplementedException();
        }
    }
}