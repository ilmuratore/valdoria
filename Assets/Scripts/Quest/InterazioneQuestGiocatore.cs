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

    private void Update()
    {
        if (!giocatore.EAttivo || !azioneInterazione.WasPressedThisFrame())
            return;

        InterazioneQuestNPC migliore = null;
        float distanzaMinima = float.PositiveInfinity;

        InterazioneQuestNPC[] npc =
            FindObjectsByType<InterazioneQuestNPC>(FindObjectsSortMode.None);

        foreach (InterazioneQuestNPC candidato in npc)
        {
            if (!candidato.PuoInteragire(giocatore)) continue;

            float distanza = Vector2.Distance(
                transform.position, candidato.transform.position
            );

            if (distanza >= distanzaMinima) continue;
            migliore = candidato;
            distanzaMinima = distanza;
        }

        if (migliore != null)
            migliore.Interagisci();
    }
}
