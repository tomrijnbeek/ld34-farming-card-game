using UnityEngine;
using System.Linq;

public class Deck : Singleton<Deck> {

    public DeckItem[] deck;
    public float[] weights;

    void Start () {
        weights = new float[deck.Length];
        float current = 0;

        for (int i = 0; i < deck.Length; i++) {
            current += deck[i].weight;
            weights[i] = current;
        }
    }

    public GameObject GetRandomPrefab () {
        var r = Random.value * weights[weights.Length - 1];
        var i = System.Array.BinarySearch(weights, r);
        if (i < 0)
            i = ~i;
        return deck[i].cardPrefab;
    }
}

[System.Serializable]
public class DeckItem {
    public GameObject cardPrefab;
    public float weight;
}