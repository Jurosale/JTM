using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    Camera mainCamera;

    public GameObject reflectionObj;
    WaterFX reflection;

    Vector3 reflectionScale;
    public bool levelThree;

    // Use this for initialization
    void Start () {

        mainCamera = gameObject.GetComponent<Camera>();
        //reflection = reflectionObj.GetComponent<WaterFX>();

        //reflectionScale = reflection.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator ZoomOut()
    {
        print("Zooming out");

        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = 0;


        while (mainCamera.orthographicSize < 10)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize += .1f;

            //if (levelThree)
            //    reflection.transform.localScale += new Vector3(.03f, .03f, 0);
        }

        //if (levelThree)
        //{
        //    reflection.enabled = true;
        //    reflection.transform.localScale = reflectionScale;
        //}
    }

    public IEnumerator ZoomIn()
    {
        print("Zooming in");


        while (mainCamera.orthographicSize > 5)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize -= .1f;

            //if (levelThree)
            //    reflection.transform.localScale -= new Vector3(.03f, .03f, 0);
        }


        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = .127f;
        //if (levelThree)
        //{
        //    reflection.enabled = true;
        //    reflection.transform.localScale = reflectionScale;
        //}

    }
}
