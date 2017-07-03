using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTerrain : MonoBehaviour {

    [Range(0.1f, 50f)]
    public float wavesWidth = 10f;
    [Range(0.1f, 50f)]
    public float wavesHeight = 10f;
    public float colliderIndent = 1f;
    [Range(0.05f, 5.0f)]
    public float edgesGap = 1f;
    public Camera cam;
    public ToysWatcher toyWatcher;
    [SerializeField]
    private float _textureScale = 10f;

    private Vector2[] _terrain;
    private Material _material;
    private float _shift;

    private int _skipEdges;
    public  int skipEdges {
        get { return _skipEdges; }
    }
    private float bottomLineLevel;
    private EdgeCollider2D _edgeCollider;
    private Mesh _mesh;
    public Mesh mesh {
        get { return _mesh; }
    }

    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;

    void Start () {
        _material = GetComponent<Renderer>().material;
        _mesh = GetComponent<MeshFilter>().mesh;
        _edgeCollider = (EdgeCollider2D)GetComponent<Collider2D>();
        bottomLineLevel = 0;
    }

    private void SetTerrainEdges (int edges) {
        // Init arrays
        _vertices = new Vector3[edges * 2];
        _triangles = new int[edges * 6];
        _uvs = new Vector2[edges * 2];

        //  Init the first edge
        _vertices[0] = new Vector3(0, bottomLineLevel, 0);
        _vertices[1] = new Vector3(0, 0, 0);
        _uvs[0] = new Vector2(0, bottomLineLevel / wavesHeight);
        _uvs[1] = new Vector2(0, 0);

        // Generate last edges
        for (int i = 1; i < edges; i++) {
            AddTerrainEdge(i);
        }
    }

    private void AddTerrainEdge (int lineNum) {
        float shift;
        int iv = lineNum * 2;
        int it = lineNum * 6;
        shift = lineNum * edgesGap;
        _vertices[iv] = new Vector3(shift, bottomLineLevel, 0);
        _vertices[iv + 1] = new Vector3(shift, 0, 0);

        _triangles[it] = iv - 2;
        _triangles[it + 1] = iv - 1;
        _triangles[it + 2] = iv;
        _triangles[it + 3] = iv;
        _triangles[it + 4] = iv - 1;
        _triangles[it + 5] = iv + 1;

        float textureScale = _textureScale;
        _uvs[iv] = new Vector2(shift / textureScale, bottomLineLevel / textureScale);
        _uvs[iv+1] = new Vector2(shift / textureScale, 0 );
    }

    private void SetShift (float shift) {
        int line;
        float y;
        float textureScale = _textureScale;
        if (shift < 0) {
            _skipEdges = Mathf.CeilToInt(shift / edgesGap) - 1;
        } else {
            _skipEdges = Mathf.FloorToInt(shift / edgesGap);
        }
        for (int i = 0; i < _vertices.Length; i+=2) {
            line = (int)_skipEdges + (i / 2);
            y = GetLevelAtX(line * edgesGap);
            _vertices[i].x = line * edgesGap;
            _vertices[i + 1].x = line * edgesGap;
            _vertices[i + 1].y = y;
            _uvs[i].y = (bottomLineLevel - y) / textureScale;
        }
        _material.SetTextureOffset("_MainTex", new Vector2(-_skipEdges / (_textureScale) * edgesGap, 0));
    }

    private void UpdateMesh () {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.RecalculateBounds();
    }

    private void UpdateCollider() {
        int pointsNum = Mathf.FloorToInt((toyWatcher.bounds.size.x + colliderIndent) / edgesGap) + 3;
        pointsNum = (pointsNum < 2) ? 2 : pointsNum;
        List<Vector2> points = new List<Vector2>();
        float x = toyWatcher.bounds.min.x - (toyWatcher.bounds.min.x % edgesGap);
        for (int i = 0; i < pointsNum; i ++) {
            points.Add(new Vector2(x, GetLevelAtX(x)));
            x += edgesGap;
        }
        _edgeCollider.points = points.ToArray();
    }

    void Update () {
        int edges = (int)Mathf.Ceil(cam.aspect * cam.orthographicSize * 2 / edgesGap) + 2;
        bottomLineLevel = cam.transform.position.y - cam.orthographicSize;
        float x = cam.transform.position.x;
        float halfWidth = cam.aspect * cam.orthographicSize;
        SetTerrainEdges(edges);
        SetShift(x - halfWidth);
        UpdateCollider();
        UpdateMesh();
    }

    public float GetLevelAtX(float x) {
        return Mathf.PerlinNoise((x) / wavesWidth, 0.5f) * wavesHeight;
    }
    
    public Vector3 GetNormalAtX(float x) {
        float currentX;
        Vector3 a, b;
        currentX = x - edgesGap;
        a = new Vector3(currentX, GetLevelAtX(currentX), 0);
        currentX = x + edgesGap;
        b = new Vector3(currentX, GetLevelAtX(currentX), 0);
        return Vector3.Normalize(new Vector3((a.y - b.y), -(a.x - b.x), 0));
    }
}