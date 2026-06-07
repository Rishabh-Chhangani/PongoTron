using UnityEngine;
using TMPro;
public class ScoreText : MonoBehaviour
{
	public TextMeshProUGUI text;
	
		public void SetScore(int value)
		{
			text.text = value.ToString();
		}
}