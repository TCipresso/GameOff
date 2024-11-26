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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Token"))
        {
            Debug.Log("Arrow hit the player! Mini-game failed.");
            DefendMiniGame.instance.EndMiniGame(false); 
            Destroy(gameObject);
        }
    }
}
