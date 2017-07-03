using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour {
    
    private Material _material;
    private float _duration = 0;
    private float _timeLost = 0;

    void Start () {
        _material = GetComponent<Renderer>().material;
    }

	void Update () {
        _timeLost += Time.deltaTime;
        _material.SetFloat("_Step", _timeLost / _duration);
    }

    public void GoAnimation (float duration) {
        _duration = duration;
    }
}
