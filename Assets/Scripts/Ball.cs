
using UnityEngine;
public class Ball : MonoBehaviour
{
	
	public Rigidbody2D rb2d;
	public float maxInitialAngle = 0.67F;
	public float moveSpeed = 4F;
	public float maxStartY = 4F;
	public float speedMulitplier = 1.1F;
	public BallAudio ballAudio;
	
    private float startX = 0F;
    private void Start()
    {
		GameManager.instance.onReset += ResetBall;
        GameManager.instance.gameUI.onStartGame += ResetBall;
    }

	private void ResetBall()
	{
		ResetBallPosition();
		InitialPush();
	}
    public void InitialPush()
    {
	    Vector2 dir = Random.value < 0.5F ? Vector2.left : Vector2.right;
	    // Vector2 dir = Vector2.left;
	    
	    dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
	    rb2d.velocity = dir * moveSpeed;
    }
    private void ResetBallPosition()
    {
	    float posY = Random.Range(-maxStartY, maxStartY);
	    Vector2 pos = new Vector2(startX, posY);
	    transform.position = pos;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Debug.Log("Ball hit trigger");
		ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
		if (scoreZone != null)
		{
			GameManager.instance.OnScoreZoneReached(scoreZone.id);
			// Debug.Log("Ball hit score zone");

		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{

		// Debug.Log(
		// 	"Paddle hit:"+ collision.gameObject.name +
		// 	"Before HitVelocity :" + rb2d.velocity 
		// 	);
		Paddle paddle = collision.collider.GetComponent<Paddle>();
		if(paddle)
		{
			ballAudio.PlayPaddleSound();
			rb2d.velocity *= speedMulitplier;
		}

		Wall wall = collision.collider.GetComponent<Wall>();
		if(wall)
		{
			ballAudio.PlayWallSound();
			
		}


		
		// Debug.Log(
		// 	"Paddle hit:"+ collision.gameObject.name +
		// 	"Before HitVelocity :" + rb2d.velocity 
		// 	);
	}
}
