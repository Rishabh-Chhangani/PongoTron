using UnityEngine;

public class Ball : MonoBehaviour
{

    public BallAudio ballAudio;
    private Rigidbody2D _rigidbody;
    public float speed = 10.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
       
        AddInitialForce();
    }
    public void ResetPosition()
    {
        _rigidbody.position = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    // Update is called once per frame
    public void AddInitialForce()
    {
        // float x = Random.value < 0.5f ? -1.0f : 1.0f;
        // float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        // Vector2 direction = new Vector2(x, y);
        // _rigidbody.AddForce(direction * speed);
        // Debug.Log("Start the Game!");

         float x = Random.value < 0.5f ? -1f : 1f;
    float y = Random.value < 0.5f
        ? Random.Range(-1f, -0.5f)
        : Random.Range(0.5f, 1f);

    Vector2 direction = new Vector2(x, y).normalized;

    _rigidbody.velocity = direction * speed;
    }

    public void AddForce(Vector2 force)
    {
        _rigidbody.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.gameObject.GetComponent<Paddle>();
        if (paddle != null)
        {
            ballAudio.PlayPaddleSound();
        }

        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            ballAudio.PlayWallSound();
        }

    }

} 
 