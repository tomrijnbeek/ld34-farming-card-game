using UnityEngine;
using System.Linq;

public class Weeds : Plant {

    public GameObject weedsPrefab;
    GrowthRateInfluence weedsInfluence;

    protected override void ApplyGrowth(float growthRate)
    {
        // No bonuses here.
        currentProgress++;

        // Spread once in every ten turns to adjacent tiles.
        if (currentProgress > maxProgress && Random.value < .1) {
            var tile = GetComponentInParent<Tile>();
            if (tile == null)
                return;
            
            var tiles = tile.AdjacentTiles().Where(t => t.plant == null).ToArray();

            if (tiles.Length > 0) {
                var newTile = tiles[Random.Range(0, tiles.Length)];

                if ((newTile.tileEffects & Tile.TileEffects.WeedProtection) > 0)
                    return;

                var obj = Instantiate(weedsPrefab);
                obj.transform.parent = newTile.transform;
                obj.transform.localPosition = new Vector3(0, 0, -1);
            }
        }
    }

	protected override void FinishedGrowing() {
        SendMessageUpwards("PlantFinished", this);
        // Don't destroy

        var tile = GetComponentInParent<Tile>();
        if (tile == null)
            return;

        foreach (var t in tile.AdjacentTiles())
            t.AddInfluence(weedsInfluence = new GrowthRateInfluence() {
                description = "Adjacent weeds",
                amount = -.2f,
            });
    }

    protected override void DestroyPlant() {
        var tile = GetComponentInParent<Tile>();
        if (tile == null)
            return;

        foreach (var t in tile.AdjacentTiles())
            t.EndInfluence(weedsInfluence);
    }
}
