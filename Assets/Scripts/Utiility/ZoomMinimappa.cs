using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ZoomMinimappa : MonoBehaviour
{
    [SerializeField] private float velocitaZoom = 0.2f;
    [SerializeField] private float zoomMinimo = 4f;
    [SerializeField] private float zoomMassimo = 20f;

    private Camera cameraMinimappa;

    private void Awake()
    {
        cameraMinimappa = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        float scroll = Mouse.current.scroll.ReadValue().y;
        if(scroll == 0f) return;
        cameraMinimappa.orthographicSize = Mathf.Clamp(
            cameraMinimappa.orthographicSize - scroll * velocitaZoom,
            zoomMinimo, zoomMassimo);
    }
}
