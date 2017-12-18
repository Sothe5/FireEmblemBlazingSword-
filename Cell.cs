using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

	public int expected;
	public int current;
	public int heuristic;
	
	public int x;
	public int y;

	public string type;
	public string personType; 
	
	public Cell[] neighbors;
	
	//Probably will remove 'cause doesn't really make too much sense to have this here (A*)
	public Cell previous;
	public Object content;

	public Cell () {
		expected = 0;
		current = 0;
		heuristic = 0;
	}


	public void setPersonType(string types){
		personType = types;
	}

	public void setType(string types){
		type = types;
	}

	public void SetPoint (int a, int b){
		x = a;
		y = b;
	}


	public Cell[] getNeighbors(){
		return neighbors;
	}


	public void addNeighbors (Cell[][] grid, int width, int height){
		neighbors = new Cell[4]; 
		posInNeigh = 0;
		numberOfNeighbours = 0;

		if(x < width){
			neighbors[posInNeigh] = grid[x+1][y];
			posInNeigh++;
			numberOfNeighbours ++;
		}
		if(x > 0){
			neighbors[posInNeigh] = grid[x-1][y];
			posInNeigh++;
			numberOfNeighbours ++;
		}
		if(y < height){
			neighbors[posInNeigh] = grid[x][y+1];
			posInNeigh++;
			numberOfNeighbours ++;
		}
		if(y > 0){
			neighbors[posInNeigh] = grid[x][y-1];
			posInNeigh++;
			numberOfNeighbours ++;
		}
	}

	public int getX(){
		return x;
	}

	public int getY(){
		return y;
	}

	public bool equals (Cell a){
		return x == a.x && y == a.y;
	}

}
