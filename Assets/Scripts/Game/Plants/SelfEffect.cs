using UnityEngine;

public class SelfEffect : MonoBehaviourBase {

    public EffectDefinition action;

    void PlantPlanted(Plant p) {
        var tile = GetComponentInParent<Tile>();
        if (tile != null)
            action.Do(new [] { tile });
    }

    void PlantFinished(Plant p) {
        var tile = GetComponentInParent<Tile>();
        if (tile != null)
            action.Undo(new [] { tile });
    }
}
