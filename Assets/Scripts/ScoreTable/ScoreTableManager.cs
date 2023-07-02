using CupComponents;
using UnityEngine;
using UnityEngine.UI;

namespace ScoreTable
{
    public class ScoreTableManager : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private SetInfoCard prefabCard;

        [SerializeField] private Button continueButton;

        private void Start()
        {
            var racers = CupManager.Instance.CupRacers;

            for (int i = 0; i < racers.Count; i++)
            {
                var card = Instantiate(prefabCard, parent);
                var racer = racers[i];
                var racerName = racer.IsPlayer ? "Player " : "Racer ";
                card.SetInfo((i + 1), racerName + racer.RacerIndex, racer.Score);
            }

            continueButton.onClick.AddListener(() => CupManager.Instance.LoadNextTrack());
        }
    }
}