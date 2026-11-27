using UnityEngine;

[RequireComponent(typeof(Alleato))]
public class InterazioneQuestNPC : MonoBehaviour
{
    [Header("Interazione")]
    [SerializeField] private BersaglioGiocatore giocatore;
    [SerializeField] private float distanzaInterazione = 2f;
    [SerializeField] private GameObject indicatoreInterazione;
    [SerializeField] private UIMissioni uiMissioni;

    [Header("Missione offerta")]
    [SerializeField] private Missione missione;

    private void Start()
    {
        if (indicatoreInterazione != null)
            indicatoreInterazione.SetActive(false);
    }

    public bool PuoInteragire(BersaglioGiocatore personaggio)
    {
        return personaggio != null && personaggio == giocatore &&
            personaggio.EAttivo && missione != null && uiMissioni != null &&
            Vector2.Distance(transform.position, personaggio.transform.position)
                <= distanzaInterazione;
    }

    private void Update()
    {
        if (indicatoreInterazione != null)
            indicatoreInterazione.SetActive(PuoInteragire(giocatore));
    }

    public void Interagisci()
    {
        if (PuoInteragire(giocatore))
            uiMissioni.ApriDialogo(missione);
    }

    private void OnDisable()
    {
        if (indicatoreInterazione != null)
            indicatoreInterazione.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanzaInterazione);
    }
}
