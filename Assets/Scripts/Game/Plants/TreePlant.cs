using UnityEngine;
using System.Collections;

public class TreePlant : Plant {
    protected override void FinishedGrowing() {
        SendMessageUpwards("PlantFinished", this);
        GameManager.Instance.currency += score;
        // Don't destroy
    }

    protected override void DestroyPlant()
    {
        var tile = GetComponentInParent<Tile>();
        foreach (var t in tile.AdjacentTiles())
            t.RemoveEffect(Tile.TileEffects.Shadow);

        base.DestroyPlant();
    }
}
