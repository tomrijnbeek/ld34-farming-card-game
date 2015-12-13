using UnityEngine;
using System.Linq;

public class Deck : Singleton<Deck> {

    public GameObject cardPrefab;
    public float[] weights;

    void Start () {
        var deck = CardDefinitions.Instance.cards;

        weights = new float[deck.Length];
        float current = 0;

        for (int i = 0; i < deck.Length; i++) {
            current += deck[i].pWeight;
            weights[i] = current;
        } 
    }

    CardDefinition GetRandomDefinition () {
        var r = Random.value * weights[weights.Length - 1];
        var i = System.Array.BinarySearch(weights, r);
        if (i < 0)
            i = ~i;
        return CardDefinitions.Instance.cards[i];
    }

    public Card GetRandomCard () {
        var def = GetRandomDefinition();

        var obj = Instantiate(cardPrefab);
        var card = obj.GetComponent<Card>();
        card.UpdateInfo(def);

        return card;
    }
}