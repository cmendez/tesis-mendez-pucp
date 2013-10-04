USE [SistemaGeneraliz];
SELECT P.PersonaId, Pro.ProveedorId, P.UserName, P.TipoUsuario, P.PrimerNombre + ' ' + P.ApellidoPaterno as 'Nombre',
P.RazonSocial, S.NombreServicio, Pro.PuntuacionPromedio, P.DireccionCompleta, U.Latitud, U.Longitud
FROM Personas P, Proveedores Pro, UbicacionesPersonas U, TiposServicios S, TiposServiciosPorProveedor SXP
WHERE P.PersonaId = Pro.PersonaId AND U.PersonaId = P.PersonaId AND SXP.ProveedorId = PRO.ProveedorId
AND SXP.TipoServicioId = S.TipoServicioId 
--AND S.NombreServicio IN ('Carpintería','Electricidad','Pintura')
--AND S.NombreServicio IN ('Electricidad')
ORDER BY S.NombreServicio asc, Pro.PuntuacionPromedio desc;