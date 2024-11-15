using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MoveableObject is an object that can be moved
/// using input from <see cref="MovementInput"/>
/// </summary>
public class MoveableObject : MonoBehaviour
{
    [SerializeField] protected float speed = 100f;
    [SerializeField] protected Rigidbody2D rb;

    /// <summary>
    /// Move the object's rigidbody in the direction of the input.
    /// </summary>
    /// <param name="input">A <see cref="Vector2"/> representing the user's input.</param>
    public virtual void Move(Vector2 input)
    {
        Vector3 direction = transform.right * input.x + transform.up * input.y;

        if (direction.magnitude >= 0.01)
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        }
    }
}
