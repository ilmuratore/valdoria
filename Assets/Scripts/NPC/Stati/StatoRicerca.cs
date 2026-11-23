using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatoRicerca : IStatoNPC
{
    private MovimentoNPC movimento;
    private Vector2 centroRicerca;
    private Vector2 destinazione;
    private float durataRicerca;
    private float raggioRicerca;
    private float raggioOstacoli;
    private float pausaMinima;
    private float pausaMassima;
    private LayerMask ostacoli;
    private ControlloBloccoNPC controllo;
    private float tempoTrascorso;
    private float tempoPausa;
    private bool haDestinazione;
    public bool Terminato { get; private set; }
    public Vector2 Centro
    {
        get { return centroRicerca; }
    }
    public float TempoRestante
    {
        get { return Mathf.Max(0f, durataRicerca - tempoTrascorso); }
    }
    public StatoRicerca(MovimentoNPC movimento, float durataRicerca, float raggioRicerca, float raggioOstacoli , LayerMask ostacoli, float pausaMinima, float pausaMassima)
    {
        this.movimento = movimento;
        this.durataRicerca = durataRicerca;
        this.raggioRicerca = raggioRicerca;
        this.raggioOstacoli = raggioOstacoli;
        this.ostacoli = ostacoli;
        this.pausaMinima = pausaMinima;
        this.pausaMassima = pausaMassima;
        controllo = new ControlloBloccoNPC();
    }
    public void ImpostaCentro(Vector2 centro)
    {
        centroRicerca = centro;
    }

    public void Entra()
    {
        Terminato = false;
        haDestinazione = false;
        tempoTrascorso = 0f;
        tempoPausa = 0f;
        movimento.Ferma();
    }
    public void Aggiorna()
    {
        if (Terminato) { return; }
        tempoTrascorso += Time.deltaTime;
        if (tempoTrascorso >= durataRicerca)
        {
            Terminato = true;
            movimento.Ferma();
            return;
        }
        if (tempoPausa > 0f)
        {
            tempoPausa -= Time.deltaTime;
            movimento.Ferma();
            return;
        }
        if (!haDestinazione)
        {
            haDestinazione = ScegliDestinazione();
            if (!haDestinazione)
            {
                tempoPausa = 1f;
                movimento.Ferma();
                return;
            }
        }
        if (movimento.Arrivato(destinazione))
        {
            ProssimaDestinazione();
            return;
        }
        if(!movimento.PercorsoLibero(destinazione, raggioOstacoli, ostacoli))
        {
            ProssimaDestinazione();
            return;
        }
        if (controllo.Aggiorna(movimento.Posizione))
        {
            ProssimaDestinazione();
            return;
        }
        movimento.MuoviVerso(destinazione);
    }

    private bool ScegliDestinazione()
    {
        for (int tentativo = 0; tentativo < 10; tentativo++)
        {
            Vector2 punto = centroRicerca + Random.insideUnitCircle * raggioRicerca;
            
            if (Vector2.Distance(movimento.Posizione, punto) < 0.5f)
            {
                continue;
            }
            if(!movimento.PercorsoLibero(punto, raggioOstacoli, ostacoli)) { continue; }
            destinazione = punto;
            controllo.Reimposta(movimento.Posizione);
            return true;
        }
        return false;
    }

    private void ProssimaDestinazione()
    {
        haDestinazione = false;
        movimento.Ferma();
        tempoPausa = Random.Range(pausaMinima, pausaMassima);
    }

    public void Esci()
    {
        movimento.Ferma();
        haDestinazione = false;
    }

}
