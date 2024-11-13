using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    [SerializeField] Rigidbody2D rb;


    public void Move(Vector2 input)
    {
        Vector3 direction = transform.right * input.x + transform.up * input.y;

        if (direction.magnitude >= 0.01)
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        }
    }
}
