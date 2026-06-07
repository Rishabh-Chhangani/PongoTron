
// using UnityEngine;

// public class ComputerPaddle : Paddle
// {
//     public Rigidbody2D ball;

//     [Header("Difficulty Speeds")]
//     public float mediumSpeed = 12f;
//     public float hardSpeed = 14f;


//     [Header("AI Settings")]
//     public float easyDeadZoneMin = 0.3f;
//     public float easyDeadZoneMax = 0.6f;

//     public float mediumDeadZoneMin = 0.15f;
//     public float mediumDeadZoneMax = 0.3f;

//     public float hardDeadZoneMin = 0.05f;
//     public float hardDeadZoneMax = 0.15f;


//     public float easyPredictionMin = 0.0f;
//     public float easyPredictionMax = 0.5f;

//     public float mediumPredictionMin = 0.5f;
//     public float mediumPredictionMax = 1.0f;

//     public float hardPredictionMin = 1.0f;
//     public float hardPredictionMax = 1.4f;


//     public float currentSpeed;
//     public float currentDeadZone;
//     public float currentPrediction;

//     private int mode;
//     private int difficulty;

//     private int frameCount;

//     private void Start()
//     {
//         Time.timeScale = 1f;
//         mode = PlayerPrefs.GetInt("Mode", 0);
//         difficulty = PlayerPrefs.GetInt("Difficulty", 0);

//         if(mode == 0)
//         {
//         if (difficulty == 0)
//         {
//             currentSpeed = speed;
//             currentDeadZone = Random.Range(easyDeadZoneMin, easyDeadZoneMax);
//             currentPrediction = Random.Range(easyPredictionMin, easyPredictionMax);
//             // Debug.Log("Easy mode selected");
//         }
//         else if (difficulty == 1)
//         {
//             currentSpeed = mediumSpeed;
//             currentDeadZone = Random.Range(mediumDeadZoneMin, mediumDeadZoneMax);
//             currentPrediction = Random.Range(mediumPredictionMin, mediumPredictionMax);
//             // Debug.Log("Medium mode selected");
//         }
//         else if (difficulty == 2)
//         {
//             currentSpeed = hardSpeed;
//             currentDeadZone = Random.Range(hardDeadZoneMin, hardDeadZoneMax);
//             currentPrediction = Random.Range(hardPredictionMin, hardPredictionMax);
//             // Debug.Log("Hard mode selected");
//         }


//         Debug.Log("Difficulty Selected: " + difficulty);
//         Debug.Log("Current Speed: " + currentSpeed);
//         Debug.Log("Current Dead Zone: " + currentDeadZone);
//         Debug.Log("Current Prediction: " + currentPrediction);
//         }
//     }

//      public void Update()
//     {

//         // 2-Player Mode Override (Player 2 controls)
//         if (PlayerPrefs.GetInt("Mode", 0) == 1)  // 2-player mode
//         {
//             Vector2 direction = Vector2.zero;

//             // Player 2: Arrow Keys OR I/K
//             if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.I))
//                 direction = Vector2.up;
//             else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K))
//                 direction = Vector2.down;

//             if (direction.sqrMagnitude > 0)
//             {
//                 _rigidbody.velocity = new Vector2(0, direction.y * speed * 0.8f);
//             }
//             else
//             {
//                 _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, 0.1f);
//             }

//             return;  // Skip AI movement completely
//         }
//     }


//     private void FixedUpdate()
//     {
//         frameCount = (frameCount + 1) % 100; // Reset frame count every 100 frames to prevent overflow

//         if (frameCount % (8 - difficulty * 2) == 0)
//         {
//             return;
//         }

//         if (ball == null)
//         {
//             return;
//         }

//         float targetY;
//         if (ball.velocity.x > 0.0f)
//         {
//             targetY = GetPredictedY();
//         }
//         else
//         {
//             targetY = 0f;
//         }

//         targetY += Random.Range(-0.2f, 0.2f) * (3 - difficulty);

//         float diff = targetY - _rigidbody.position.y;


//         if (Mathf.Abs(diff) <= currentDeadZone)
//         {
//             _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, 0.1f);
//         }
//         else if (diff > 0f)
//         {
//             Vector2 targetVel = new Vector2(0f, currentSpeed);
//             _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVel, 0.1f);
//         }
//         else
//         {
//             Vector2 targetVel = new Vector2(0f, -currentSpeed);
//             _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVel, 0.1f);
//         }
//     }

//     public float GetPredictedY()
//     {
//         if (currentPrediction <= 0.0f || ball.velocity.x <= 0.0f)
//         {
//             return ball.position.y;
//         }

//         float distanceX = Mathf.Abs(transform.position.x - ball.position.x);
//         float time = distanceX / Mathf.Abs(ball.velocity.x);
//         float predictedY = ball.position.y + ball.velocity.y * time * currentPrediction;
//         return predictedY;
//     }


   
// }
