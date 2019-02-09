using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpirit : MonoBehaviour {

    public GameObject spirit;
    bool check;

	// Use this for initialization
	void Start () {
        spirit.SetActive(false);
        check = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (check && transform.localScale.y == 0) {
            spirit.SetActive(true);
            check = false;
        }
	}
}
