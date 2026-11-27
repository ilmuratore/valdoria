using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaObiettivoMissione : MonoBehaviour
{
    [SerializeField] private MissioneEsplorazione missione;
    public MissioneEsplorazione Missione => missione;

    private void OnTriggerEnter2D(Collider2D altro)
    {
        if (missione == null)
            return;

        BersaglioGiocatore giocatore =
            altro.GetComponentInParent<BersaglioGiocatore>();

        if (giocatore != null)
            missione.RegistraArrivo(giocatore);
    }
}
