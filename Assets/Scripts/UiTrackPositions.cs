using TMPro;
using UnityEngine;

public class UiTrackPositions : MonoBehaviour
{
    private TMP_Text _tmp;
    private string _text;

    private void Awake() => _tmp = GetComponent<TMP_Text>();

    private void Update()
    {
        _text = string.Empty;
        var racers = CupManager.Instance.Racers;
        foreach (var racer in racers)
            _text += racer.Car.name + " " + racer.RacerPosition.LastPointIndex + " " +
                     racer.RacerPosition.DistanceToNextPoint + "\n";
        _tmp.SetText(_text);
    }
}