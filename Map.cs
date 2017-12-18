using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

	
	public Cell [][] matrixScenary;

	public int width;
	public int height;


	void Start () {

		matrixScenary = new Cell[width+1][];

		for(int i = 0; i <= width; i++){
			matrixScenary[i] = new Cell[height+1];
			for(int j = 0; j <= height; j++){
				matrixScenary[i][j] = gameObject.AddComponent<Cell>(); // == new Cell();
				matrixScenary[i][j].SetPoint(i,j); // dudoso
				matrixScenary[i][j].setType(Types.Free);
				matrixScenary[i][j].setPersonType(null);
			}
		}

		for(int i = 0; i <= width; i++){	// mejorar eficiencia
			for(int j = 0; j <= height; j++){
				matrixScenary[i][j].addNeighbors(matrixScenary,width,height);
			}
		}

	}

	public string GetValue(int a,int b){
		return matrixScenary[a][b].personType;
	}

	public void SetValue(int a, int b, string c){
		matrixScenary[a][b].personType = c;
	}


	public string GetValueScenary(int a,int b){
		return matrixScenary[a][b].type;
	}

	public void EndTurn(){
		Object[] objs = Ally.FindSceneObjectsOfType(typeof(Ally));
		foreach (Ally a in objs){
			a.setDisponibility(true);
		}
	}


	private void RemoveFromArray (Cell[] myArray, Cell elem,int pos){ // quita de open set el Cell prometedor		*****Plantear hacer quicksort y despues binary search

		for(int i = pos-1; i >= 0; i--){ // por como esta definida deberia hacerlo de princi
			if(myArray[i].equal(elem)){
				for(int j = i; j< myArray.Length-1; j++){
					myArray[j] = myArray[j+1];
				}
				break;			
			}
		} 
	}

	private bool isIncluded (Cell a, Cell[] list,int pos){

		for(int i = 0; i < pos; i++){
			if(list[i].equal(a)){
				return true;
			}
		}

		return false;
	}

	private int heuristic (Cell a, Cell b){
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y -b.y);
	}

	private int distance (Ally myAllys,Cell start){

		int myAllyX = Mathf.FloorToInt(myAllys.transform.position.x);
		int myAllyY = Mathf.FloorToInt(myAllys.transform.position.y);

		return Mathf.Abs((myAllyX-start.x)+ (myAllyY - start.y));
	}

	private int distBetween (Cell a,Cell b){
		// cambiar free de cursor a ocuppied con sus implicaciones
		if(b.type == Types.Forest){
		return 2;
		}else{
		return 1;
		}
	}


	public bool canReachCell(Cell start, Cell end, Cell [] path,Cell posInPath){
		
		Ally ally = (Ally)start.content;

		int posInOpen = 0;
		int posInClose = 0;

		start.addNeighbors(matrixScenary,width,height);
		end.addNeighbors(matrixScenary,width,height);

		Cell [] openSet;	//open and close set generated and initiallized
		Cell [] closeSet;
		openSet = new Cell[(width+1)*(height+1)];
		closeSet = new Cell[(width+1)*(height+1)];
		openSet[posInOpen]  = start;
		posInOpen++;
		bool ok = false;

		while (posInOpen > 0 && !ok){

			int prometedor = 0; // buscamos de la lista de los posibles neighbors cual es el que tiene una f mas baja
			for(int i = 0; i < posInOpen; i++){ 
				if(openSet[i].expected < openSet[prometedor].expected && distance(myAlly,openSet[i]) < myAlly.movement){ // la distancia entre inicio y prometedor es menor que myAlly.movement
					prometedor = i;

				}
			}

			Cell currentCell = openSet[prometedor];
			if(currentCell.equal(end)){ // si el prometedor es el final

				 // intentar reducirlo despues
				

				Cell temp = currentCell;
				path[posInPath.x] = temp;
				posInPath.x++;

				while(temp.previous){
					path[posInPath.x] = temp.previous;
					posInPath.x++;
					temp = temp.previous;

				}

				ok = true;
				if(path[0].currentCost > myAlly.movement){ // se deberia comparar la g con el movement de myAlly
					Destroy(start);
					Destroy(end);
					return false;
				}
				Destroy(start);
				Destroy(end);
				return ok;
			}

			RemoveFromArray(openSet,currentCell,posInOpen);
			System.Array.Resize<Cell>(ref openSet,openSet.Length-1);
			posInOpen--;
			closeSet[posInClose] = currentCell; // Add current cell to the visited ones
			posInClose++;


			foreach (Cell cell in currentCell.getNeighbors()) {	
				if(!isIncluded(cell, closeSet, posInClose) && (cell.type != Types.Occupied)){ 
					int costToNeighbour = currentCell.currentCost + distBetween(currentCell, cell); // Distance to neighbour
					bool newPath = false;
					if(isIncluded(cell, openSet,posInOpen)){
						//Update 
						if(costToNeighbour < cell.currentCost){
							cell.currentCost = costToNeighbour;
							newPath = true;
						}
					}else{
						cell.currentCost = costToNeighbour;
						newPath = true;
						openSet[posInOpen] = cell;
						posInOpen++;
					}
					if(newPath){
						cell.heuristicCost = heuristic(cell,end);		
						cell.expectedCost = cell.currentCost +cell.heuristicCost;
						cell.previousCell = currentCell;
					}
				}
			}
		}

		Destroy(start);
		Destroy(end);
		return ok;

		// cuando saliese seria no solucion

	}



}
