Proceso Busqueda_Automatizada_Proveedores()
{
	//Inicialización
	cliente <- C;
	lista_Servicios <- S; // [][] 
	lista_Proveedores <- GenerarListaProveedores(lista_Servicios); //[][]
	lista_Inicial <- GenerarListaInicial(lista_Proveedores);
	lista_Mejores <- lista_Inicial;
	lista_Tabu <- [];
	M <- ObtenerTamañoMaximo(lista_Proveedores);
	nroTipoServicios <- lista_Servicios.Size;
	maxIteraciones <- nroTipoServicios * M;	
	maxTamañoTabu <- M * 2;	
	iteracion <- 0;	
	
	//Cuerpo Principal
	while(iteracion < maxIteraciones)
	{
		//Buscar a proveedor candidato para intercambio según servicio en turno (k)
		lista_Candidatos <- [];
		k <- iteracion MOD nroTipoServicios;
		proveedor <- lista_Mejores[k];
		
		//Buscar vecinos y agregar si no están marcados como Tabú
		vecindario <- ObtenerVecindario(lista_Proveedores, proveedor, k);
		foreach(candidato in vecindario)
		{
			if (!EsTabu(lista_Tabu, candidato))
			{
				lista_Candidatos <- AgregarCandidato(lista_Candidatos, candidato);
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
			lista_Tabu <- MarcarTabu(lista_Tabu, mejorCandidato);
			lista_Mejores <- ListaMejorada(lista_Mejores, mejorCandidato, k);
			
			//Desmarcar elementos de lista Tabú
			while(TamañoActual(lista_Tabu) > maxTamañoTabu)
				lista_Tabu <- DesmarcarTabu(lista_Tabu);
		}	
		
		iteracion++;
	}
	
	return lista_Mejores;
}

SubProceso GenerarListaProveedores(lista_Servicios)
{
	cantidadMaxima <- 100; //podría ser configurable
	puntajeMinimo <- 14; //podría ser configurable
	i <- 0;
	lista_Proveedores <- [][];
	
	//Para cada servicio requerido
	foreach(servicio in lista_Servicios)
	{
		//Obtenemos proveedores directamente de la BD 
		//ordenados por puntaje de mayor a menor
		lista_Proveedores[i] <- ObtenerProveedoresBD(servicio, cantidadMaxima, puntajeMinimo);
		i++;
	}
	
	return lista_Proveedores;
}

SubProceso GenerarListaInicial(lista_Proveedores)
{
	lista_Inicial <- [];
	i <- 0;
	
	//Para cada sublista de proveedores
	foreach(sublista in lista_Proveedores)
	{
		//Obtener el primer proveedor y almacenarlo
		proveedor <- sublista[0]; 
		lista_Inicial[i] <- proveedor;
		i++
	}
	
	return lista_Inicial;
}

SubProceso ObtenerTamañoMaximo(lista_Proveedores)
{
	mayor <- 0;
	//Para cada sublista de proveedores
	foreach(sublista in lista_Proveedores)
	{
		//Obtener el primer proveedor y almacenarlo
		cantidad <- sublista.Cantidad;
		
		if (cantidad > mayor)
			mayor <- cantidad;
	}
}

SubProceso ObtenerVecindario(lista_Proveedores, proveedor, k)
{
	vecindario <- [];
	lista <- lista_Proveedores[k];	//la lista viene preordenada por puntaje
	np <- lista.Count;
	
	//Para cada proveedor
	for(i <- 1 to np)
	{
		if (lista[i] != proveedor)
		{			
			//Calculamos el distanciamiento de cada potencial 
			//vecino respecto del proveedor de turno
			distancia <- Calcular_Distancia_GPS(lista[i].Ubicacion, proveedor.Ubicacion);
			vecindario[i] <- lista[i];
			vecindario[i].Distancia <- distancia;			
		}
	}	

	return vecindario;
}

SubProceso EsTabu(lista_Tabu, candidato)
{
	pertenece <- false;
	
	foreach(proveedor in lista_Tabu)
	{
		if (proveedor == candidato)
		{
			pertenece <- true;
			break;
		}
	}
	return pertenece;
}

SubProceso AgregarCandidato(lista_Candidatos, candidato)
{
	n <- lista_Candidatos.Count + 1;
	lista_Candidatos[n] <- candidato;
	return lista_Candidatos;
}

SubProceso BuscarOptimoLocal(lista_Candidatos)
{
	n <- lista_Candidatos.Count;
	
	foreach(candidato in lista_Candidatos)
	{
		candidato.Factor <- (candidato.Puntaje / candidato.distancia);
	}
	
	//Ordenar lista por factor P/D y tomar primer elemento
	lista_Candidatos <- QS_Proveedores(lista_Candidatos, 0, n - 1);
	mejorCandidato <- lista_Candidatos[0];
	
	return mejorCandidato;
}

Subproceso QS_Proveedores(lista_Candidatos, izq, der)
{
	a <- lista_Candidatos;
	i <- izq;
	j <- der;
	pivot <- ((izq + der) / 2);
	x <- a[pivot].Factor;

	while (i <= j)
	{
		while (a[i].Factor > x)
		{
			i++;
		}
		while (a[j].Factor < x)
		{
			j--;
		}
		if (i <= j)
		{
			temp <- a[i];
			a[i] <- a[j];
			a[j] <- temp;
			
			i++;
			j--;
		}
	}
	if (izq < j)
	{
		QuickSort(a, izq, j);
	}
	if (i < der)
	{
		QuickSort(a, i, der);
	}
}

SubProceso CalcularSumaFactores(lista_Mejores, mejorCandidato, k)
{
	suma <- 0;	
	i <- 0;
	
	foreach(proveedor in lista_Mejores)
	{
		//Solo para proveedor en posición 'k' y si no es 'null'
		if ((k == i) && (mejorCandidato != null))
		{
			factor <- mejorCandidato.Factor;
		}
		else
		{
			factor <- proveedor.Factor;
		}
		
		suma <- suma + factor;
		i++;
	}

	sumatoriaIntercambio <- suma;
	return sumatoriaIntercambio;
}

SubProceso MarcarTabu(lista_Tabu, mejorCandidato)
{
	//Marcamos como tabu a un proveedor, agregándolo a la lista Tabú
	n <- lista_Tabu.Count + 1;
	lista_Tabu[n] <- mejorCandidato;
	return lista_Tabu;
}

SubProceso ListaMejorada(lista_Mejores, mejorCandidato, k)
{
	//Reemplazamos al provedor en posicion 'k' por el nuevo mejor
	lista_Mejores[k] <- mejorCandidato;
	return lista_Mejores;
}

SubProceso TamañoActual(lista_Tabu)
{
	return lista_Tabu.Count;
}

SubProceso DesmarcarTabu(lista_Tabu)
{
	inicio <- 2; //A partir del 2do elemento; el primero deja de ser Tabú
	fin <- lista_Tabu.Count;
	nueva_lista <- [];
	j <- 1;
	
	//Copiamos los elementos de la lista Tabú desde el 2do elemento, hasta el final
	for(i <- inicio to fin)
	{		
		nueva_lista[j] <- lista_Tabu[i];
		i++;
		j++;
	}
	
	return nueva_lista;
}







