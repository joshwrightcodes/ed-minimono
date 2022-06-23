namespace Wright.Demo.MiniMono.Application.Common.Identity;

/// <summary>
/// Defines an interface for interacting with an Identity Service.
/// </summary>
public interface IIdentityService
{
	/// <summary>
	/// Gets the username of the current user by user id.
	/// </summary>
	/// <param name="userId">User ID to retrieve.</param>
	/// <returns>User's username.</returns>
	Task<string> GetUserNameAsync(string userId);

	/// <summary>
	/// Checks if the user is in a particular role or not.
	/// </summary>
	/// <param name="userId">User ID to query for.</param>
	/// <param name="role">Role name to check.</param>
	/// <returns>
	/// Returns <c>true</c> if the user has the specified role, otherwise returns <c>false</c>.
	/// </returns>
	Task<bool> IsInRoleAsync(string userId, string role);

	/// <summary>
	/// Checks if the user is authorised under the specified policy.
	/// </summary>
	/// <param name="userId">User ID to query for.</param>
	/// <param name="policyName">Policy name to check.</param>
	/// <returns>
	/// Returns <c>true</c> if the user is authorised under the policy, otherwise returns <c>false</c>.
	/// </returns>
	Task<bool> AuthorizeAsync(string userId, string policyName);
}