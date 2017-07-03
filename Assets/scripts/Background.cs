using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public Camera trackingCamera;

    private float ratio;

    void Start () {
        ratio = transform.localScale.y / trackingCamera.orthographicSize;
    }
	
	void Update () {
        transform.position = new Vector3(trackingCamera.transform.position.x, trackingCamera.transform.position.y, transform.position.z);
        transform.localScale = new Vector3(trackingCamera.orthographicSize * ratio, trackingCamera.orthographicSize * ratio, 1);
    }
}
