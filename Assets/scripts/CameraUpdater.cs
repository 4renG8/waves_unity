using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdater : MonoBehaviour {

    public ToysWatcher toysWatcher;
    public float smoothFactor = 0.1f;
    public float reserveViewSpace = 5f;
    public float minSize = 3f;
    public float maxSize = 10f;
    public float horizontalIndent = 5f;

    private float _defaultZ;
    private Camera _camera;

    void Start () {
        _camera = GetComponent<Camera>();
        _defaultZ = transform.position.z;
    }

    void Update() {
        Vector2 target;
        float width = toysWatcher.bounds.size.x + reserveViewSpace;
        float height = toysWatcher.bounds.size.y;
        float scale = (width > height) ? width : height;
        if (scale > maxSize) {
            target = new Vector2(toysWatcher.lastToyPosition.x + reserveViewSpace / 2, toysWatcher.lastToyPosition.y);
        } else {
            target = new Vector2(toysWatcher.bounds.center.x + reserveViewSpace / 2, toysWatcher.bounds.center.y);
        }
        if (!toysWatcher.isEmpty) {
            transform.position = new Vector3(transform.position.x + (target.x - transform.position.x) * smoothFactor, transform.position.y + (target.y - transform.position.y) * smoothFactor, _defaultZ);
            _camera.orthographicSize = Mathf.Clamp(scale / 2, minSize, maxSize);
        }
    }
}
