//not sure if we use this anymore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComponent : MonoBehaviour {

    public bool conditionMet;
    public bool isWater;
    public bool ice;
    public float waterHeightCoordinate;

	// Use this for initialization
	void Start () {

        conditionMet = false;

	}
	
	// Update is called once per frame
	void Update () {

        if (isWater)
        {
            if (transform.position.y >= waterHeightCoordinate)
            {
                conditionMet = true;
            }
        }
		
	}
}
