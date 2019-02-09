using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSign : MonoBehaviour {

    public Vector3 originalPosition;

    public Image objectSprite;
    public Text objectName;

	// Use this for initialization
	void Start () {

        originalPosition = transform.position;

        if (gameObject.name != "Speak" || gameObject.name != "Cast")
        {
            foreach (Transform child in transform)
            {

                if (child.name == "Object Sprite")
                {
                    objectSprite = child.gameObject.GetComponent<Image>();
                }

                if (child.name == "Object Name")
                {
                    objectName = child.gameObject.GetComponent<Text>();
                }

            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
