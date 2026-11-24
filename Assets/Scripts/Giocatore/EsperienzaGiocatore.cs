using System;
using UnityEngine;

[RequireComponent(typeof(Giocatore))]
[RequireComponent(typeof(StatistichePersonaggio))]
public class EsperienzaGiocatore : MonoBehaviour
{
    [Header("Bonus per livello")]
    [SerializeField] private int bonusVita = 10;
    [SerializeField] private int bonusForza = 2;
    [SerializeField] private int bonusDifesa = 1;

    public int Livello { get; private set; } = 1;
    public int Esperienza { get; private set; }
    public int EsperienzaNecessaria { get { return 100 + (Livello + 1) * 50; } }
    public event Action ProgressioneModificata;
    public event Action<int> LivelloAumentato;
    private StatistichePersonaggio statistiche;
    
    private void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();
    }

    public void AggiungiEsperienza(int quantita)
    {
        if(quantita <= 0 || statistiche.VitaAttuale <= 0) { return;  }
        Esperienza += quantita;
        while(Esperienza >= EsperienzaNecessaria)
        {
            Esperienza -= EsperienzaNecessaria;
            Livello++;
            statistiche.AumentaStatistiche(bonusVita, bonusForza, bonusDifesa);
            LivelloAumentato?.Invoke(Livello);
        }
        ProgressioneModificata?.Invoke();
    }

    [ContextMenu("TEST - Aggiungi 40 XP")]
    private void TestEsperienza()
    {
        AggiungiEsperienza(40);
    }

}