using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : Singleton<GameManager> {

    public GameObject tilePrefab;
    public GameObject weedPrefab;
    public Tile[,] tiles;

    public int currency;

    public bool growNow;
    public bool gameOver;

    public UnityEngine.UI.Text currencyText; 

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

        // Some initial weeds.
        var tile = tiles[Random.Range(0, 5), Random.Range(0,5)];

        var weedObj = Instantiate(weedPrefab);
        weedObj.transform.parent = tile.transform;
        weedObj.transform.localPosition = new Vector3(0, 0, -1);
	}

    public void GrowthStep() {
        // Some free moneyz
        currency++;

        // Grow plants
        foreach (var t in this.tiles) {
            t.DoGrowthStep();
        }

        // Process effect durations
        EffectManager.Instance.GrowthStep();

        // Spawn weed every 20 turns or so.
        if (Random.value < .05) {
            var eligibleTiles = tiles.Cast<Tile>().Where(t => t.plant == null).ToArray();
            if (eligibleTiles.Length > 0) {
                var tile = eligibleTiles[Random.Range(0, eligibleTiles.Length)];
                if ((tile.tileEffects & Tile.TileEffects.WeedProtection) == 0) {
                    // Actually spawn.
                    var weedObj = Instantiate(weedPrefab);
                    weedObj.transform.parent = tile.transform;
                    weedObj.transform.localPosition = new Vector3(0, 0, -1);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (growNow) {
            GrowthStep();
            growNow = false;
        }

        currencyText.text = "$ " + currency;
	}

    void LateUpdate () {
        // Maybe game over.
        if (Hand.Instance.cards.All(c => !c.usable)) {
            GameOver ();
        }
    }

    public void ResetHighlightedTiles () {
        foreach (var t in tiles)
            t.Highlight(Tile.Selection.None);
    }

    void GameOver () {
        if (!gameOver) {
            Debug.Log("Game over");
            gameOver = true;
        }
    }
}
