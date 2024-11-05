using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
