using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileInfo : Singleton<TileInfo> {

    public Tile selectedTile;

    public Text title, description;

	// Use this for initialization
	void Start () {
        UpdateInfo();
	}
	
	// Update is called once per frame
	void Update () {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Tile tile = null;

        if (Mathf.Abs(worldPos.x) > 2.5 || Mathf.Abs(worldPos.y) > 2.5) {
            tile = null;
        } else {
            tile = GameManager.Instance.tiles[2 + Mathf.RoundToInt(worldPos.x), 2 + Mathf.RoundToInt(worldPos.y)];
        }

        if (tile != selectedTile) {
            selectedTile = tile;
            UpdateInfo();
        }
	}

    public void UpdateInfo () {
        if (selectedTile == null) {
            title.text = "";
            description.text = "";
            return;
        }

        var plant = selectedTile.plant;

        if (plant == null) {
            title.text = "Empty patch";
            description.text = "";
            return;
        }

        title.text = plant.title;

        var builder = new System.Text.StringBuilder();
        builder.AppendLine("Growth status");
        builder.AppendLine(IndentedLine(string.Format("Growth rate: {0}%", Mathf.RoundToInt(selectedTile.growthRate * 100))));
        builder.AppendLine(IndentedLine(string.Format("Turns left: {0}", Mathf.CeilToInt(
            (plant.maxProgress - plant.currentProgress) / selectedTile.growthRate))));

        if (selectedTile.influences.Count > 0) {
            builder.AppendLine();
            builder.AppendLine("Effects");
            foreach (var i in selectedTile.influences) {
                builder.AppendLine(IndentedLine(i.GetString()));
            }
        }

        description.text = builder.ToString();
    }

    string IndentedLine (string str) {
        return "  " + str;
    }
}
