namespace RecipeManagement.Services;

using System.Security.Claims;
using RecipeManagement.Databases;
using SharedKernel.Domain;
using RecipeManagement.Domain;
using HeimGuard;
using Microsoft.EntityFrameworkCore;

public class UserPolicyHandler : IUserPolicyHandler
{
    private readonly RecipesDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UserPolicyHandler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        var user = _currentUserService.User;
        if (user == null) throw new ArgumentNullException(nameof(user));

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(r => r.Value)
            .Distinct()
            .ToArray();
        
        // super admins can do everything
        if(roles.Contains(Roles.SuperAdmin))
            return Permissions.List();

        var permissions = await _dbContext.RolePermissions
            .Where(rp => roles.Contains(rp.Role))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToArrayAsync();

        return await Task.FromResult(permissions);
    }
}