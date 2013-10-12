Proceso Algoritmo_B�squeda_Tabu()
{
	lista_Mejores <- lista_Inicial                                            
	lista_Tabu <- null
	while(!condicion_parada)
	{
		lista_Candidatos <- null
		for(candidato en vecindario)
		{
			if (!EsTabu(lista_Tabu, candidato))			
				lista_Candidatos <- lista_Candidatos + candidato
		}
        mejorCandidato <- BuscarOptimoLocal(lista_Candidatos)
		if(criterios(candidato) > criterios(lista_Mejores))
		{
			lista_Tabu <- MarcarTabu(lista_Tabu, mejorCandidato)
			lista_Mejores <- lista_Mejores + mejorCandidato
			while(Tama�oActual(lista_Tabu) > maxTama�oTabu)
				DesmarcarElementosTabu(lista_Tabu)
		}
	 }
     return(lista_Mejores)
}