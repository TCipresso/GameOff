using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Distance limiter limits the distance from an anchor.
/// </summary>
public class DistanceLimiter : MonoBehaviour
{
    [SerializeField] Transform anchor;
    [SerializeField] float distance;
    [SerializeField] bool limitHorizontal;
    [SerializeField] bool limitVertical;
    [SerializeField] bool limitLinear;
    Vector3 startPoint;

    private void OnEnable()
    {
        startPoint = transform.position;
    }

    private void Update()
    {
        //Debug.Log($"{name}: {Mathf.Abs(transform.position.y - startPoint.y)}");
        if (!limitLinear && !limitHorizontal && !limitVertical) Debug.LogError($"{name} No limit setting set!");
        else if (limitLinear && Mathf.Abs((transform.position - startPoint).magnitude) >= distance) gameObject.SetActive(false);
        else if (limitVertical && Mathf.Abs(transform.position.y - startPoint.y) >= distance) gameObject.SetActive(false);
        else if (limitHorizontal && Mathf.Abs(transform.position.x - startPoint.x) >= distance) gameObject.SetActive(false);
    }
}
