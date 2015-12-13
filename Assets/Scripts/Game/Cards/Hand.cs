using UnityEngine;
using System.Collections.Generic;

public class Hand : Singleton<Hand> {
    
    public List<Card> cards;

    public bool cardActive;

	// Use this for initialization
	void Start () {
        cards = new List<Card>();
        cardActive = false;

        // Two random cards, at least three usable cards.
        InitializeCard(Deck.Instance.GetRandomCard());
        InitializeCard(Deck.Instance.GetRandomCard());
        ReplenishHand (true);

        RealignCards ();
	}
	
	// Update is called once per frame
	void Update () {
        if (!cardActive) {
            foreach (var c in cards) {
                c.Disable(!c.usable);
            }
        }
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
            c.Disable(!c.usable);
        }
    }

    public void DiscardCard (Card c) {
        cards.RemoveAll(d => c == d);
        DestroyCard(c);

        ReplenishHand ();
        RealignCards ();
    }

    public void DiscardAll () {
        foreach (var c in cards) {
            DestroyCard(c);
        }

        cards.Clear();

        ReplenishHand ();
        RealignCards ();
    }

    void DestroyCard (Card c) {
        Destroy(c.gameObject);
    }

    public void RealignCards () {
        if (cards.Count == 0)
            return;

        for (int i = 0; i < cards.Count; i++) {
            cards[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector3(200 * (i - .5f * (cards.Count - 1)), 0, 0);
        }
    }

    public void ReplenishHand (bool onlyFreeCards = false) {
        while (cards.Count < 5)
            InitializeCard(Deck.Instance.GetRandomCard(onlyFreeCards));
    }

    void InitializeCard (Card card) {
        card.transform.SetParent(transform);
        cards.Add(card);
        card.transform.localScale = Vector3.one;
    }
}
