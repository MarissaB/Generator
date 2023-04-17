using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Generator.Authorization
{
    public class OperationNames
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadAllOperationName = "ReadAll";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string AdministratorsRole = "Administrators";
    }

    public class Operations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = OperationNames.CreateOperationName };
        public static OperationAuthorizationRequirement ReadAll =
            new OperationAuthorizationRequirement { Name = OperationNames.ReadAllOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = OperationNames.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = OperationNames.DeleteOperationName };
    }
}
