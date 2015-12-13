using UnityEngine;
using System.Collections;

public class Effect {
    public readonly string name;
    private readonly EffectDefinition action;
    public readonly Tile[] tiles;
    public int turnsLeft;

    public Effect(string name, EffectDefinition action, int duration, Tile[] tiles) {
        this.name = name;
        this.action = action;
        this.tiles = tiles;

        turnsLeft = duration;
    }

    public void Start() { action.Do(tiles); }
    public void End() { action.Undo(tiles); }
}
