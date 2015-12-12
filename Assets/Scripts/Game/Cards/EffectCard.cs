using UnityEngine;
using System.Collections;

public class EffectCard : ActionCard {
    protected override string description {
        get {
            return string.Format("Duration: {0} turns\n{1}", effect.duration, base.description);
        }
    }

    public Effect effect;

    protected override Action GetAction()
    {
        return effect.action;
    }

    protected override void DoTheThing(Tile[] tiles)
    {
        EffectManager.Instance.AddEffect(effect, tiles);
    }
}
