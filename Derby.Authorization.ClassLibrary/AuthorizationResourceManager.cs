using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Authorization;
using Azure.Core;
using Azure.ResourceManager.Authorization.Models;
using Azure;
using Azure.ResourceManager.Resources;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.Graph;
using static System.Formats.Asn1.AsnWriter;

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
        public async Task<List<AuthorizationRoleDefinitionData>> GetListAuthorizationRoleDefinitionDataAsync(string resourceId)
        {
            Console.WriteLine($"Getting Role Assignment Collection");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(resourceId);
            AuthorizationRoleDefinitionCollection authorizationRoleDefinitionCollection = armClient.GetAuthorizationRoleDefinitions(resourceIdentifier);
            List<AuthorizationRoleDefinitionData> authorizationRoleDefinitionDataList = new List<AuthorizationRoleDefinitionData>();
            await foreach (AuthorizationRoleDefinitionResource item in authorizationRoleDefinitionCollection.GetAllAsync())
            {
                authorizationRoleDefinitionDataList.Add(item.Data);
            }
            Console.WriteLine($"Count: {authorizationRoleDefinitionDataList.Count()}");
            return authorizationRoleDefinitionDataList;
        }
        private async Task<AuthorizationRoleDefinitionData> GetAuthorizationRoleDefinitionData(string resourceId, string name)
        {
            Console.WriteLine($"Getting Authorization Role Definition Data");
            List<AuthorizationRoleDefinitionData> authorizationRoleDefinitionDataList = await GetListAuthorizationRoleDefinitionDataAsync(resourceId);
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = authorizationRoleDefinitionDataList.Where(authorizationRoleDefinitionData => authorizationRoleDefinitionData.RoleName.Equals(name)).First();
            Console.WriteLine($"Id: {authorizationRoleDefinitionData.Id} Name: {authorizationRoleDefinitionData.RoleName} Description: {authorizationRoleDefinitionData.Description} Type: {authorizationRoleDefinitionData.ResourceType}");
            return authorizationRoleDefinitionData;
        }
        public async Task<RoleAssignmentData> GetRoleAssignmentCreateOrUpdateContentAsync(string resourceId, string name)
        {
            Console.WriteLine($"Creating Role Assignment Create Or Update Content Async");
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
            Console.WriteLine($"[Id]: {roleAssignmentData.Id} [Scope]: {roleAssignmentData.Scope} [Display Name]: {roleAssignmentData.Name}");
            return roleAssignmentData;
        }

       

    }
}
