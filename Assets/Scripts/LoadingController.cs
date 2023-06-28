using UnityEngine;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject loadingCanvas;

    private void Start() => CupManager.Instance.OnLoading += CupManagerOnLoading;

    private void CupManagerOnLoading(bool isLoading)
    {
        camera.gameObject.SetActive(isLoading);
        loadingCanvas.SetActive(isLoading);
    }

    private void OnDestroy()
    {
        if (CupManager.Instance == null) return;
        CupManager.Instance.OnLoading -= CupManagerOnLoading;
    }
}