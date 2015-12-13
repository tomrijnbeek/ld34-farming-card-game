using UnityEngine;
using System.Collections.Generic;

public class EffectManager : Singleton<EffectManager> {

    public List<Effect> activeEffects = new List<Effect>();

    public void AddEffect(Effect e) {
        e.Start();
        activeEffects.Add(e);
    }

    public void GrowthStep() {
        foreach (var e in activeEffects) {
            e.turnsLeft--;
            if (e.turnsLeft <= 0) {
                e.End();
            }
        }

        activeEffects.RemoveAll(e => e.turnsLeft <= 0);
    }
}
