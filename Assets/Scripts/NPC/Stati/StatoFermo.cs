using UnityEngine;

public class StatoFermo : IStatoNPC
{
    private MovimentoNPC movimento;

    public StatoFermo(MovimentoNPC movimento)
    {
        this.movimento = movimento;
    }

    public void Entra()
    {
        movimento.Ferma();
    }
    public void Aggiorna() { }

    public void Esci() { }
}
