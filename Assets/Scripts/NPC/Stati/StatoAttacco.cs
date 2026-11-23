using UnityEngine;

public class StatoAttacco : IStatoNPC
{
    private MovimentoNPC movimento;
    private RilevamentoNPC rilevamento;
    private AttaccoPersonaggio attacco;

    public StatoAttacco(MovimentoNPC movimento, RilevamentoNPC rilevamento, AttaccoPersonaggio attacco)
    {
        this.movimento = movimento;
        this.rilevamento = rilevamento;
        this.attacco = attacco;
    }

    public void Entra()
    {
        movimento.Ferma();
    }
    public void Aggiorna()
    {
        movimento.Ferma();
        if(attacco.StaAttaccando) { return; }
        Vector2 posizione = rilevamento.PosizioneBersaglio;
        movimento.GuardaVerso(posizione);
        Vector2 direzione = posizione - movimento.Posizione;
        attacco.ProvaAttacco(direzione);
    }
    public void Esci()
    {
        movimento.Ferma();
    }
}
