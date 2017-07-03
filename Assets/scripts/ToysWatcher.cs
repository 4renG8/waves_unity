using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToysWatcher : MonoBehaviour {

    public ToysEmitter toysEmitter;
    public float indent = 0.5f;
    private Vector2 _lastToyPosition;
    public Vector2 lastToyPosition {
        get { return _lastToyPosition; }
    }
    private Bounds _bounds;
    public Bounds bounds {
        get { return _bounds; }
    }
    private bool _isEmpty;
    public bool isEmpty {
        get { return _isEmpty; }
    }

    void Start() {
        _lastToyPosition = new Vector2(0, 0);
        indent *= 2;
    }

    void Update() {
        Vector3 pos;
        float minx = float.MaxValue;
        float miny = float.MaxValue;
        float maxx = float.MinValue;
        float maxy = float.MinValue;
        float width, height;
        float xCenter;
        float yCenter;
        foreach (GameObject toy in toysEmitter.toys) {
            pos = toy.transform.position;
            if (pos.x < minx) {
                minx = pos.x;
                _lastToyPosition = new Vector2(toy.transform.position.x, toy.transform.position.y);
            }
            if (pos.x > maxx) {
                maxx = pos.x;
            }
            if (pos.y < miny) {
                miny = pos.y;
            }
            if (pos.y > maxy) {
                maxy = pos.y;
            }
        }
        width = maxx - minx;
        height = maxy - miny;
        xCenter = minx + width / 2;
        yCenter = miny + height / 2;
        width += indent;
        height += indent;
        if (toysEmitter.toys.Count == 0) {
            _bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(width, height, 0));
            _isEmpty = true;
        } else {
            _bounds = new Bounds(new Vector3(xCenter, yCenter, 0), new Vector3(width, height, 0));
            _isEmpty = false;
        }
    }
}
