using UnityEngine;
public class Paddle : MonoBehaviour
{
	public Rigidbody2D rb2d;
	public int id;
	public float moveSpeed = 2f;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Update()
	{
		float value = ProcessInput();
		Move(value);
	}
	
	private float ProcessInput()
	{
		float movement = 0F;
		switch(id)
		{
			case 1 :
				movement = Input.GetAxis("MovePlayer1");
				break;
			case 2 : 
				movement = Input.GetAxis("MovePlayer2");
				break;
		}
		return movement;
	}
	
	private void Move(float movement)
	{
		 Debug.Log($"Paddle: {gameObject.name}, rb2d = {rb2d}");
		//rb2d.velocity.y = value *moveSpeed; unity does not allow this but he              said this "There are ways around this, like extension methods, but that's          more advanced and we're not going to discussthis for now  "
		Vector2 velo = rb2d.velocity;
		velo.y = moveSpeed * movement;
		rb2d.velocity = velo;
		
	}
	
}