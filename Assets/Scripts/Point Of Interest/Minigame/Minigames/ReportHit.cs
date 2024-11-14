using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ReportHit is a subscriber pattern where 
/// a trigger can report a hit to another object.
/// </summary>
public interface ReportHit
{
    public void ReportObstacleHit();
}
