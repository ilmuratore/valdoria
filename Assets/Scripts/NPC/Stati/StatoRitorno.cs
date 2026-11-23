using UnityEngine;

public class StatoRitorno : IStatoNPC
{
    private MovimentoNPC movimento;
    private StatoPattuglia pattuglia;
    private Vector2 posizioneIniziale;
    private Vector2 destinazione;
    private float velocita;
    private float raggioOstacoli;
    private float tempoRiprova;
    private LayerMask ostacoli;
    private bool haDestinazione;
    private ControlloBloccoNPC controllo;
    public bool Terminato { get; private set; }
    public StatoRitorno(MovimentoNPC movimento, StatoPattuglia pattuglia, Vector2 posizioneIniziale, float velocita, float raggioOstacoli, LayerMask ostacoli)
    {
        this.movimento = movimento;
        this.pattuglia = pattuglia;
        this.posizioneIniziale = posizioneIniziale;
        this.velocita = velocita;
        this.raggioOstacoli = raggioOstacoli;
        this.ostacoli = ostacoli;
        controllo = new ControlloBloccoNPC();
    }

    public void Entra()
    {
        Terminato = false;
        haDestinazione = false;
        tempoRiprova = 0f;
        movimento.Ferma();
    }
    public void Aggiorna()
    {
        if (Terminato) { return; }
        if(tempoRiprova > 0f)
        {
            tempoRiprova -= Time.deltaTime;
            movimento.Ferma();
            return;
        }
        if (!haDestinazione)
        {
            haDestinazione = ScegliDestinazione();
            if (!haDestinazione)
            {
                movimento.Ferma();
                tempoRiprova = 1f;
                return;
            }
        }
        if (movimento.Arrivato(destinazione))
        {
            Terminato = true;
            movimento.Ferma();
            return;
        }
        if(!movimento.PercorsoLibero(
            destinazione, raggioOstacoli, ostacoli))
        {
            Riprova();
            return;
        }
        if (controllo.Aggiorna(movimento.Posizione))
        {
            Riprova();
            return;
        }
        movimento.MuoviVerso(destinazione, velocita);
    }
    private void Riprova()
    {
        haDestinazione = false;
        tempoRiprova = 1f;
        movimento.Ferma();
    }
    private bool ScegliDestinazione()
    {
        bool trovato;

        if(pattuglia != null)
        {
            trovato = pattuglia.SelezionaPuntoRaggiungibile(raggioOstacoli, ostacoli, out destinazione);
        }
        else
        {
            destinazione = posizioneIniziale;
            trovato = movimento.PercorsoLibero(destinazione, raggioOstacoli, ostacoli);
        }
        if (trovato)
        {
            controllo.Reimposta(movimento.Posizione);
        }
        return trovato;
    }

    public void Esci()
    {
        movimento.Ferma();
    }
}
