using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviourBase {

    static bool tutorialRan;

    public int visible;
    public GameObject[] dialogs;

	// Use this for initialization
	void Start () {
        if (tutorialRan) {
            End ();
            return;
        }

        visible = 0;
        dialogs[0].SetActive(true);
        Hand.Instance.cardActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            dialogs[visible].SetActive(false);
            visible++;
            if (visible >= dialogs.Length) {
                End ();
            } else {
                dialogs[visible].SetActive(true);
            }
        } else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) {
            End ();
        }
	}

    void End () {
        Hand.Instance.cardActive = false;
        tutorialRan = true;
        Destroy (gameObject);
    }
}
