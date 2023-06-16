using UnityEngine;
using VehicleComponents;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private VehiclePhysics vehiclePrefab;

    private RaceManager _raceManager;
    private VehiclePhysics _vehicle;
    private GameObject _modelPrefab;
    private IControllerInput _controllerInput;
    private Racer _racer;

    private bool _canGon;

    public void Setup(RaceManager raceManager, Racer racer, GameObject modelPrefab, IControllerInput controllerInput)
    {
        _raceManager = raceManager;
        _raceManager.OnGo += RaceManagerOnGo;

        _racer = racer;
        _modelPrefab = modelPrefab;
        _controllerInput = controllerInput;

        _vehicle = Instantiate(vehiclePrefab, transform.position, transform.rotation);
        Instantiate(_modelPrefab, _vehicle.transform);
    }

    private void RaceManagerOnGo() => _canGon = true;

    private void Update()
    {
        if (!_canGon) return;

        _vehicle.Accelerate(_controllerInput.Acceleration - _controllerInput.Break);
        _vehicle.Turn(_controllerInput.Turn);
    }

    private void OnDestroy()
    {
        if (_raceManager != null)
            _raceManager.OnGo -= RaceManagerOnGo;
    }
}