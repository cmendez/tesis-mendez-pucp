SubProceso lista_Proveedores <- Ordenar_Lista_Proveedores ( Lista_Proveedores )	
Fin SubProceso
SubProceso distancia <- Calcular_Distancia_GPS ( proveedor, cliente )
Fin SubProceso
SubProceso lista_ordenada <- Obtener_Proveedores ( servicio )
Fin SubProceso
SubProceso lista_Final <- Almacenar_Proveedor ( lista_Proveedores )
Fin SubProceso

Proceso Busqueda_Automatizada_Proveedores
	lista_Servicios_Requeridos <- S;
	
	Para Cada servicio en lista_Servicios_Requeridos
		lista_Proveedores <- Obtener_Proveedores(servicio);
		
		Para Cada proveedor en lista_Proveedores			
			distancia <- Calcular_Distancia_GPS(proveedor, cliente);
			proveedor_distancia <- distancia;
		FinPara
	lista_ordenada <- Ordenar_Lista_Proveedores(lista_Proveedores);
	lista_Final <- Almacenar_Proveedor(lista_ordenada);
	FinPara
	
	Escribir lista_Final;
FinProceso
