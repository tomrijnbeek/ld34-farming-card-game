using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {

    public Sprite[] sprites;

    public int currentStage;
    public int numStages;

    int currentSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GrowthStep () {
        currentStage++;

        if (currentStage >= numStages) {
            FinishedGrowing();
            return;
        }

        int sprite = (sprites.Length * currentStage) / numStages;
        if (sprite != currentSprite) {
            GetComponent<SpriteRenderer>().sprite = sprites[sprite];
            currentSprite = sprite;
        }
    }

    void FinishedGrowing () {
        Destroy(gameObject);
    }
}
