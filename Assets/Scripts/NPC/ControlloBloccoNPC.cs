using UnityEngine;

public class ControlloBloccoNPC
{
    private float tempoFermo;
    private float tempoMassimo;
    private float spostamentoMinimo;
    private Vector2 ultimaPosizione;

    public ControlloBloccoNPC(float tempoMassimo = 1.5f, float spostamentoMinimo = 0.05f)
    {
        this.tempoMassimo = tempoMassimo;
        this.spostamentoMinimo = spostamentoMinimo;
    }

    public void Reimposta(Vector2 posizione)
    {
        ultimaPosizione = posizione;
        tempoFermo = 0f;

    }

    public bool Aggiorna(Vector2 posizione)
    {
        float distanza = Vector2.Distance(posizione, ultimaPosizione);
        if(distanza >= spostamentoMinimo)
        {
            ultimaPosizione = posizione;
            tempoFermo = 0f;
            return false;
        }
        tempoFermo += Time.deltaTime;
        return tempoFermo >= tempoMassimo;
    }
}
