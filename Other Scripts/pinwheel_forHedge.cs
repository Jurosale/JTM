using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinwheel_forHedge : MonoBehaviour {

    public GameObject pinwheel;

    bool wheelSpinning = false;
    float timeLeft;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Keep pin wheel spinning forever
        pinwheel.transform.Rotate(0, 0, 60 * Time.deltaTime);

        //Pin wheel spins faster when
        if (GameObject.Find("SpeechRecognition").gameObject.GetComponent<SpeechRecognition01>().word == "wind")
        {
            wheelSpinning = true;
            timeLeft = 5.0f;
        }
        if (wheelSpinning == true)
        {
            timeLeft -= Time.deltaTime;
            pinwheel.transform.Rotate(0, 0, 150 * Time.deltaTime);
            if (timeLeft < 0)
            {
                pinwheel.transform.Rotate(0, 0, 60 * Time.deltaTime);
                wheelSpinning = false;
            }
        }

    }
}
