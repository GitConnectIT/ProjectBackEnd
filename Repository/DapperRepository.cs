using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.Models;
using Repository.Contracts;
using Repository.Extensions.Utility;
using Shared.DTO;
using Shared.RequestFeatures;

namespace Repository;

public class DapperRepository : IDapperRepository
{
    private readonly DapperContext _context;

    public DapperRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<ApplicationMenuDTO> GetMenuById(int id)
    {
        var query = @"
                SELECT *
            FROM ( 
	            SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
				LEFT JOIN MenuPermissions mp ON apm.Id=mp.MenuId
				WHERE apm.Id = @Id
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<ApplicationMenuDTO>(query, new { Id = id });
            return result;
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> AllMenuList()
    {
        var query = @"
               SELECT *
            FROM ( SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
			EXCEPT(
	            SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
				LEFT JOIN MenuPermissions mp ON apm.Id=mp.MenuId
				WHERE apm.Id = mp.MenuId)
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetApplicationMenuAsync(string userRole, int userId)
    {
        var query = @"
            
            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
            WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = @userId AND am.ParentId IS NULL AND p.SubjectId IS NULL

			UNION ALL

			SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN RolePermissions rp ON mp.PermissionId = rp.PermissionId
	            LEFT JOIN AspNetRoles r ON r.Id = rp.RoleId
            WHERE p.IsActive = 1 AND rp.IsActive = 1 AND r.Name = @userRole AND am.ParentId IS NULL AND p.SubjectId IS NULL 
					and rp.PermissionId NOT IN( SELECT up.PermissionId
												FROM MenuPermissions mp
													INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
													INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
													LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
												WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = 74 AND am.ParentId IS NULL AND p.SubjectId IS NULL)

			ORDER BY am.[Order] ASC
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query, new { userRole, userId });
            return result;
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetChidlrenMenuAsync(string userRole, int userId, int parentId)
    {
        var query = @"
            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN RolePermissions rp ON mp.PermissionId = rp.PermissionId
	            LEFT JOIN AspNetRoles r ON r.Id = rp.RoleId
            WHERE p.IsActive = 1 AND rp.IsActive = 1 AND r.Name = @userRole AND am.ParentId = @parentId

            UNION ALL

            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
            WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = @userId AND am.ParentId = @parentId
            ORDER BY am.[Order] ASC
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query, new { userRole, userId, parentId });
            return result;
        }

    }


    
    public async Task<bool> DeleteEmailTemplate(int[] emailTemplateIds)
    {
        var query = @" DELETE FROM EmailTemplate
                        where id in @EmailTemplateIds";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<bool>(query, new { emailTemplateIds });
            return result;
        }
    }
    public async Task<IEnumerable<EmailTemplateDTO>> EmailTemplateTable(LookupRepositoryDTO filter)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            et.Id
		            , et.Name
		            , et.Code
		            , et.Subject
		            , et.Body
		            , et.DateCreated
		            , et.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
		            , et.DateModified
		            , et.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM EmailTemplate et 
		            INNER JOIN AspNetUsers uc ON et.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON et.ModifiedBy = um.Id
            ) result
        ";
        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<EmailTemplateDTO>(query);
            return result;
        }
    }
    public async Task<PagedList<EmailTemplateDTO>> SearchEmailTemplate(LookupRepositoryDTO filter)
    {
        var result = await EmailTemplateTable(filter);

        return PagedList<EmailTemplateDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }
    public async Task<PagedList<ClientListDTO>> SearchClients(LookupRepositoryDTO filter)
    {
        var result = await ClientsTable(filter);

        return PagedList<ClientListDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }



    public async Task<IEnumerable<ClientListDTO>> ClientsTable(LookupRepositoryDTO filter)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            c.Id
		            , c.FirstName
		            , c.LastName
					, CONCAT(c.FirstName,' ' , c.LastName) as FullName
					, c.Email
					, c.State
	            FROM Clients c
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ClientListDTO>(query);
            return result;
        }

    }

}