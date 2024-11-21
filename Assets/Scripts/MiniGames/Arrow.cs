using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        speed = Random.Range(400f, 400f);
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
