using UnityEngine;
using System.Collections;

public class SingleActionCard : ActionCard {

    public Action action;

    protected override Action GetAction()
    {
        return action;
    }

    protected override void DoTheThing(Tile[] tiles)
    {
        action.Do(tiles);
    }
}
