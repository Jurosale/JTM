//someone comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUI : MonoBehaviour {

    public List<GameObject> stateSigns;
    public List<GameObject> speakSigns;
    public List<GameObject> castSigns;

    public List<GameObject> spells;

    public List<GameObject> speakSignsToShow;
    public List<GameObject> castSignsToShow;

    public bool stateOn;
    public bool speakState;
    public bool castState;

    int signCounter;

	// Use this for initialization
	void Start () {

        signCounter = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public IEnumerator DeploySigns(List<GameObject> signs)
    {

        signCounter = 0;

        while (signCounter < 51)
        {
            foreach(GameObject sign in signs)
            {
                if (sign.name == "Speak" || sign.name == "Cast")
                    sign.GetComponent<RectTransform>().position += new Vector3(0, -4, 0);

                else
                    sign.GetComponent<RectTransform>().position += new Vector3(0, -8, 0);
            }

            signCounter++;
            yield return null;
        }

        foreach (GameObject sign in signs)
        {
            sign.GetComponent<RectTransform>().position += new Vector3(0, 4, 0);
        }

    }

    public IEnumerator WithdrawSigns(List<GameObject> signs)
    {

        signCounter = 0;

        while (signCounter < 50)
        {
            foreach (GameObject sign in signs)
            {
                sign.GetComponent<RectTransform>().position += new Vector3(0, 4, 0);
            }

            signCounter++;
            yield return null;
        }

        //foreach (GameObject sign in signs)
        //{
        //    sign.transform.position = sign.GetComponent<ObjectSign>().originalPosition;
        //}
    }

    public IEnumerator WithdrawAllSigns()
    {

        StartCoroutine(WithdrawSigns(stateSigns));
        StartCoroutine(WithdrawSigns(speakSigns));
        StartCoroutine(WithdrawSigns(castSigns));

        yield return null;
    }

    public IEnumerator RollOutAllSpells()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollOutSpell(spells[i]));
            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator RollOutSpell(GameObject spell)
    {
        print("Rolling");

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -2));
            yield return null;
        }
    }

    public IEnumerator RollInAllSpells()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollInSpell(spells[i]));
            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator RollInSpell(GameObject spell)
    {
        print("Rolling");

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 2));
            yield return null;
        }
    }
}
