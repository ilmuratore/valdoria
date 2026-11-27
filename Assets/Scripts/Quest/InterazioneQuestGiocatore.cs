using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BersaglioGiocatore))]
public class InterazioneQuestGiocatore : MonoBehaviour
{
    private BersaglioGiocatore giocatore;
    private InputAction azioneInterazione;

    private void Awake()
    {
        giocatore = GetComponent<BersaglioGiocatore>();
        azioneInterazione = InputSystem.actions.FindAction("Player/Interact", true);
    }
    void Update()
    {
        if(!giocatore.EAttivo || !azioneInterazione.WasPressedThisFrame())
        {
            return;
        }
        InterazioneQuestNPC vicino = null;
        float distanzaMinima = float.PositiveInfinity;
        InterazioneQuestNPC[] npc = FindObjectsByType<InterazioneQuestNPC>(FindObjectsSortMode.None);
        foreach(InterazioneQuestNPC candidato in npc)
        {
            if (!candidato.PuoInteragire(giocatore)) continue;
            float distanza = Vector2.Distance(transform.position, candidato.transform.position);
            if (distanza >= distanzaMinima) continue;
            vicino = candidato;
            distanzaMinima = distanza;
        }
        if(vicino != null)
        {
            vicino.Interagisci();
        }
    }
}
