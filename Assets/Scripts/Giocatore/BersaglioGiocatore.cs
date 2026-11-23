using UnityEngine;

[RequireComponent(typeof(StatistichePersonaggio))]
public class BersaglioGiocatore : MonoBehaviour, IBersaglio
{
    private StatistichePersonaggio statistiche;

    private void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();

    }

    public Vector2 Posizione
    {
        get { return transform.position; }
    }

    public bool EAttivo
    {
        get { return statistiche.VitaAttuale > 0; }
    }

}
