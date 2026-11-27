using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MinimappaSegueGiocatore : MonoBehaviour
{
    [SerializeField] private Transform giocatore;
    private float profonditaCamera;

    private void Awake()
    {
        profonditaCamera = transform.position.z;
    }

    private void LateUpdate()
    {
        if (giocatore == null) return;
        transform.position = new Vector3(giocatore.position.x, giocatore.position.y, profonditaCamera);
    }
}
