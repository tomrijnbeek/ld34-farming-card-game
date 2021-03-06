﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileSelector : MonoBehaviour {

    public int w, h;
    public System.Predicate<Tile[]> selectionChecker;

    Tile[,] tiles { get { return GameManager.Instance.tiles; } }

    public bool validSelection;
    public int iFrom, iTo, jFrom, jTo;

	// Use this for initialization
	void Start () {
        validSelection = false;
	}
	
	// Update is called once per frame
	void Update () {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var hasSelection = GetAxis(worldPos.x, w, out iFrom, out iTo) && GetAxis(worldPos.y, h, out jFrom, out jTo);

        if (selectionChecker != null) {
            validSelection = hasSelection ? selectionChecker(selectedTiles().ToArray()) : false;
        }
        else
            validSelection = hasSelection;

        foreach (var t in tiles)
            t.Highlight(Tile.Selection.None);
        if (hasSelection)
            foreach (var t in selectedTiles())
                t.Highlight(validSelection ? Tile.Selection.Valid : Tile.Selection.Invalid);
	}

    public IEnumerable<Tile> selectedTiles() {
//        if (!validSelection)
//            throw new UnityException("There is no valid selection to enumerate over.");

        for (int j = jFrom; j <= jTo; j++)
            for (int i = iFrom; i <= iTo; i++)
                if (i >= 0 && j >= 0 && i < tiles.GetLength(0) && j < tiles.GetLength(1))
                    yield return tiles[i, j];
    }

    bool GetAxis (float pos, int size, out int start, out int end) {
        if (Mathf.Abs(pos) > 2.5)
        {
            start = end = -1;
            return false; // outside of grid
        }

        if (size % 2 == 1) { // odd, just get closest round number
            var c = Mathf.RoundToInt(pos);
            start = 2 + c - (size - 1) / 2;
            end = 2 + c + (size - 1) / 2;
        } else { // even, use "half" grid
            var c = Mathf.RoundToInt(pos - .5f);
            start = 2 + c - (size - 1) / 2;
            end = 3 + c + (size - 1) / 2;
        }

        return true;
    }
}
