using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MovementInput is a class that handles input and moves a <see cref="MoveableObject"/>
/// </summary>
public class MovementInput : MonoBehaviour
{
    [SerializeField] MoveableObject puppet;
    public static MovementInput instance { get; private set; }
    Vector2 input = new Vector2();

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    private void FixedUpdate()
    {
        if (puppet == null) return;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        puppet.Move(input);
    }

    public void SetPuppet(MoveableObject puppet)
    {
        this.puppet = puppet;
    }
}
