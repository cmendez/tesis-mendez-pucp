Proceso Busqueda_Automatizada_Proveedores()
{
	//inicialización
	cliente := C;
	lista_servicios := S;
	
	//para cada servicio
	for (i := 1 to lista_servicios.Count)
	{
		//Obtener lista preordenada de la base de datos
		lista_proveedores := Obtener_Proveedores(lista_servicios[i]);
		
		//para cada proveedor
		for(j := 1 to lista_proveedores.Count)
		{
			//calculamos su distanciamiento
			distancia := Calcular_Distancia_GPS(lista_proveedores[j].Ubicacion, cliente.Ubicacion);
			lista_proveedores[j].Distancia := distancia;
		}		
		//Ordenar lista de mejor a peor y tomar primer elemento
		QS_Proveedores(lista_Proveedores);
		lista_Final[i] := lista_Proveedores[0];
	}
}

Subproceso Obtener_Proveedores(Servicio T)
{
	query := "Select * From Proveedores P, ServiciosProveedor SP" +
				"Where SP.Servicio = T And P.Habilitado = 'True'" +
				"Order By P.Puntuacion";
	
	lista_proveedores := Ejecutar_Query(query);	
	retornar lista_proveedores;
}

Subproceso Calcular_Distancia_GPS(Ubicacion X, Ubicacion Y)
{
	R = 6371; // radio en km
	dLat = (Y.lat2 - X.lat1).toRadians();
	dLon = (Y.lon2 - X.lon1).toRadians();
	lat1 = X.lat1.toRadians();
	lat2 = Y.lat2.toRadians();

	a = sin(dLat/2) * sin(dLat/2) +
			  sin(dLon/2) * sin(dLon/2) * cos(lat1) * cos(lat2); 
	c = 2 * atan2(sqrt(a), sqrt(1-a)); 
	d = R * c;	
	
	distancia := d;
	retornar distancia;
}

Subproceso QS_Proveedores(Lista lista_Proveedores)
{
	
}