using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class Card : MonoBehaviourBase {
    TileSelector selector;

	// Update is called once per frame
	void Update () {
        if (selector) {
            if (Input.GetMouseButtonDown(0) && selector.validSelection) {
                DoTheThing(selector.selectedTiles().ToArray());
                Finished();
            } else if (Input.GetMouseButtonDown(1)) {
                Cancelled();
            }
        }
	}

    public virtual void Activate () {
        if (Hand.Instance.cardActive)
            return;

        Hand.Instance.SetActiveCard(this);

        selector = CreateTileSelector();

        if (!selector) {
            DoTheThing(GameManager.Instance.tiles.Cast<Tile>().ToArray());
            Finished();
        }
    }

    void Finished() {
        GameManager.Instance.ResetHighlightedTiles();
        Hand.Instance.ResetActiveCard();
        Hand.Instance.DiscardCard(this);
    }

    void Cancelled() {
        GameManager.Instance.ResetHighlightedTiles();
        if (selector) {
            Destroy(selector);
            selector = null;
        }
        Hand.Instance.ResetActiveCard();
    }

    public void Disable(bool disable) {
        if (disable)
            GetComponent<CanvasGroup>().alpha = .3f;
        else
            GetComponent<CanvasGroup>().alpha = 1;
    }

    protected abstract TileSelector CreateTileSelector();
    protected abstract void DoTheThing(Tile[] tiles);
}
