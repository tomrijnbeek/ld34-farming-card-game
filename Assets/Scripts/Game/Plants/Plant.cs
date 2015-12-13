using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {

    public Sprite[] sprites;
    public string title;

    public float currentProgress;
    public float maxProgress;

    public int score;

    int currentSprite;
    bool ranFinished;

	// Use this for initialization
	void Start () {
        SendMessageUpwards("PlantPlanted", this, SendMessageOptions.DontRequireReceiver);
        GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
	}
	
	// Update is called once per frame
	void Update () {
        // Move into update to make sure surrounding growth tiles bonus can still be applied.
        if (maxProgress > 0 && currentProgress >= maxProgress && !ranFinished) {
            FinishedGrowing();
            ranFinished = true;
        }
	}

    public void GrowthStep (float growthRate) {
        ApplyGrowth(growthRate);

        int sprite = Mathf.FloorToInt((sprites.Length * (currentProgress - 1)) / (maxProgress - 1));
        sprite = Mathf.Clamp(sprite, 0, sprites.Length - 1);

        if (sprite != currentSprite) {
            GetComponent<SpriteRenderer>().sprite = sprites[sprite];
            currentSprite = sprite;
        }
    }

    protected virtual void FinishedGrowing () {
        SendMessageUpwards("PlantFinished", this);
        GameManager.Instance.currency += score;
        DestroyPlant();
    }

    protected virtual void ApplyGrowth(float growthRate) {
        currentProgress += growthRate;
    }

    void DoDestroy() {
        DestroyPlant();
    }

    protected virtual void DestroyPlant () {
        SendMessageUpwards("PlantDestroyed", this);
        Destroy(gameObject);
    }
}
