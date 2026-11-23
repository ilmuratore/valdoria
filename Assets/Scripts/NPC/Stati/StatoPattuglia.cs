using UnityEngine;

public class StatoPattuglia : IStatoNPC
{

    private MovimentoNPC movimento;
    [SerializeField] private Transform[] punti;
    private float pausaMinima;
    private float pausaMassima;
    private int indicePunto;
    private float tempoPausa;

    public StatoPattuglia (MovimentoNPC movimento, Transform[] punti, float pausaMinima, float pausaMassima)
    {
        this.movimento = movimento;
        this.punti = punti;
        this.pausaMinima = pausaMinima;
        this.pausaMassima = pausaMassima;
    }

    public void Entra()
    {
        tempoPausa = 0f;
        movimento.Ferma();
    }

    public void Aggiorna()
    {
        if (punti == null || punti.Length == 0)
        {
            movimento.Ferma();
            return;
        }

        if (tempoPausa > 0f)
        {
            tempoPausa -= Time.fixedDeltaTime;
            movimento.Ferma();
            return;
        }

        Transform punto = punti[indicePunto];
        if(punto == null)
        {
            ProssimoPunto();
            movimento.Ferma();
            return;
        }

        Vector2 destinazione = punto.position;
        if (movimento.Arrivato(destinazione))
        {
            ProssimoPunto();
            tempoPausa = Random.Range(pausaMinima, pausaMassima);
            movimento.Ferma();
        }
        else
        {
            movimento.MuoviVerso(destinazione);
        }
    }

    public void ProssimoPunto()
    {
        indicePunto = (indicePunto + 1) % punti.Length;
    }
    public bool SelezionaPuntoRaggiungibile(float raggioOstacoli, LayerMask ostacoli, out Vector2 destinazione)
    {
        destinazione = movimento.Posizione;
        if (punti == null || punti.Length == 0) { return false; }
        float distanzaMinima = float.PositiveInfinity;
        int indiceMigliore = -1;
        for (int i = 0; i < punti.Length; i++)
        {
            if (punti[i] == null) continue;
            Vector2 punto = punti[i].position;
            if(!movimento.PercorsoLibero(punto, raggioOstacoli, ostacoli)) { continue; }
            float distanza = Vector2.Distance(movimento.Posizione, punto);
            if(distanza < distanzaMinima)
            {
                distanzaMinima = distanza;
                indiceMigliore = i;
            }
        }
        if(indiceMigliore < 0) { return false; }
        indicePunto = indiceMigliore;
        destinazione = punti[indiceMigliore].position;
        return true;
    }

    public void Esci()
    {
        movimento.Ferma();
    }
}
