using UnityEngine;
using System.Collections;

public class EffectCard : Card {
    protected override string title { get { return "Title"; } }
    protected override string description { get { return "Description"; } }

    protected override TileSelector CreateTileSelector()
    {
        return null;
    }

    protected override void DoTheThing(Tile[] tiles)
    {

    }
}
