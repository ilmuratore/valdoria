using UnityEngine;

[RequireComponent(typeof(StatistichePersonaggio))]
public abstract class Personaggio : MonoBehaviour
{
    [Header("Identitá")]
    [SerializeField] private string nomePersonaggio = "Personaggio";

    protected StatistichePersonaggio statistiche;

    public string Nome
    {
        get { return nomePersonaggio; }

    }

    public StatistichePersonaggio Statistiche
    {
        get { return statistiche;  }
    }

    public bool EVivo
    {
        get { return statistiche.VitaAttuale > 0; }
    }

    protected virtual void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();
    }
}
