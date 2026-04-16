using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 100f;
    public float maxSpeed = 250f;
    public float maxSpinSpeed = 10f;
    
    Rigidbody2D rb;
    public GameObject explosionEffect2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        float randomSpeed = Random.Range(minSpeed, maxSpeed)/randomSize;
        rb = GetComponent<Rigidbody2D>();
        Vector2 randD = Random.insideUnitCircle;
        rb.AddForce(randD * randomSpeed);
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(explosionEffect2, contactPoint, Quaternion.identity);

        // Destroy the effect after 1 second
        Destroy(bounceEffect, 1f);
    }
}
