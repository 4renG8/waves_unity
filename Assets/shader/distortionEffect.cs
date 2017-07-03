using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distortionEffect : MonoBehaviour {

    public Material material;

    void Start () {
		
	}
	
	// Update is called once per frame
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, material);
    }
}