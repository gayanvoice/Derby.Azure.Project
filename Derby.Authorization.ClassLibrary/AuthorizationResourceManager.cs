using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Authorization;
using Azure.Core;
using Azure.ResourceManager.Authorization.Models;
using Azure;

namespace Derby.Authorization.ClassLibrary
{
    //https://learn.microsoft.com/en-gb/azure/digital-twins/concepts-security
    //https://learn.microsoft.com/en-us/rest/api/authorization/role-assignments/create?view=rest-authorization-2022-04-01&tabs=dotnet#create-role-assignment-for-resource-group
    public class AuthorizationResourceManager
    {
        private string _tenentId;
        private string _principleId;
        public AuthorizationResourceManager(string tenentId, string principleId)
        {
            _tenentId = tenentId;
            _principleId = principleId;
        }
        public async Task<List<RoleAssignmentData>> GetRoleAssignmentDataListAsync(string resourceId)
        {
            Console.WriteLine($"Getting Role Definition Data List");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(resourceId);
            RoleAssignmentCollection roleAssignmentCollection = armClient.GetRoleAssignments(resourceIdentifier);
            List<RoleAssignmentData> roleAssignmentDataList = new List<RoleAssignmentData>();
            await foreach (RoleAssignmentResource roleAssignmentResource in roleAssignmentCollection.GetAllAsync())
            {
                RoleAssignmentData roleAssignmentData = roleAssignmentResource.Data;
                roleAssignmentDataList.Add(roleAssignmentData);
                Console.WriteLine($"Id: {roleAssignmentData.Id} Scope: {roleAssignmentData.Scope} Display Name: {roleAssignmentData.Name}");
            }
            Console.WriteLine($"Count: {roleAssignmentDataList.Count()}");
            return roleAssignmentDataList;
        }
        public async Task<List<AuthorizationRoleDefinitionData>> GetAuthorizationRoleDefinitionDataListAsync(string resourceId)
        {
            Console.WriteLine($"Getting Authorization Role Definition Data List");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(resourceId);
            AuthorizationRoleDefinitionCollection authorizationRoleDefinitionCollection = armClient.GetAuthorizationRoleDefinitions(resourceIdentifier);
            List<AuthorizationRoleDefinitionData> authorizationRoleDefinitionDataList = new List<AuthorizationRoleDefinitionData>();
            await foreach (AuthorizationRoleDefinitionResource authorizationRoleDefinitionResource in authorizationRoleDefinitionCollection.GetAllAsync())
            {
                AuthorizationRoleDefinitionData authorizationRoleDefinitionData = authorizationRoleDefinitionResource.Data;
                authorizationRoleDefinitionDataList.Add(authorizationRoleDefinitionData);
            }
            Console.WriteLine($"Count: {authorizationRoleDefinitionDataList.Count()}");
            return authorizationRoleDefinitionDataList;
        }
        public async Task<AuthorizationRoleDefinitionData> GetAuthorizationRoleDefinitionData(string resourceId, string name)
        {
            Console.WriteLine($"Getting Authorization Role Definition Data");
            List<AuthorizationRoleDefinitionData> authorizationRoleDefinitionDataList = await GetAuthorizationRoleDefinitionDataListAsync(resourceId);
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = authorizationRoleDefinitionDataList.Where(authorizationRoleDefinitionData => authorizationRoleDefinitionData.RoleName.Equals(name)).First();
            Console.WriteLine($"Id: {authorizationRoleDefinitionData.Id} Name: {authorizationRoleDefinitionData.RoleName} Description: {authorizationRoleDefinitionData.Description} Type: {authorizationRoleDefinitionData.ResourceType}");
            return authorizationRoleDefinitionData;
        }
        public async Task<RoleAssignmentData> CreateRoleAssignmentAsync(string resourceId, string name)
        {
            Console.WriteLine($"Creating Role Assignment Async");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);         
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await GetAuthorizationRoleDefinitionData(resourceId: resourceId, name: name);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(authorizationRoleDefinitionData.Id);
            RoleAssignmentCreateOrUpdateContent roleAssignmentCreateOrUpdateContent = new RoleAssignmentCreateOrUpdateContent(resourceIdentifier, Guid.Parse(_principleId));
            roleAssignmentCreateOrUpdateContent.PrincipalType = RoleManagementPrincipalType.User;
            ResourceIdentifier resourceIdentifier2 = RoleAssignmentResource.CreateResourceIdentifier(resourceId, Guid.NewGuid().ToString());
            RoleAssignmentResource roleAssignmentResource = armClient.GetRoleAssignmentResource(resourceIdentifier2);
            ArmOperation<RoleAssignmentResource> armOperationRoleAssignmentResource = roleAssignmentResource.Update(WaitUntil.Completed, roleAssignmentCreateOrUpdateContent);
            RoleAssignmentResource roleAssignmentResource1 = armOperationRoleAssignmentResource.Value;
            RoleAssignmentData roleAssignmentData = roleAssignmentResource1.Data;
            Console.WriteLine($"Id: {roleAssignmentData.Id} Scope: {roleAssignmentData.Scope} Display Name: {roleAssignmentData.Name}");
            return roleAssignmentData;
        }
        public async Task<RoleAssignmentData> GetRoleAssignmentAsync(string resourceId, string name)
        {
            Console.WriteLine($"Get Role Assignment Async");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await GetAuthorizationRoleDefinitionData(resourceId: resourceId, name: name);
            ResourceIdentifier resourceIdentifier = RoleAssignmentResource.CreateResourceIdentifier(resourceId, "7e378bdc-84ac-4202-bd90-90b9392e6bfe");
            RoleAssignmentResource roleAssignmentResource = armClient.GetRoleAssignmentResource(resourceIdentifier);
            RoleAssignmentResource roleAssignmentResource2 = await roleAssignmentResource.GetAsync();
            RoleAssignmentData roleAssignmentData = roleAssignmentResource2.Data;
            Console.WriteLine($"Id: {roleAssignmentData.Id} Scope: {roleAssignmentData.Scope} Display Name: {roleAssignmentData.Name}");
            return roleAssignmentData;
        }
        public async Task<RoleAssignmentData> DeleteRoleAssignmentAsync(string resourceId, string name)
        {
            Console.WriteLine($"Deleting Role Assignment Async");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await GetAuthorizationRoleDefinitionData(resourceId: resourceId, name: name);
            ResourceIdentifier resourceIdentifier = RoleAssignmentResource.CreateResourceIdentifier(resourceId, "7e378bdc-84ac-4202-bd90-90b9392e6bfe");
            RoleAssignmentResource roleAssignmentResource = armClient.GetRoleAssignmentResource(resourceIdentifier);
            ArmOperation<RoleAssignmentResource> armOperationRoleAssignmentResource = await roleAssignmentResource.DeleteAsync(WaitUntil.Completed);
            RoleAssignmentResource roleAssignmentResource2 = armOperationRoleAssignmentResource.Value;
            RoleAssignmentData roleAssignmentData = roleAssignmentResource2.Data;
            Console.WriteLine($"Id: {roleAssignmentData.Id} Scope: {roleAssignmentData.Scope} Display Name: {roleAssignmentData.Name}");
            return roleAssignmentData;
        }
    }
}