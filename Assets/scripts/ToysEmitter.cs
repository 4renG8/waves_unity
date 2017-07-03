using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToysEmitter : MonoBehaviour {
    
    [Range(1, 10)]
    public int numbersOfToysOnStart = 3;
    public float emissionForce = 0f;
    [SerializeField]
    private GameObject dummyPrefab;
    [SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private GameObject toyPrefab;
    [SerializeField]
    private WaveTerrain waveTerrain;
    private List<GameObject> _toys;
    public List<GameObject> toys {
        get { return _toys; }
    }
    public float spawnDelay = 1;
    public float animationCutTime = 3;
    public float levelOffset = 3;

    private float _remove = 0;
    
    void Start () {
        _toys = new List<GameObject>();
        for (int i = 0; i < numbersOfToysOnStart; i++) {
            AddToy((transform.position.x + i * 1.5f));
        }
    }

    public void AddToy (float x) {
        StartCoroutine(AddEffectsAndToy(x));
    }

    public void RemoveToy () {
        if (_toys.Count > 0) {
            Destroy(_toys[0]);
            _toys.RemoveAt(0);
        }
    }

    private IEnumerator AddEffectsAndToy (float x) {
        float y = waveTerrain.GetLevelAtX(x) + levelOffset;
        GameObject spawn;
        spawn = Instantiate(dummyPrefab);
        spawn.transform.position = new Vector3(x, y + spawn.GetComponent<SpriteRenderer>().bounds.size.y / 2, 0);
        spawn.GetComponent<SpawnEffect>().GoAnimation(spawnDelay);
        StartCoroutine(AddSpawnParticlesSystem(x, spawn.transform.position.y));

        yield return new WaitForSeconds(spawnDelay);
        Destroy(spawn);

        GameObject currentToy;
        currentToy = Instantiate(toyPrefab);
        currentToy.transform.position = new Vector3(x, y + currentToy.GetComponent<SpriteRenderer>().bounds.size.y / 2, 0);
        currentToy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -emissionForce));
        _toys.Add(currentToy);
    }

    private IEnumerator AddSpawnParticlesSystem (float x, float y) {
        GameObject particles;
        particles = Instantiate(particlesPrefab);
        particles.transform.position = new Vector3(x, y, 0.3f);
        
        yield return new WaitForSeconds(animationCutTime);
        Destroy(particles);
    }
}
