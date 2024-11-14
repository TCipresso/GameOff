using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{
    [SerializeField] MoveableObject puppet;
    private static MovementInput instance;
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
        Debug.Log($"{input.x} {input.y}");
        puppet.Move(input);
    }

    public void SetPuppet(MoveableObject puppet)
    {
        this.puppet = puppet;
    }
}
