using System.Collections.Generic;
using RaceComponents;
using TMPro;
using UnityEngine;

public class UiTrackPositions : MonoBehaviour
{
    private TMP_Text _tmp;
    private string _text;
    private List<RacerPosition> _racersPositions;

    private void Awake() => _tmp = GetComponent<TMP_Text>();
    private void Start() => _racersPositions = TrackManager.Instance.RacersPositions;

    private void Update()
    {
        _text = string.Empty;
        foreach (var racerPosition in _racersPositions)
            _text += racerPosition.Car.name + " " + racerPosition.Laps + "\n";
        _tmp.SetText(_text);
    }
}