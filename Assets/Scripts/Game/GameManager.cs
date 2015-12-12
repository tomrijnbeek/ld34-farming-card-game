using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public GameObject tilePrefab;

	// Use this for initialization
	void Start () {
        var tiles = new Tile[5,5];

        for (int j = 0; j < 5; j++)
            for (int i = 0; i < 5; i++) {
                var obj = Instantiate(tilePrefab);
                obj.transform.position = new Vector3(i - 2, j - 2, 0);
                tiles[i,j] = obj.GetComponent<Tile>();
                if (i > 0) {
                    tiles[i-1,j].right = tiles[i,j];
                    tiles[i,j].left = tiles[i-1,j];
                }
                if (j > 0) {
                    tiles[i,j-1].top = tiles[i,j];
                    tiles[i,j].bottom = tiles[i,j-1];
                }
            }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
