using UnityEngine;

[RequireComponent(typeof(Nemico))]
[RequireComponent(typeof(StatistichePersonaggio))]
public class RicompensaEsperienzaNemico : MonoBehaviour
{
    [SerializeField] private int esperienzaFornita = 40;
    private StatistichePersonaggio statistiche;

    private void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();
    }

    private void OnEnable()
    {
        statistiche.Morto += AssegnaEsperienza; //Quando StatistichePersonaggio emette l'evento Morto, esegui il metodo AssegnaEsperienza
    }

    private void OnDisable()
    {
        statistiche.Morto -= AssegnaEsperienza; //Quando StatistichePersonaggio emette l'evento Morto, disiscrivi l'evento 
    }

    private void AssegnaEsperienza(Personaggio attaccante)
    {
        Giocatore giocatore = attaccante as Giocatore;
        if(giocatore = null) { return; }
        EsperienzaGiocatore esperienza = giocatore.GetComponent<EsperienzaGiocatore>();
        if(esperienza != null)
        {
            esperienza.AggiungiEsperienza(esperienzaFornita);
        }
    }

}
