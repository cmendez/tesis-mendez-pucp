SubProceso lista_Proveedores<-Ordenar_Lista_Proveedores (lista_Proveedores)
FinSubProceso

SubProceso distancia<-Calcular_Distancia_GPS (proveedor,cliente)
FinSubProceso

SubProceso lista_ordenada<-Obtener_Proveedores (servicio)
FinSubProceso

SubProceso lista_Final<-Almacenar_Proveedor (lista_Proveedores)
FinSubProceso

SubProceso lista_Proveedores<-Actualizar_Lista (lista_Proveedores,distancia)
FinSubProceso

Proceso Busqueda_Automatizada_Proveedores
	servicios_pendientes <- S;
	
	Mientras (servicios_pendientes) Hacer
		lista_Proveedores <- Obtener_Proveedores(servicio);
		
		Para Cada proveedor de lista_Proveedores Hacer
			distancia <- Calcular_Distancia_GPS(proveedor,cliente);
			lista_Proveedores <- Actualizar_Lista(lista_Proveedores,distancia);
		FinPara
		
		lista_ordenada <- Ordenar_Lista_Proveedores(lista_Proveedores);
		lista_Final <- Almacenar_Proveedor(lista_ordenada);
	FinMientras
FinProceso

