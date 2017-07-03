using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCenter : MonoBehaviour {
    

    void Start() {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.centerOfMass = new Vector3(0, -0.8f);
    }

    void OnMouseDown() {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(new Vector2(450, 450));
    }
    
}