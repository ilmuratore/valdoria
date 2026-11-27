using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaObiettivoMissione : MonoBehaviour
{
    [SerializeField] private MissioneEsplorazione missione;
    public MissioneEsplorazione Missione => missione;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (missione == null) return;
        BersaglioGiocatore giocatore = collision.GetComponentInParent<BersaglioGiocatore>();
        if (giocatore != null) missione.RegistraArrivo(giocatore);
    }

}
