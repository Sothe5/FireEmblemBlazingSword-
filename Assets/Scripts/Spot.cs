using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

	public int f;
	public int g;
	public int h;
	public int x;
	public int y;
	public int distancia;
	public Spot previous;
	public string type;
	public string personType;
	public string terrainType;
	public Sprite platformSprite;

	public Spot(){

	}

	void Start () {
		
		f = 0;
		g = 0;
		h = 0;
	}
		
	public void setPersonType(string types){
		personType = types;
	}

	public void setTerrainType(string types){
		terrainType = types;
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

	public List<Spot> getNeighbors(Spot[][] grid, int Right, int Up){
		List<Spot> sol = new List<Spot> ();
		if(x < Right){
			sol.Add (grid[x+1][y]);
		}
		if(x > 0){
			sol.Add (grid[x-1][y]);
		}
		if(y < Up){
			sol.Add (grid[x][y+1]);
		}
		if(y > 0){
			sol.Add (grid[x][y-1]);
		}

		return sol;
	}

	public List<Spot> getFarNeighbors(Spot[][] grid, int Right, int Up){
		List<Spot> sol = new List<Spot> ();
		if(x+1 < Right)
			sol.Add (grid[x+2][y]);

		if(x-1 > 0)
			sol.Add (grid[x-2][y]);

		if(y+1 < Up)
			sol.Add (grid[x][y+2]);

		if(y-1 > 0)
			sol.Add (grid[x][y-2]);

		if(x < Right && y < Up)
			sol.Add (grid[x+1][y+1]);
		
		if(x < Right && y > 0)
			sol.Add (grid[x+1][y-1]);

		if(x > 0 && y < Up)
			sol.Add (grid[x-1][y+1]);

		if(x > 0 && y > 0)
			sol.Add (grid[x-1][y-1]);

		return sol;
	}

	public override bool Equals(System.Object o){
		Spot a = o as Spot;
		if (o == null) {
			return false;
		}
		return x == a.x && y == a.y;
	}

	public bool Equals(Spot a){
		if (a == null) {
			return false;
		}
		return x == a.x && y == a.y;
	}

	public override int GetHashCode (){
		return base.GetHashCode ();
	}

	public bool equal (Spot a){
		return x == a.x && y == a.y;
	}
}
