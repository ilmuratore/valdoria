using System.Collections.Generic;
using UnityEngine;

public abstract class MissioneEliminazione : Missione
{
    [Header("Nemici dellla missione")]
    [SerializeField] private GameObject prefabNemico;
    [SerializeField] private Transform[] puntiGenerazione;
    private readonly HashSet<ObiettivoNemicoMissione> nemiciAttivi = new HashSet<ObiettivoNemicoMissione>();
    public int Eliminati { get; private set; }
    public int Totale { get; private set; }
    protected abstract string NomeNemici { get; }
    public override string TestoObiettivo => $"{NomeNemici} eliminati : {Eliminati}/{Totale}";
    protected override bool VerificaPreparazione()
    {
        if (prefabNemico == null || !prefabNemico.activeSelf || prefabNemico.GetComponent<Nemico>() == null
            || prefabNemico.GetComponent<StatistichePersonaggio>() == null ||
            puntiGenerazione == null) return false;
        foreach(Transform punto in puntiGenerazione)
        {
            if (punto != null) return true;
        }
        return false;
    }

    protected override void AvviaObiettivi()
    {
        Eliminati = 0;
        Totale = 0;
        nemiciAttivi.Clear();
        foreach(Transform punto in puntiGenerazione)
        {
            if (punto == null) continue;
            GameObject nemico = Instantiate(prefabNemico, punto.position, punto.rotation);
            RilevamentoNPC sensore = nemico.GetComponent<RilevamentoNPC>();
            if (sensore != null) sensore.ImpostaBersaglio(giocatore);
            ObiettivoNemicoMissione obiettivo = nemico.GetComponent<ObiettivoNemicoMissione>();
            if(obiettivo == null)
            {
                obiettivo = nemico.AddComponent<ObiettivoNemicoMissione>();
            }
            obiettivo.Inizializza(this);
            nemiciAttivi.Add(obiettivo);
            Totale++;
        }
    }

    public void RegistraEliminazione(ObiettivoNemicoMissione nemico)
    {
        if (Stato != StatoMissione.InCorso || !nemiciAttivi.Remove(nemico)) return;
        Eliminati++;
        if (Eliminati == Totale)
        {
            ObiettiviCompletati();
        }
        else
        {
            NotificaModifica();
        }   
    }
}
