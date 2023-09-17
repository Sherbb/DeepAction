using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    //TODO: This is a very quick solution to get some feel early. Need a proper feedback system in the future.

    public static CameraShaker instance;

    public float springForce;
    public float dampening;
    public float max = 25f;

    private Vector2 velocity;

    void Awake()
    {
        instance = this;
    }

    public void Shake(Vector2 impactPosition, float force)
    {
        velocity -= ((Vector2)transform.position - impactPosition).normalized * force;
    }

    void Update()
    {
        transform.localPosition = transform.localPosition + (Vector3)(velocity * Time.deltaTime);
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, -max, max), 
            Mathf.Clamp(transform.localPosition.y, -max, max), 
            Mathf.Clamp(transform.localPosition.z, -max, max)
        );

        velocity *= (1f / dampening);

        velocity -= (Vector2)transform.localPosition * springForce * Time.deltaTime;
    }
}
