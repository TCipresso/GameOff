using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TwoDimensionalTrigger is a way to easily have a 
/// OnTriggerEnter2D event on a trigger.
/// </summary>
public class TwoDimensionalTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter.Invoke();    
    }
}
