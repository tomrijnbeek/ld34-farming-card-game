using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviourBase {

    public Tile left, right, top, bottom;
    public float growthRate = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoGrowthStep (float factor = 1, bool ignoreRate = false) {
        BroadcastMessage("GrowthStep", (ignoreRate ? 1 : growthRate) * factor, SendMessageOptions.DontRequireReceiver);
    }

    public void Highlight(bool highlight) {
        if (highlight) {
            GetComponent<SpriteRenderer>().color = Color.red;
        } else {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
