using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    Vector2 cachedPosition;

    private void Start()
    {
        cachedPosition = transform.position;
    }

    public void Move(Vector2 input)
    {
        if (input.x < -0.01f || 0.01f < input.x)
            cachedPosition.x += input.x * speed * Time.deltaTime;

        if (input.y < -0.01f || 0.01f < input.y)
            cachedPosition.y += input.y * speed * Time.deltaTime;

        transform.position = cachedPosition;
    }
}
