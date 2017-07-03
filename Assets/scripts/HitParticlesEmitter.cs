using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticlesEmitter : MonoBehaviour {

    public GameObject collisionEffect;
    public float zCoordinate;
    public float lifeTime;
    public Vector2 offset;

    void OnCollisionEnter2D(Collision2D collision) {
        foreach (ContactPoint2D contact in collision.contacts) {
            AddSparkles(new Vector3(contact.point.x + offset.x, contact.point.y + offset.y, zCoordinate));
        }
    }

    private void AddSparkles(Vector3 point) {
        GameObject sparkles = (GameObject)Instantiate(collisionEffect);
        sparkles.transform.position = point;
        Destroy(sparkles, lifeTime);

    }
}
