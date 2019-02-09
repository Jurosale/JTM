using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePondSpirit : MonoBehaviour {

    public GameObject waterSpell;
    public GameObject nearestShrine;
    public GameObject bridge;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //moves spirt if the conditions to the puzzle is complete
        if (waterSpell.GetComponent<FixArtist>().isPondRestored == true && bridge.GetComponent<FixBridge>().isBridgeRestored == true)
        {
            //turns off spirit interaction script
            GetComponent<Spirit_Interaction>().enabled = false;
            //moves her
            this.transform.position = Vector3.Lerp(this.transform.position,
                 nearestShrine.transform.position + new Vector3(4, 0, 0), Time.deltaTime * 0.2f);
            if (this.transform.position.x > nearestShrine.transform.position.x + 3f)
            {
                GetComponent<MovePondSpirit>().enabled = false;
            }
        }

    }
}
