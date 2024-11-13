using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{
    [SerializeField] MoveableObject puppet;
    Vector2 input = new Vector2();

    private void Update()
    {
        if (puppet == null) return;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        Debug.Log($"{input.x} {input.y}");
        puppet.Move(input);
    }
}
