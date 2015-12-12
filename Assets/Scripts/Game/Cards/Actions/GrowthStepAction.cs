using UnityEngine;
using System.Collections;

public class GrowthStepAction : Action {

    public float growthStepFactor;
    public bool ignoreRate;

	public override void Do(Tile[] tiles)
    {
        foreach (var t in tiles) {
            t.DoGrowthStep(growthStepFactor, ignoreRate);
        }
    }
}
