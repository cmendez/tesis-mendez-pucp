Proceso Busqueda_Automatizada_Proveedores()
{
	//Inicialización
	cliente <- C;
	lista_Servicios <- S; // [][] 
	lista_Proveedores <- GenerarProveedores(lista_Servicios, cliente); //[][]
	lista_Inicial <- GenerarListaInicial(lista_Proveedores);
	lista_Mejores <- listaInicial;
	lista_Tabu <- [];
	M <- ObtenerTamañoMaximo(lista_Proveedores);
	nroTipoServicios <- lista_Servicios.Size;
	maxIteraciones <- nroTipoServicios * M;	
	maxTamañoTabu <- M*2;	
	iteracion <- 0;	
	
	//Cuerpo Principal
	while(iteracion < maxIteraciones)
	{
		//Buscar a proveedor candidato para intercambio según servicio en turno (k)
		lista_Candidatos <- [];
		k <- iteracion MOD nroTipoServicios;
		proveedor <- lista_Mejores[k];
		
		//Buscar vecinos y agregar si no están marcados como Tabú
		vecindario <- ObtenerVecindario(lista_Proveedores, proveedor);
		for(candidato in vecindario)
		{
			if (!EsTabu(lista_Tabu, candidato))
			{
				AgregarCandidato(lista_Candidatos, candidato);
			}
		}
		
		//Encontrar óptimo local
		mejorCandidato <- BuscarOptimoLocal(lista_Candidatos);
		
		//Calculamos el factor total obtenido para ambas listas (original y con reemplazo)
		sumatoriaOriginal <- CalcularSumaFactores(lista_Mejores, null, k);
		sumatoriaIntercambio <- CalcularSumaFactores(lista_Mejores, mejorCandidato, k);
		
		//Actualizar Mejores
		if(sumatoriaIntercambio > sumatoriaOriginal)
		{
			MarcarTabu(lista_Tabu, mejorCandidato);
			lista_Mejores <- ListaMejorada(lista_Mejores, mejorCandidato, k);
			
			//Desmarcar elementos de lista Tabú
			while(TamañoActual(lista_Tabu) > maxTamañoTabu)
				lista_Tabu <- DesmarcarTabu(lista_Tabu);
		}	
		
		iteracion++;
	}
	
	return lista_Mejores;
}