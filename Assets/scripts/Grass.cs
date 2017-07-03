using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {
    
    public WaveTerrain waveTerrain;
    public float grassScale = 0.7f;
    public float textureScale = 10f;
    [Range (-Mathf.PI, Mathf.PI)]
    public float animationSpeed = 0.05f;
    [Range(-Mathf.PI, Mathf.PI)]
    public float animationStep = 0.1f;
    public float stretchFactor = 0.1f;

    private Mesh _mesh;
    private float angle;

    void Start () {
        _mesh = GetComponent<MeshFilter>().mesh;
        angle = 0;

    }
	
	void Update () {
        int vertexNum = waveTerrain.mesh.vertices.Length;
        Vector2[] uv = waveTerrain.mesh.uv;
        Vector3[] vr = waveTerrain.mesh.vertices;
        Vector3 normal;
        for (int i = 1; i < vertexNum; i+=2) {
            normal = waveTerrain.GetNormalAtX(waveTerrain.mesh.vertices[i].x) * grassScale;
            vr[i - 1] = waveTerrain.mesh.vertices[i];
            uv[i - 1].x = vr[i - 1].x / textureScale;
            uv[i - 0].x = vr[i].x / textureScale;
            uv[i - 1].y = 0;
            uv[i - 0].y = 0.99f;
            vr[i].x = vr[i].x + normal.x + Mathf.Cos(angle + (i + waveTerrain.skipEdges * 2) * animationStep) * stretchFactor;
            vr[i].y = vr[i].y + normal.y + Mathf.Sin(angle + (i + waveTerrain.skipEdges * 2) * animationStep) * stretchFactor;
        }
        _mesh.Clear();
        _mesh.vertices = vr;
        _mesh.triangles = waveTerrain.mesh.triangles;
        _mesh.uv = uv;

        _mesh.RecalculateBounds();
        angle += animationSpeed;
    }
}
