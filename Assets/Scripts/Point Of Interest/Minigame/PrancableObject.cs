using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void MakeJumpable()
    {
        canJump = true;
    }
}
