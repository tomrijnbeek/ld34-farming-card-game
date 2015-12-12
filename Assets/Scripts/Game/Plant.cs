using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {

    public Sprite[] sprites;
    public string title;
    public string specialEffect;

    public float currentProgress;
    public float maxProgress;

    int currentSprite;

	// Use this for initialization
	void Start () {
        SendMessageUpwards("PlantPlanted", this, SendMessageOptions.DontRequireReceiver);
	}
	
	// Update is called once per frame
	void Update () {
        // Move into update to make sure surrounding growth tiles bonus can still be applied.
        if (currentProgress >= maxProgress) {
            FinishedGrowing();
        }
	}

    void GrowthStep (float growthRate) {
        ApplyGrowth(growthRate);

        if (currentProgress >= maxProgress) {
            return;
        }

        int sprite = Mathf.FloorToInt((sprites.Length * (currentProgress - 1)) / (maxProgress - 1));
        if (sprite != currentSprite) {
            GetComponent<SpriteRenderer>().sprite = sprites[sprite];
            currentSprite = sprite;
        }
    }

    void FinishedGrowing () {
        SendMessageUpwards("PlantFinished", this);
        Destroy(gameObject);
    }

    protected virtual void ApplyGrowth(float growthRate) {
        currentProgress += growthRate;
    }
}
