using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInitial : MonoBehaviour {


	private Map myMap;

	void Start () {
		
		myMap = FindObjectOfType<Map> ();

		myMap.matrixScenary [0] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [0] [8].setType (Types.Ocupaid);
		myMap.matrixScenary [2] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [2] [8].setType (Types.Ocupaid);
		myMap.matrixScenary [3] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [3] [8].setType (Types.Ocupaid);
		myMap.matrixScenary [4] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [4] [8].setType (Types.Ocupaid);
		myMap.matrixScenary [7] [7].setType (Types.Ocupaid);
		myMap.matrixScenary [8] [6].setType (Types.Ocupaid);
		myMap.matrixScenary [11] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [11] [8].setType (Types.Ocupaid);
		myMap.matrixScenary [11] [6].setType (Types.Ocupaid);
		myMap.matrixScenary [11] [5].setType (Types.Ocupaid);
		myMap.matrixScenary [12] [5].setType (Types.Ocupaid);
		myMap.matrixScenary [12] [3].setType (Types.Ocupaid);
		myMap.matrixScenary [13] [3].setType (Types.Ocupaid);
		myMap.matrixScenary [13] [2].setType (Types.Ocupaid);
		myMap.matrixScenary [13] [1].setType (Types.Ocupaid);
		myMap.matrixScenary [14] [1].setType (Types.Ocupaid);
		myMap.matrixScenary [10] [0].setType (Types.Ocupaid);
		myMap.matrixScenary [11] [0].setType (Types.Ocupaid);
		myMap.matrixScenary [12] [0].setType (Types.Ocupaid);
		myMap.matrixScenary [13] [0].setType (Types.Ocupaid);
		myMap.matrixScenary [14] [0].setType (Types.Ocupaid);
		myMap.matrixScenary [12] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [13] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [14] [9].setType (Types.Ocupaid);
		myMap.matrixScenary [14] [8].setType (Types.Ocupaid);



		// cambiar modo manual para que el contador vaya bien

		myMap.matrixScenary [8] [4].setTerrainType (Types.Forest);
		myMap.matrixScenary [8] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [7] [5].setTerrainType (Types.Forest);
		myMap.matrixScenary [9] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [9] [2].setTerrainType (Types.Forest);
		myMap.matrixScenary [6] [7].setTerrainType (Types.Forest);
		myMap.matrixScenary [5] [9].setTerrainType (Types.Forest);
		myMap.matrixScenary [10] [7].setTerrainType (Types.Forest);
		myMap.matrixScenary [3] [7].setTerrainType (Types.Forest);
		myMap.matrixScenary [6] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [6] [2].setTerrainType (Types.Forest);
		myMap.matrixScenary [6] [1].setTerrainType (Types.Forest);
		myMap.matrixScenary [6] [0].setTerrainType (Types.Forest);
		myMap.matrixScenary [7] [1].setTerrainType (Types.Forest);
		myMap.matrixScenary [5] [1].setTerrainType (Types.Forest);
		myMap.matrixScenary [8] [0].setTerrainType (Types.Forest);
		myMap.matrixScenary [2] [5].setTerrainType (Types.Forest);
		myMap.matrixScenary [0] [6].setTerrainType (Types.Forest);
		myMap.matrixScenary [3] [4].setTerrainType (Types.Forest);
		myMap.matrixScenary [3] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [3] [2].setTerrainType (Types.Forest);
		myMap.matrixScenary [3] [1].setTerrainType (Types.Forest);
		myMap.matrixScenary [2] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [0] [3].setTerrainType (Types.Forest);
		myMap.matrixScenary [0] [2].setTerrainType (Types.Forest);
		myMap.matrixScenary [1] [0].setTerrainType (Types.Forest);
		myMap.matrixScenary [5] [5].setTerrainType (Types.Forest);

		myMap.matrixScenary [8] [4].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [8] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [7] [5].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [9] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [9] [2].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [6] [7].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [5] [9].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [10] [7].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [3] [7].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [6] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [6] [2].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [6] [1].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [6] [0].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [7] [1].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [5] [1].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [8] [0].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [2] [5].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [0] [6].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [3] [4].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [3] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [3] [2].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [3] [1].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [2] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [0] [3].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [0] [2].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [1] [0].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");
		myMap.matrixScenary [5] [5].platformSprite = Resources.Load<Sprite> ("Platforms/bosque");


		myMap.matrixScenary [7] [9].setTerrainType (Types.Mountain);
		myMap.matrixScenary [8] [9].setTerrainType (Types.Mountain);
		myMap.matrixScenary [9] [9].setTerrainType (Types.Mountain);

		myMap.matrixScenary [7] [9].platformSprite = Resources.Load<Sprite> ("Platforms/montaña");
		myMap.matrixScenary [8] [9].platformSprite = Resources.Load<Sprite> ("Platforms/montaña");
		myMap.matrixScenary [9] [9].platformSprite = Resources.Load<Sprite> ("Platforms/montaña");

		myMap.matrixScenary [12] [4].platformSprite = Resources.Load<Sprite> ("Platforms/puente");
		myMap.matrixScenary [11] [7].platformSprite = Resources.Load<Sprite> ("Platforms/puente");

		myMap.matrixScenary [0] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [0] [8].setTerrainType (Types.Blocked);
		myMap.matrixScenary [2] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [2] [8].setTerrainType (Types.Blocked);
		myMap.matrixScenary [3] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [3] [8].setTerrainType (Types.Blocked);
		myMap.matrixScenary [4] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [4] [8].setTerrainType (Types.Blocked);
		myMap.matrixScenary [7] [7].setTerrainType (Types.Blocked);
		myMap.matrixScenary [8] [6].setTerrainType (Types.Blocked);
		myMap.matrixScenary [11] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [11] [8].setTerrainType (Types.Blocked);
		myMap.matrixScenary [11] [6].setTerrainType (Types.Blocked);
		myMap.matrixScenary [11] [5].setTerrainType (Types.Blocked);
		myMap.matrixScenary [12] [5].setTerrainType (Types.Blocked);
		myMap.matrixScenary [12] [3].setTerrainType (Types.Blocked);
		myMap.matrixScenary [13] [3].setTerrainType (Types.Blocked);
		myMap.matrixScenary [13] [2].setTerrainType (Types.Blocked);
		myMap.matrixScenary [13] [1].setTerrainType (Types.Blocked);
		myMap.matrixScenary [14] [1].setTerrainType (Types.Blocked);
		myMap.matrixScenary [10] [0].setTerrainType (Types.Blocked);
		myMap.matrixScenary [11] [0].setTerrainType (Types.Blocked);
		myMap.matrixScenary [12] [0].setTerrainType (Types.Blocked);
		myMap.matrixScenary [13] [0].setTerrainType (Types.Blocked);
		myMap.matrixScenary [14] [0].setTerrainType (Types.Blocked);
		myMap.matrixScenary [12] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [13] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [14] [9].setTerrainType (Types.Blocked);
		myMap.matrixScenary [14] [8].setTerrainType (Types.Blocked);


	
		myMap.SetValue (12, 8, Types.Allie);
		myMap.SetValue (14, 7, Types.Allie);


		myMap.SetValue (2, 3, Types.Enemy);
		myMap.matrixScenary [2] [3].setType (Types.Ocupaid);
		myMap.SetValue (1, 4, Types.Enemy);
		myMap.matrixScenary [1] [4].setType (Types.Ocupaid);
		myMap.SetValue (0, 3, Types.Enemy);
		myMap.matrixScenary [0] [3].setType (Types.Ocupaid);
		myMap.SetValue (0, 5, Types.Enemy);
		myMap.matrixScenary [0] [5].setType (Types.Ocupaid);
		myMap.SetValue (1, 1, Types.Enemy);
		myMap.matrixScenary [1] [1].setType (Types.Ocupaid);
		myMap.SetValue (4, 0, Types.Enemy);
		myMap.matrixScenary [4] [0].setType (Types.Ocupaid);
	}
	

}
