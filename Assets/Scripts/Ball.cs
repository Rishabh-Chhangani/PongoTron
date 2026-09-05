using UnityEngine;

public class Ball : MonoBehaviour
{

    public BallAudio ballAudio;
    private Rigidbody2D _rigidbody;
    public float speed = 10.0f;

    [SerializeField] private float maxStartY = 4f;

    [SerializeField] private float speedIncreaseMultiplier = 1.1f;
    [SerializeField] private float maxSpeed = 20f;

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
        float randomY = Random.Range(-maxStartY, maxStartY);

        _rigidbody.position = new Vector2(0f, randomY);
        _rigidbody.velocity = Vector2.zero;
    }


    public void AddInitialForce()
    {
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
            IncreaseSpeed();
        }

        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            ballAudio.PlayWallSound();
        }
    }


    public void IncreaseSpeed()
    {
        float currentSpeed = _rigidbody.velocity.magnitude;

        if (currentSpeed <= 0f)
            return;

        float newSpeed = Mathf.Min(
            currentSpeed * speedIncreaseMultiplier,
            maxSpeed
        );

        _rigidbody.velocity = _rigidbody.velocity.normalized * newSpeed;
    }

} 
 