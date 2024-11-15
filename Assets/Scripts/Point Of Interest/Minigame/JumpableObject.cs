using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JumpableObject is a <see cref="MoveableObject"/> that only jumps.
/// </summary>
public class JumpableObject : MoveableObject
{
    [SerializeField] float jumpForce;
    private bool canJump = true;

    public override void Move(Vector2 input)
    {
        /* I don't know what's wrong with this part!
         * I went back and analyzed my code in a different game and reimplemented it 
         * here, but it's not working the same. Any horizontal input stops gravity, so
         * I'm turning horizontal input off for now.
         */
        /*Vector3 direction = transform.right * input.x;
        if (direction.magnitude >= 0.01)
        {
            rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * speed);
        }*/

        if (input.y < 0.01) canJump = true;
        if (canJump && input.y == 1)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0); //You probably shouldn't directly mess with the velocity but it works.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
