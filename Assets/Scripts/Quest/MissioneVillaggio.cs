using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MissioneVillaggio : MonoBehaviour
{
    [Header("Storia")]
    [SerializeField] private string titolo = "Liberare il villaggio";
    [TextArea(3, 6)]
    [SerializeField] private string descrizione = "I goblin hanno invaso il villaggio. Aiutaci a scacciarli.";
    [SerializeField] private int ricompensaXp = 100;
    [Header("Nemici dellla missione")]
    [SerializeField] private GameObject prefabNemico;
    [SerializeField] private Transform[] puntiGenerazione;
    [Header("Giocatore")]
    [SerializeField] private BersaglioGiocatore giocatore;

    private readonly HashSet<ObiettivoNemicoMissione> nemiciAttivi = new HashSet<ObiettivoNemicoMissione>();
    public StatoMissione Stato { get; private set; } = StatoMissione.NonAccettata;
    public string Titolo { get { return titolo; } }
    public string Descrizione { get { return descrizione; } }
    public int RicompensaXP { get { return ricompensaXp; } }

    public int Eliminati { get; private set; }
    public int Totale { get; private set; }
    public event Action MissioneModificata;

    public bool Accetta()
    {
        if(Stato != StatoMissione.NonAccettata || giocatore == null || !giocatore.EAttivo || prefabNemico == null || puntiGenerazione == null) { return false; }
        if(!prefabNemico.activeSelf || prefabNemico.GetComponent<Nemico>() == null || prefabNemico.GetComponent<StatistichePersonaggio>() == null){
            Debug.LogError("Assegna un Prefab nemico attivo con Nemico e StatistichePersonaggio.", this);
            return false;
        }
        int puntiValidi = 0;
        foreach(Transform punto in puntiGenerazione)
        {
            if (punto != null) puntiValidi++;

        }

        if(puntiValidi == 0) { Debug.LogError("La missione richiede almeno un punto di generazione", this); return false; }
        Stato = StatoMissione.InCorso;
        Eliminati = 0;
        Totale = 0;
        nemiciAttivi.Clear();
        foreach(Transform punto in puntiGenerazione)
        {
            if (punto == null) continue;
            GameObject nemico = Instantiate(prefabNemico, punto.position, punto.rotation);
            RilevamentoNPC sensore = nemico.GetComponent<RilevamentoNPC>();
            if(sensore != null) { sensore.ImpostaBersaglio(giocatore); }
            ObiettivoNemicoMissione obiettivo = nemico.AddComponent<ObiettivoNemicoMissione>();
            obiettivo.Inizializza(this);
            nemiciAttivi.Add(obiettivo);
            Totale++;
        }
        MissioneModificata?.Invoke();
        return true;
    }

    public void RegistraEliminazione(ObiettivoNemicoMissione nemico)
    {
        if (Stato != StatoMissione.InCorso || !nemiciAttivi.Remove(nemico)) return;
        Eliminati++;
        if(Eliminati == Totale) { Stato = StatoMissione.DaConsegnare;  }
        MissioneModificata?.Invoke();
    }

    public bool Consegna()
    {
        if (Stato != StatoMissione.DaConsegnare || giocatore == null || !giocatore.EAttivo) return false;
        EsperienzaGiocatore esperienza = giocatore.GetComponent<EsperienzaGiocatore>();
        if (esperienza == null) return false;
        Stato = StatoMissione.Completata;
        esperienza.AggiungiEsperienza(Mathf.Max(0,ricompensaXp));
        MissioneModificata?.Invoke();
        return true;
    }
}
