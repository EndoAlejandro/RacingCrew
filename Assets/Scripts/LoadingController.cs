using CupComponents;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;
    private void Start() => CupManager.Instance.OnLoading += CupManagerOnLoading;
    private void CupManagerOnLoading(bool isLoading) => loadingCanvas.SetActive(isLoading);
    private void OnDestroy()
    {
        if (CupManager.Instance != null) CupManager.Instance.OnLoading -= CupManagerOnLoading;
    }
}