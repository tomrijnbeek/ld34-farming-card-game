using UnityEngine;
using System.Collections;

public class DiscardHandAction : Action {
    public override void Do(Tile[] tiles)
    {
        Hand.Instance.DiscardAll();
    }
}
