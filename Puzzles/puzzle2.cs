//Ignore this entire script. It's not used in the final version of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle2 : MonoBehaviour
{
    public GameObject avatar;
    public GameObject spikes;
    public GameObject spikesT;
    public GameObject water;
    public GameObject ice;
    public GameObject stone;
    public GameObject stoneChecker;
    public GameObject spikes2;
    public GameObject spikes2T;

    private float oldY;

    // Use this for initialization
    void Start()
    {
        oldY = water.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (avatar.transform.localPosition.x < -33f && avatar.transform.localPosition.x > -110f)
        {
            avatar.GetComponent<AvatarSpells>().canGrow = false;
        }

        if (avatar.transform.localPosition.x < 60f && avatar.transform.localPosition.x > 40f)
        {
            avatar.GetComponent<AvatarSpells>().canGrow = false;
        }

        if (water.transform.localPosition.y >= oldY + water.GetComponent<SummonSpells>().waterHeight && ice.transform.localScale.y > 0)
        {
            spikes.GetComponent<BoxCollider2D>().enabled = false;
            spikesT.GetComponent<BoxCollider2D>().enabled = false;
            ice.GetComponent<UniversalSpells>().enabled = false;
            ice.GetComponent<UniversalSpells>().canFire = false;
            water.GetComponent<UniversalSpells>().enabled = false;
        }
        else
        {
            spikes.GetComponent<BoxCollider2D>().enabled = true;
            spikesT.GetComponent<BoxCollider2D>().enabled = true;
            ice.GetComponent<UniversalSpells>().enabled = true;
            ice.GetComponent<UniversalSpells>().canFire = true;
            water.GetComponent<UniversalSpells>().enabled = true;
        }

        if (Vector2.Distance(spikes2.transform.localPosition, stone.transform.localPosition) <= 36f &&
            Vector2.Distance(spikes2.transform.position, avatar.transform.position) > 4f)
        {
            spikes2.GetComponent<BoxCollider2D>().enabled = false;
            spikes2T.GetComponent<BoxCollider2D>().enabled = false;
            stone.GetComponent<UniversalSpells>().enabled = false;
        }

        if (stone.transform.localPosition.y < 0f)
        {
            stoneChecker.transform.localScale = new Vector3 (0f,0f,0f);
        }
    }
}
