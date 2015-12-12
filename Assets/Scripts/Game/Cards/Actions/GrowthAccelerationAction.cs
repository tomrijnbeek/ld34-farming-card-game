using UnityEngine;
using System.Collections;

public class GrowthAccelerationAction : UndoableAction {

    public float factor;

    public override void Do(Tile[] tiles)
    {
        foreach (var t in tiles)
            t.growthRate *= factor;
    }

    public override void Undo(Tile[] tiles)
    {
        foreach (var t in tiles)
            t.growthRate /= factor;
    }
}
