using UnityEngine;

[RequireComponent(typeof(StatistichePersonaggio))]
public class ObiettivoNemicoMissione : MonoBehaviour
{

    private StatistichePersonaggio statistiche;
    private MissioneEliminazione missione;
    private bool segnalato; 
    
    private void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();

    }

    private void OnEnable()
    {
        statistiche.Morto += QuandoMuore;
    }

    private void OnDisable()
    {
        statistiche.Morto -= QuandoMuore;    
    }

    public void Inizializza(MissioneEliminazione nuovaMissione) { missione = nuovaMissione; }

    private void QuandoMuore(Personaggio attaccante)
    {
        if (segnalato || missione == null) return;
        segnalato = true;
        missione.RegistraEliminazione(this);
    }
}
