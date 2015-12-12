using UnityEngine;
using System.Collections;

public class GrowthAccelerationAction : UndoableAction {

    public float percentage;

    GrowthRateInfluence influence;

    public override void Do(Tile[] tiles)
    {
        influence = new GrowthRateInfluence() {
            description = title,
            amount = percentage,
        };

        foreach (var t in tiles)
            t.AddInfluence(influence);
    }

    public override void Undo(Tile[] tiles)
    {
        foreach (var t in tiles)
            t.EndInfluence(influence);
    }
}
