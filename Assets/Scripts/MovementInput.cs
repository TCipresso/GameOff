using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{
    //[SerializeField] MoveableObject puppet;
    Vector2 input = new Vector2();

    private void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        Debug.Log($"{input.x} {input.y}");
        //puppet.Move(input);
    }
}
