using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGUI : MonoBehaviour {

    public Camera camera;
    public ToysEmitter toysEmitter;

    void OnGUI () {
        if (GUI.Button(new Rect(15, 15, 150, 50), "Add roly-poly toy")) {
            toysEmitter.AddToy(camera.transform.position.x);
        }
        if (GUI.Button(new Rect(15, 80, 150, 50), "Remove roly-poly toy")) {
            toysEmitter.RemoveToy();
        }
    }
}
