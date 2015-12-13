using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

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
        var builder = new System.Text.StringBuilder();

        if (plant == null) {
            title.text = "Empty patch";
        } else {
            title.text = plant.title;

            var rate = plant is Mushrooms
                ? ((selectedTile.tileEffects & Tile.TileEffects.Shadow) > 0 ? 1.5f : 1)
                : (plant is Weeds ? 1 : selectedTile.growthRate);

            builder.AppendLine("Growth status");
            if (plant.currentProgress >= plant.maxProgress)
                builder.AppendLine("Fully grown");
            else {
                builder.AppendLine(IndentedLine(string.Format("Growth rate: {0}%", Mathf.RoundToInt(rate * 100))));
                builder.AppendLine(IndentedLine(string.Format("Turns left: {0}", Mathf.CeilToInt(
                    (plant.maxProgress - plant.currentProgress) / rate))));
            }
        }

        if (selectedTile.tileEffects != Tile.TileEffects.None) {
            if (plant != null)
                builder.AppendLine();
            
            foreach (var e in System.Enum.GetValues(typeof(Tile.TileEffects))) {
                if ((selectedTile.tileEffects & (Tile.TileEffects)e) > 0)
                    builder.AppendLine(Regex.Replace(e.ToString(), "(\\B[A-Z])", " $1"));
            }
        }

        if (selectedTile.influences.Count > 0 && !(plant != null && (plant is Mushrooms || plant is Weeds))) {
            if (plant != null)
                builder.AppendLine();
            builder.AppendLine("Effects");
            foreach (var i in selectedTile.influences) {
                builder.AppendLine(IndentedLine(i.GetString()));
            }
        }

        if (plant != null && plant is Mushrooms && (selectedTile.tileEffects & Tile.TileEffects.Shadow) > 0) {
            builder.AppendLine();
            builder.AppendLine("Effects");
            builder.AppendLine("  Shadow: +50%");
        }

        description.text = builder.ToString();
    }

    string IndentedLine (string str) {
        return "  " + str;
    }
}
