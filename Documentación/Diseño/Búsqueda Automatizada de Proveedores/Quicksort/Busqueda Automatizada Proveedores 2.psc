SubProceso lista_Proveedores <- Ordenar_Lista_Proveedores ( Lista_Proveedores )	
Fin SubProceso
SubProceso distancia <- Calcular_Distancia_GPS ( proveedor, cliente )
Fin SubProceso
SubProceso lista_ordenada <- Obtener_Proveedores ( servicio )
Fin SubProceso
SubProceso lista_Final <- Almacenar_Proveedor ( lista_Proveedores )
Fin SubProceso


Proceso Busqueda_Automatizada_Proveedores
	
	lista_servicios <- S;
	
	Para i<-1 Hasta numero_servicios Hacer
		lista_proveedores <- Obtener_Proveedores(servicio);
			
		Para j<-1 Hasta numero_proveedores Hacer
			distancia <- Calcular_Distancia_GPS(proveedor, cliente);
			proveedor_distancia <- distancia;
		Fin Para
	lista_ordenada <- Ordenar_Lista_Proveedores(lista_Proveedores);
	lista_Final <- Almacenar_Proveedor(lista_ordenada);
	FinPara
	

	
FinProceso