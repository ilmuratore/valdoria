using UnityEngine;

public class StatoInseguimento : IStatoNPC
{
    private MovimentoNPC movimento;
    private RilevamentoNPC rilevamento;
    private float velocita;
    private float distanzaArresto;

    public StatoInseguimento(MovimentoNPC movimento, RilevamentoNPC rilevamento, float velocita, float distanzaArresto)
    {
        this.movimento = movimento;
        this.rilevamento = rilevamento;
        this.velocita = velocita;
        this.distanzaArresto = distanzaArresto;
    }

    public void Entra()
    {
        movimento.Ferma();
    }

    public void Aggiorna()
    {
        Vector2 destinazione = rilevamento.PosizioneBersaglio;
        float distanza = Vector2.Distance(movimento.Posizione, destinazione);
        if(distanza <= distanzaArresto)
        {
            movimento.Ferma();
            return;
        }
        movimento.MuoviVerso(destinazione, velocita);
    }

    public void Esci()
    {
        movimento.Ferma();
    }
}
