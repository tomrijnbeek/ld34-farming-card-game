﻿using UnityEngine;
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

    public void DoGrowthStep () {
        BroadcastMessage("GrowthStep", growthRate, SendMessageOptions.DontRequireReceiver);
    }
}
