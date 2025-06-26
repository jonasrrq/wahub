# Role Management System Documentation

## Overview

WaHub now includes a comprehensive role-based access control (RBAC) system that allows administrators to manage user roles and permissions throughout the application.

## Default Roles

The system comes with three pre-configured roles:

- **Admin**: Full system access, including role management
- **Manager**: Mid-level permissions (to be defined based on business needs)
- **User**: Standard user permissions

## Default Admin User

A default administrator account is automatically created:
- **Email**: admin@wahub.com
- **Password**: Admin123!
- **Role**: Admin

*Note: Change the default password immediately after first login.*

## Role Management Features

### For Administrators

Administrators can access the Role Management page via the sidebar navigation under the "Administration" section.

**Available Operations:**
- View all roles and their user counts
- Create new roles
- Edit existing role names and descriptions
- Delete roles (only if no users are assigned)
- View users assigned to each role

### API Endpoints

The system provides RESTful API endpoints for role management:

```
GET    /api/role                    - Get all roles
GET    /api/role/{id}              - Get role by ID
POST   /api/role                   - Create new role
PUT    /api/role/{id}              - Update role
DELETE /api/role/{id}              - Delete role
POST   /api/role/assign            - Assign role to user
DELETE /api/role/assign            - Remove role from user
GET    /api/role/user/{userId}     - Get user's roles
GET    /api/role/{roleName}/users  - Get users in role
```

All role management endpoints require Admin role authorization.

## Technical Implementation

### Authentication & Authorization

- Roles are integrated with ASP.NET Core Identity
- Claims-based authentication includes role claims
- Authorization policies restrict access to admin-only features
- Role information is persisted across client/server interactions

### Database Schema

The system uses ASP.NET Core Identity's standard role tables:
- `AspNetRoles`: Role definitions
- `AspNetUserRoles`: User-role relationships
- `AspNetRoleClaims`: Role-based claims (if needed)

### Testing

The role system includes comprehensive unit tests covering:
- Role creation and validation
- Role assignment and removal
- User role queries
- Error handling for edge cases

Run tests with: `dotnet test`

## Security Considerations

1. **Access Control**: Only Admin users can manage roles
2. **Validation**: Role names must be unique and non-empty
3. **Safe Deletion**: Roles with assigned users cannot be deleted
4. **Audit Trail**: Consider implementing logging for role changes

## Usage Examples

### Programmatic Role Assignment

```csharp
// Inject IRoleService
await roleService.AssignRoleToUserAsync(userId, "Manager");

// Check if user has role
bool isAdmin = await roleService.UserIsInRoleAsync(userId, "Admin");

// Get all user roles
var userRoles = await roleService.GetUserRolesAsync(userId);
```

### UI Authorization

```razor
<AuthorizeView Roles="Admin">
    <Authorized>
        <!-- Admin-only content -->
    </Authorized>
</AuthorizeView>
```

### Controller Authorization

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    // Admin-only actions
}
```

## Future Enhancements

Consider implementing:
- Permission-based authorization (in addition to roles)
- Role hierarchies
- Temporary role assignments
- Role assignment approval workflows
- Audit logging for role changes
- Bulk user role operations

## Troubleshooting

**Issue**: Role Management page not visible
- **Solution**: Ensure user has Admin role and is properly authenticated

**Issue**: Cannot delete role
- **Solution**: Remove all users from the role first, then delete

**Issue**: Default admin user not created
- **Solution**: Check database migrations ran successfully and RoleSeeder executed

**Issue**: Role claims not working
- **Solution**: Verify authentication providers include role claims in UserSession and ClientAuthStateProvider

## Support

For technical support or questions about the role management system, consult the development team or check the test suite for usage examples.