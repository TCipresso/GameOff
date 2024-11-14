using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves an object linearly using transform.
/// </summary>
public class MoveForward : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool onFixedUpdate = false;
    [SerializeField] bool horizontal = true;
    [SerializeField] bool vertical = true;

    private void Update()
    {
        if (onFixedUpdate) return;
        Move();
    }

    private void FixedUpdate()
    {
        if (!onFixedUpdate) return;
        Move();
    }

    /// <summary>
    /// Moves the game object linearly with its transform
    /// in the direction and speed wanted.
    /// </summary>
    private void Move()
    {
        if(!horizontal && !vertical)
        {
            Debug.LogWarning($"{name} has a move forward script but both horizontal and vertical is false!");
            return;
        }
        Vector2 target = transform.position;
        if(horizontal) target.x += speed;
        if(vertical) target.y += speed;
        transform.position = Vector2.MoveTowards(transform.position, target, (speed > 0 ? speed : -speed) * Time.deltaTime);
    }
}
