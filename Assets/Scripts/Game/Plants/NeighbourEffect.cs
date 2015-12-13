using UnityEngine;
using System.Linq;

public class NeighbourEffect : MonoBehaviourBase {

    public EffectDefinition action;

    void PlantPlanted(Plant p) {
        var tile = GetComponentInParent<Tile>();
        if (tile != null)
            action.Do(tile.AdjacentTiles().ToArray());
    }

    void PlantFinished(Plant p) {
        var tile = GetComponentInParent<Tile>();
        if (tile != null)
            action.Undo(tile.AdjacentTiles().ToArray());
    }
}
