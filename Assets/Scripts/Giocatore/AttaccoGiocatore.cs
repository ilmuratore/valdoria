using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AttaccoPersonaggio))]
[RequireComponent(typeof(Animator))]
public class AttaccoGiocatore : MonoBehaviour
{
    private AttaccoPersonaggio attacco;
    private Animator animatore;
    private InputAction azioneAttacco;

    private void Awake()
    {
        attacco = GetComponent<AttaccoPersonaggio>();
        animatore = GetComponent<Animator>();
        azioneAttacco = InputSystem.actions.FindAction("Player/Attack", true);

    }

    private void Update()
    {
        if(!azioneAttacco.WasPressedThisFrame()) { return; }
        Vector2 direzione = new Vector2(animatore.GetFloat("MoveX"), animatore.GetFloat("MoveY"));
        if(direzione.sqrMagnitude < 0.01f) { direzione = Vector2.down; }
        attacco.ProvaAttacco(direzione);
    }

}
