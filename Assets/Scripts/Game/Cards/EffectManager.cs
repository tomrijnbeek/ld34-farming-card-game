using UnityEngine;
using System.Collections.Generic;

public class EffectManager : Singleton<EffectManager> {

    public List<Effect> activeEffects;
    public UnityEngine.UI.Text effectsText;

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

    void Start() {
        activeEffects = new List<Effect>();
    }

    void Update () {
        if (activeEffects.Count == 0) {
            effectsText.text = "";
        } else {
            effectsText.text = "Active effects:\n";
            foreach (var e in activeEffects) {
                effectsText.text += e.name + " (" + e.turnsLeft + " turns)  \n";
            }
        }
    }
}
