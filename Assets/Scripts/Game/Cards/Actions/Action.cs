using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviourBase {
    public string title;
    public string description;
    //public Sprite sprite;
    public int width, height;

    public abstract void Do(Tile[] tiles);
}
