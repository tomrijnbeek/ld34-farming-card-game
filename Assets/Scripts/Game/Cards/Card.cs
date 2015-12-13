using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Card : MonoBehaviourBase {

    public CardDefinition definition;
    TileSelector selector;
    public Text titleText, descriptionText, costText, durationText, gainText;

    public bool usable {
        get {
            if (definition.cost > GameManager.Instance.currency) return false;
            if (definition.type == CardDefinitions.CardTypes.Produce && GameManager.Instance.tiles.Cast<Tile>()
                .All(t => t.plant != null)) return false;
            return true;
        }
    }

    public void UpdateInfo(CardDefinition def) {
        definition = def;

        // Colors
        GetComponent<Image>().color = CardDefinitions.Instance.backColors[(int)def.type];
        foreach (var comp in GetComponentsInChildren<Text>()) {
            comp.color = CardDefinitions.Instance.textColors[(int)def.type];
        }

        // Text
        titleText.text = def.name;
        descriptionText.text = def.description;
        if (def.cost != 0)
            costText.text = FormatCurrency(def.cost);
        if (def.turns != 0)
            durationText.text = FormatDuration(def.turns);
        if (def.gain != 0)
            gainText.text = FormatCurrency(def.gain);
    }

    string FormatCurrency(int cost) {
        return string.Format("${0}", cost);
    }
    string FormatDuration(int turns) {
        return string.Format("{0} turns", turns);
    }

	// Update is called once per frame
	void Update () {
        if (selector) {
            if (Input.GetMouseButtonDown(0) && selector.validSelection) {
                definition.Do(selector.selectedTiles().ToArray());
                Finished();
            } else if (Input.GetMouseButtonDown(1)) {
                Cancelled();
            }
        }
	}

    public virtual void Activate () {
        if (!usable || Hand.Instance.cardActive)
            return;

        Hand.Instance.SetActiveCard(this);

        selector = CreateTileSelector();

        if (!selector) {
            definition.Do(GameManager.Instance.tiles.Cast<Tile>().ToArray());
            Finished();
        }
    }

    void Finished() {
        GameManager.Instance.currency -= definition.cost;
        GameManager.Instance.ResetHighlightedTiles();
        Hand.Instance.ResetActiveCard();
        Hand.Instance.DiscardCard(this);

        GameManager.Instance.GrowthStep();
    }

    void Cancelled() {
        GameManager.Instance.ResetHighlightedTiles();
        if (selector) {
            Destroy(selector);
            selector = null;
        }
        Hand.Instance.ResetActiveCard();

        Shrink();
    }

    TileSelector CreateTileSelector()
    {
        if (definition.areaWidth == 0 || definition.areaHeight == 0)
            return null;

        var selector = gameObject.AddComponent<TileSelector>();
        selector.w = definition.areaWidth;
        selector.h = definition.areaHeight;

        if (definition.AreaCheck != null)
            selector.selectionChecker = definition.AreaCheck;

        return selector;
    }

    public void Grow () {
        if (Hand.Instance.cardActive)
            return;

        transform.localScale = new Vector3(1.6f, 1.6f, 1);
    }

    public void Shrink () {
        if (Hand.Instance.cardActive)
            return;

        transform.localScale = Vector3.one;
    }

    public void Disable(bool disable) {
        if (disable)
            GetComponent<CanvasGroup>().alpha = .3f;
        else
            GetComponent<CanvasGroup>().alpha = 1;
    }
}
