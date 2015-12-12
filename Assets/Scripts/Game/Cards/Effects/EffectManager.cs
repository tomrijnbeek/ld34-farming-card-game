using UnityEngine;
using System.Collections.Generic;

public class EffectManager : Singleton<EffectManager> {

    public List<Effect> activeEffects = new List<Effect>();
    Dictionary<Effect, Tile[]> affectedTiles = new Dictionary<Effect, Tile[]>();

    public void AddEffect(Effect effect, Tile[] tiles) {
        effect.action.Do(tiles);
        activeEffects.Add(effect);
        affectedTiles.Add(effect, tiles);
    }

    public void GrowthStep() {
        foreach (var e in activeEffects) {
            e.duration--;
            if (e.duration <= 0) {
                e.action.Undo(affectedTiles[e]);
                affectedTiles.Remove(e);
            }
        }

        activeEffects.RemoveAll(e => e.duration <= 0);
    }
}
