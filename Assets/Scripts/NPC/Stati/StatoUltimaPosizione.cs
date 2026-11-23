using UnityEngine;

public class StatoUltimaPosizione : IStatoNPC
{
    private MovimentoNPC movimento;
    private Vector2 destinazione;
    private float tempoMassimo;
    private float tempoTrascorso;
    private float raggioOstacoli;
    private LayerMask ostacoli;
    private ControlloBloccoNPC controllo;

    public bool Terminato { get; private set; }
    public bool Raggiunto { get; private set; }

    public StatoUltimaPosizione(MovimentoNPC movimento, float tempoMassimo, float raggioOstacoli, LayerMask ostacoli)
    {
        this.movimento = movimento;
        this.tempoMassimo = tempoMassimo;
        this.raggioOstacoli = raggioOstacoli;
        this.ostacoli = ostacoli;

        controllo = new ControlloBloccoNPC();
    }
    public void ImpostaDestinazione(Vector2 punto)
    {
        destinazione = punto;
    }

    public void Entra()
    {
        Terminato = false;
        Raggiunto = false;
        tempoTrascorso = 0f;
        controllo.Reimposta(movimento.Posizione);
        movimento.Ferma();

    }
    public void Aggiorna()
    {
        if (Terminato) { return;  }
        if (movimento.Arrivato(destinazione))
        {
            Raggiunto = true;
            Termina();
            return;

        }
        tempoTrascorso += Time.deltaTime;
        if(tempoTrascorso >= tempoMassimo)
        {
            Termina();
            return;
        }
        if(!movimento.PercorsoLibero(
            destinazione, raggioOstacoli, ostacoli))
        {
            Termina();
            return;
        }
        if (controllo.Aggiorna(movimento.Posizione))
        {
            Termina();
            return;
        }
    }

    private void Termina()
    {
        Terminato = true;
        movimento.Ferma();
    }


    public void Esci()
    {
        movimento.Ferma();
    }

}
