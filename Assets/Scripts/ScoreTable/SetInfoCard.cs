using UnityEngine;
using TMPro;

public class SetInfoCard : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _textPosition;
	[SerializeField] private TextMeshProUGUI _textName;
	[SerializeField] private TextMeshProUGUI _textScore;
	public void SetInfo(string textPosition, string textName, string textScore) { 
		_textPosition.text = textPosition;
		_textName.text = textName;
		_textScore.text = textScore;
	}
}
