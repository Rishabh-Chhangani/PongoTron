// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class MainMenu : MonoBehaviour
// {
//     public void SetEasy()
//     {
//         PlayerPrefs.SetInt("Difficulty",0);
//         PlayerPrefs.Save();
//         Debug.Log("Easy mode selected");
//     }

//     public void SetMedium()
//     {
//         PlayerPrefs.SetInt("Difficulty",1);
//         PlayerPrefs.Save();
//         Debug.Log("Medium mode selected");
//     }

//     public void SetHard()
//     {
//         PlayerPrefs.SetInt("Difficulty",2);
//         PlayerPrefs.Save();
//         Debug.Log("Hard mode selected");
//     }
//     public void PlayGame()
//     {
//         PlayerPrefs.SetInt("Mode", 0);
//         PlayerPrefs.Save();
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }
//     public void PlayTwoPlayer()
//     {
//         PlayerPrefs.SetInt("Mode", 1);
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }

//     public void QuitGame()
//     {
//         Application.Quit();
//     }
// }
