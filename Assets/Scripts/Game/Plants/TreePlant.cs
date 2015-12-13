using UnityEngine;
using System.Collections;

public class TreePlant : Plant {
    protected override void FinishedGrowing() {
        SendMessageUpwards("PlantFinished", this);
        GameManager.Instance.currency += score;
        // Don't destroy
    }
}
