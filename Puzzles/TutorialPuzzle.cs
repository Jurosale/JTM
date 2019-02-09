using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      After burning the ice and restoring
 *      the root, player can restore bridge
*********************************************** */

public class TutorialPuzzle : MonoBehaviour {

    public GameObject bridge, brokenbridge;
    //public GameObject ice;
    public GameObject root;
    public GameObject speechRec;

    private float dimX, dimY, dimZ;
    private Vector4 colorObject;
    bool checkRoot;

    // Use this for initialization
    void Start () {

        /* dimX = ice.transform.localScale.x;
         dimY = ice.transform.localScale.y;
         dimZ = ice.transform.localScale.z;

         colorObject = new Vector4(ice.GetComponent<SpriteRenderer>().color.r, ice.GetComponent<SpriteRenderer>().color.g,
             ice.GetComponent<SpriteRenderer>().color.b, ice.GetComponent<SpriteRenderer>().color.a);
             */
        

        //Added by inan
        root.gameObject.GetComponent<Root>().canRestore = true;

        checkRoot = true;
    }
	
	// Update is called once per frame
	void Update () {

        //If the ice is burned away, cannot restore the ice back (when trying to restore root)
        //   Root can now be restored
       /* if (ice.transform.localScale.y == 0)
        {
            ice.SetActive(false);
            root.gameObject.GetComponent<Root>().canRestore = true;
        }
        */
        //If root is restored
        if(root.gameObject.GetComponent<Root>().canRestore == true)
        {
            //can restore bridge
            bridge.SetActive(true);
        }
        //If the bridge is restored, get rid of the debris
        if (bridge.GetComponent<FixBridge>().isBridgeRestored == true)
        {
            brokenbridge.SetActive(false);
        }

        //If cast ice, block of ice comes back
       /* if (ice.transform.localScale.y == 0 && speechRec.gameObject.GetComponent<SpeechRecognition01>().word == "ice")
        {
            Debug.Log("Bringing back ice");
            ice.SetActive(true);
            ice.GetComponent<SpriteRenderer>().color = colorObject;
            ice.transform.localScale = new Vector3(dimX, dimY, dimZ);
        }
        */
    }
}
