
using UnityEngine;
public class Ball : MonoBehaviour
{
	public GameManager gameManager;
	public Rigidbody2D rb2d;
	public float maxInitialAngle = 0.67F;
	public float moveSpeed = 4F;
	public float maxStartY = 4F;
	public float speedMulitplier = 1.1F;
	
    private float startX = 0F;
    private void Start()
    {
        InitialPush();
    }
    public void InitialPush()
    {
	    Vector2 dir = Random.value < 0.5F ? Vector2.left : Vector2.right;
	    
	    dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
	    rb2d.velocity = dir * moveSpeed;
    }
    private void ResetBall()
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
			gameManager.OnScoreZoneReached(scoreZone.id);
			Debug.Log("Ball hit score zone");
			ResetBall();
			InitialPush();
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Paddle paddle = collision.collider.GetComponent<Paddle>();
		if(paddle)
		{
			rb2d.velocity *= speedMulitplier;
		}
	}
}
