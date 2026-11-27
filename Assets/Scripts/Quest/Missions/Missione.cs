using System;
using UnityEngine;

public abstract class Missione : MonoBehaviour
{
    [Header("Storia")]
    [SerializeField] private string titolo = "Nuova Missione";
    [TextArea(3, 6)]
    [SerializeField] private string descrizione = "Descrizione della missione";
    [SerializeField] private int ricompensaXp = 100;

    [Header("Dialoghi")]
    [TextArea(2, 4)]
    [SerializeField] private string dialoghiInCorso;
    [TextArea(2, 4)]
    [SerializeField] private string dialogoDaConsegnare;
    [TextArea(2, 4)]
    [SerializeField] private string dialogoCompletata;
    [Header("Giocatore")]
    [SerializeField] protected BersaglioGiocatore giocatore;
    public StatoMissione Stato { get; private set; } = StatoMissione.NonAccettata;
    public string Titolo => titolo;
    public string Descrizone => descrizione;
    public int RicompensaXp => ricompensaXp;
    public BersaglioGiocatore Giocatore => giocatore;
    public event Action MissioneModificata;
    public abstract string TestoObiettivo { get; }

    public string TestoDialogo
    {
        get
        {
            switch (Stato)
            {
                case StatoMissione.NonAccettata:
                    return descrizione + "\n\nVuoi accettare la missione?";
                case StatoMissione.InCorso:
                    return dialoghiInCorso;
                case StatoMissione.DaConsegnare:
                    return dialogoDaConsegnare;
                case StatoMissione.Completata:
                    return dialogoCompletata;
                default:
                    return string.Empty;
            }
        }
    }

    public bool Accetta()
    {
        if (Stato != StatoMissione.NonAccettata || giocatore == null ||
            !giocatore.EAttivo || !VerificaPreparazione()
            ) { return false; }

        Stato = StatoMissione.InCorso;

        AvviaObiettivi();

        NotificaModifica();
        return true;
    }
    protected abstract bool VerificaPreparazione();
    protected abstract void AvviaObiettivi();


    public void NotificaModifica()
    {
        MissioneModificata?.Invoke();
    }

    public void ObiettiviCompletati()
    {
        if (Stato != StatoMissione.InCorso) return;
        Stato = StatoMissione.DaConsegnare;
        NotificaModifica();
    }

    public bool Consegna()
    {
        if (Stato != StatoMissione.DaConsegnare || giocatore == null || !giocatore.EAttivo) return false;
        EsperienzaGiocatore esperienza = giocatore.GetComponent<EsperienzaGiocatore>();
        if (esperienza == null) return false;
        Stato = StatoMissione.Completata;
        esperienza.AggiungiEsperienza(Mathf.Max(0, ricompensaXp));
        NotificaModifica();
        return true;
    }
}
