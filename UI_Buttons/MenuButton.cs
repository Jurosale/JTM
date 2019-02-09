using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;


public class MenuButton : MonoBehaviour {

    public string menuSound = "event:/Menu_Highlight";

    // Use this for initialization
    void Start () {
        Button buttonMenu = GetComponent<Button>();
        buttonMenu.onClick.AddListener(buttonMenuSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void buttonMenuSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(menuSound);
    }
}
