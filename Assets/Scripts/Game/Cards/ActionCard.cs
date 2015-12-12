using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ActionCard : Card {
    protected override string title { get { return "Title"; } }
    protected override string description { get { return "Description"; } }

    void Start() {
        
    }

    protected override TileSelector CreateTileSelector()
    {
        return null;
    }

    protected override void DoTheThing(Tile[] tiles)
    {
        
    }
}