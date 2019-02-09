using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour, IPointerEnterHandler {

    //public Button b2;
    public string discoverySound = "event:/SFX_DiscoverSpell_UI";
    public string soundClick2 = "event:/Button_Click";
    public string soundHover2 = "event:/Button_Hover";

    public Button b3;

    // Use this for initialization
    //Help with fmod integration stuff:
    //http://www.fmod.org/getting-started-fmod-unity-basic-c-scripting/ 
    void Start () {

        //Button Two
        Button buttonClick2 = GetComponent<Button>();
        buttonClick2.onClick.AddListener(buttonTwoOnClick);

        Button buttonClick3 = b3.GetComponent<Button>();
        buttonClick3.onClick.AddListener(buttonThreeOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buttonTwoOnClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundClick2);
    }
    public void buttonThreeOnClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(discoverySound);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundHover2);
    }
}
