using UnityEngine;
using System.Collections.Generic;

public class Hand : Singleton<Hand> {

    public List<Card> cards = new List<Card>();

    public bool cardActive;

	// Use this for initialization
	void Start () {
        ReplenishHand ();
        RealignCards ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActiveCard (Card c) {
        cardActive = true;

        foreach (var d in cards) {
            if (d != c)
                d.Disable(true);
        }
    }

    public void ResetActiveCard () {
        cardActive = false;

        foreach (var c in cards) {
            c.Disable(false);
        }
    }

    public void DiscardCard (Card c) {
        cards.RemoveAll(d => c == d);
        Destroy(c.gameObject);

        ReplenishHand ();
        RealignCards ();
    }

    public void RealignCards () {
        if (cards.Count == 0)
            return;

        for (int i = 0; i < cards.Count; i++) {
            cards[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector3(200 * (i - .5f * (cards.Count - 1)), 0, 0);
        }
    }

    public void ReplenishHand () {
        while (cards.Count < 5)
            InstantiateCard(Deck.Instance.GetRandomPrefab());
    }

    void InstantiateCard (GameObject prefab) {
        var obj = Instantiate(prefab);
        obj.transform.SetParent(transform);

        var card = obj.GetComponent<Card>();
        cards.Add(card);
        card.Shrink();
    }
}
