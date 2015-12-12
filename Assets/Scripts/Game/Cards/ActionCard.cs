using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class ActionCard : Card {
    protected override string title { get { return GetAction().title; } }
    protected override string description { get { return GetAction().description; } }

    protected abstract Action GetAction();

    protected override TileSelector CreateTileSelector()
    {
        var action = GetAction();
        if (action.width == 0 || action.height == 0)
            return null;

        var selector = gameObject.AddComponent<TileSelector>();
        selector.w = action.width;
        selector.h = action.height;

        return selector;
    }
}