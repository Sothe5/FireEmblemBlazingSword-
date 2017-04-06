using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

	public int f;
	public int g;
	public int h;
	public int x;
	public int y;
	public Spot[] neighbors;
	public int posInNeigh;
	public Spot previous;
	public int numNeigh;
	public string type;
	public string personType; 



	void Start () {
		
		f = 0;
		g = 0;
		h = 0;
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

	public int getF(){
		return f;
	}

	public Spot[] getNeighbors(){
		return neighbors;
	}

	public int getG(){
		return g;
	}

	public int getH(){
		return h;
	}

	public int getX(){
		return x;
	}

	public int getY(){
		return y;
	}

	public void addNeighbors (Spot[][] grid, int Right, int Up){
		neighbors = new Spot[4]; 
		posInNeigh = 0;
		numNeigh = 0;

		if(x < Right){
			neighbors[posInNeigh] = grid[x+1][y];
			posInNeigh++;
			numNeigh ++;
		}
		if(x > 0){
			neighbors[posInNeigh] = grid[x-1][y];
			posInNeigh++;
			numNeigh ++;
		}
		if(y < Up){
			neighbors[posInNeigh] = grid[x][y+1];
			posInNeigh++;
			numNeigh ++;
		}
		if(y > 0){
			neighbors[posInNeigh] = grid[x][y-1];
			posInNeigh++;
			numNeigh ++;
		}
	}

	public bool equal (Spot a){
		return x == a.x && y == a.y;
	}

}
