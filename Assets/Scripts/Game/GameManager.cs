using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public GameObject tilePrefab;
    public Tile[,] tiles;

    public bool growNow;

	// Use this for initialization
	void Start () {
        tiles = new Tile[5,5];

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

    void GrowthStep() {
        foreach (var t in this.tiles) {
            t.DoGrowthStep();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (growNow) {
            GrowthStep();
            growNow = false;
        }
	}

    public void ResetHighlightedTiles () {
        foreach (var t in tiles)
            t.Highlight(false);
    }
}
