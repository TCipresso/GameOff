using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PrancableObject is a <see cref="MoveableObject"/> that just jumps.
/// Unliked <see cref="JumpableObject"/>, this object can only jump once 
/// and can only jump again when something tells it it can.
/// </summary>
public class PrancableObject : MoveableObject
{
    [SerializeField] float jumpForce;
    private bool canJump = true;
    private bool jumpKeyDown = false;

    public override void Move(Vector2 input)
    {
        if (input.y < 0.01) jumpKeyDown = false;
        if (!jumpKeyDown && canJump && input.y == 1)
        {
            canJump = false;
            jumpKeyDown = true;
            rb.velocity = new Vector2(rb.velocity.x, 0); //You probably shouldn't directly mess with the velocity but it works.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Allows the prancable object to jump again.
    /// </summary>
    public void MakeJumpable()
    {
        canJump = true;
    }
}
