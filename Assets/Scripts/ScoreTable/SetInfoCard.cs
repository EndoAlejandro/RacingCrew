using TMPro;
using UnityEngine;

namespace ScoreTable
{
    public class SetInfoCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPosition;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textScore;

        public void SetInfo(int position, string racerName, int score)
        {
            textPosition.SetText(position.ToString());
            textName.SetText(racerName);
            textScore.SetText(score.ToString());
        }
    }
}