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
	}

    public IEnumerator ZoomOut()
    {
        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = 0;


        while (mainCamera.orthographicSize < 10)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize += .1f;
        }
    }

    public IEnumerator ZoomIn()
    {
        print("Zooming in");


        while (mainCamera.orthographicSize > 5)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize -= .1f;
        }


        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = .127f;
    }
}
