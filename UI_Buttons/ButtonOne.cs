using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOne : MonoBehaviour, IPointerEnterHandler
{

    //public Button b1;
    public string soundClick1 = "event:/SFX_ButtonClick_UI";
    public string soundHover1 = "event:/SFX_ButtonHover_UI";
    // Use this for initialization
    //Help with fmod integration stuff:
    //http://www.fmod.org/getting-started-fmod-unity-basic-c-scripting/ 

    void Start () {
        Button buttonClick1 = GetComponent<Button>();
        buttonClick1.onClick.AddListener(buttonOneOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void buttonOneOnClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundClick1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundHover1);
    }
}
