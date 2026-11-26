using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Alleato))]
public class InterazioneQuestNPC : MonoBehaviour
{
    [Header("Interazione")]
    [SerializeField] private BersaglioGiocatore giocatore;
    [SerializeField] private float distanzaInterazione = 2f;
    [SerializeField] private GameObject indicatoreInterazione;
    [SerializeField] private UIMissioni uiMissioni;

    private InputAction azioneInterazione;

    private void Awake()
    {
        azioneInterazione = InputSystem.actions.FindAction("Player/Interact", true);
    }

    private void Start()
    {
        if (indicatoreInterazione != null) indicatoreInterazione.SetActive(false);
    }

    private void Update() 
    {
        bool vicino = giocatore != null && giocatore.EAttivo && Vector2.Distance(transform.position, giocatore.transform.position) <= distanzaInterazione;
        if (indicatoreInterazione != null) indicatoreInterazione.SetActive(vicino);
        if (vicino && uiMissioni != null && azioneInterazione.WasPressedThisFrame())
        {
            uiMissioni.ApriOChiudiDialogo();
        }
    }

    private void OnDisable()
    {
        if (indicatoreInterazione != null) indicatoreInterazione.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanzaInterazione);
    }

}
