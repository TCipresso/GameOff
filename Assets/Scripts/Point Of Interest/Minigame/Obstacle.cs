using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that the player has to avoid during <see cref="Minigame"/>s.
/// </summary>
public class Obstacle : MonoBehaviour
{
    public ReportHit reporter { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (reporter != null) reporter.ReportObstacleHit();
    }
}
