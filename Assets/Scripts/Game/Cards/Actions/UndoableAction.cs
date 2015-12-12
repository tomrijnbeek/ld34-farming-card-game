using UnityEngine;
using System.Collections;

public abstract class UndoableAction : Action {

    public abstract void Undo(Tile[] tiles);
}
