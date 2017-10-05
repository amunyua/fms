CREATE VIEW AllUsers As
-- DROP VIEW AllUsers;
SELECT u.*,
UR.RoleId AS Rid,
R.RoleName


FROM AspNetUsers u
LEFT JOIN UserRoles UR ON u.Id = UR.UserId
LEFT JOIN Roles R ON R.RoleId = UR.RoleId
GO
