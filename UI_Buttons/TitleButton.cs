using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour {

    public string titleSound = "event:/Title_Highlight";
    // Use this for initialization
    void Start () {
        Button buttonTitle = GetComponent<Button>();
        buttonTitle.onClick.AddListener(buttonTitleSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void buttonTitleSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(titleSound);
    }
}
