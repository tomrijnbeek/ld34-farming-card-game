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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GrowthStep (float growthRate) {
        ApplyGrowth(growthRate);

        if (currentProgress >= maxProgress) {
            FinishedGrowing();
            return;
        }

        int sprite = Mathf.FloorToInt((sprites.Length * currentProgress) / maxProgress);
        if (sprite != currentSprite) {
            GetComponent<SpriteRenderer>().sprite = sprites[sprite];
            currentSprite = sprite;
        }
    }

    void FinishedGrowing () {
        Destroy(gameObject);
    }

    protected virtual void ApplyGrowth(float growthRate) {
        currentProgress += growthRate;
    }
}
