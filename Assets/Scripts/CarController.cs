using CarComponents;
using RaceComponents;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Car carPrefab;

    private TrackManager _trackManager;
    private Car _car;
    private GameObject _modelPrefab;
    private IControllerInput _controllerInput;
    private Racer _racer;

    private bool _canGon;

    public void Setup(TrackManager trackManager, Racer racer, GameObject modelPrefab, IControllerInput controllerInput)
    {
        _trackManager = trackManager;
        _trackManager.OnGo += TrackManagerOnGo;

        _racer = racer;
        _modelPrefab = modelPrefab;
        _controllerInput = controllerInput;

        _car = Instantiate(carPrefab, transform.position, transform.rotation);
        Instantiate(_modelPrefab, _car.transform);
    }

    private void TrackManagerOnGo() => _canGon = true;

    private void Update()
    {
        if (!_canGon) return;

        /*_car.Accelerate(_controllerInput.Acceleration - _controllerInput.Break);
        _car.Turn(_controllerInput.Turn);*/
    }

    private void OnDestroy()
    {
        if (_trackManager != null)
            _trackManager.OnGo -= TrackManagerOnGo;
    }
}