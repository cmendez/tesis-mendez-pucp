Proceso Busqueda_Automatizada_Proveedores()
{
	//Inicializaci�n
	cliente <- C;
	lista_Servicios <- S; // [][] 
	lista_Proveedores <- GenerarProveedores(lista_Servicios, cliente); //[][]
	lista_Inicial <- GenerarListaInicial(lista_Proveedores);
	lista_Mejores <- listaInicial;
	lista_Tabu <- [];
	M <- ObtenerTama�oMaximo(lista_Proveedores);
	nroTipoServicios <- lista_Servicios.Size;
	maxIteraciones <- nroTipoServicios * M;	
	maxTama�oTabu <- M*2;	
	iteracion <- 0;	
	
	//Cuerpo Principal
	while(iteracion < maxIteraciones)
	{
		//Buscar a proveedor candidato para intercambio seg�n servicio en turno (k)
		lista_Candidatos <- [];
		k <- iteracion MOD nroTipoServicios;
		proveedor <- lista_Mejores[k];
		
		//Buscar vecinos y agregar si no est�n marcados como Tab�
		vecindario <- ObtenerVecindario(lista_Proveedores, proveedor);
		for(candidato in vecindario)
		{
			if (!EsTabu(lista_Tabu, candidato))
			{
				AgregarCandidato(lista_Candidatos, candidato);
			}
		}
		
		//Encontrar �ptimo local
		mejorCandidato <- BuscarOptimoLocal(lista_Candidatos);
		
		//Calculamos el factor total obtenido para ambas listas (original y con reemplazo)
		sumatoriaOriginal <- CalcularSumaFactores(lista_Mejores, null, k);
		sumatoriaIntercambio <- CalcularSumaFactores(lista_Mejores, mejorCandidato, k);
		
		//Actualizar Mejores
		if(sumatoriaIntercambio > sumatoriaOriginal)
		{
			MarcarTabu(lista_Tabu, mejorCandidato);
			lista_Mejores <- ListaMejorada(lista_Mejores, mejorCandidato, k);
			
			//Desmarcar elementos de lista Tab�
			while(Tama�oActual(lista_Tabu) > maxTama�oTabu)
				lista_Tabu <- DesmarcarTabu(lista_Tabu);
		}	
		
		iteracion++;
	}
	
	return lista_Mejores;
}