using System;
using UnityEngine;

public class StatistichePersonaggio : MonoBehaviour, IDanneggiabile, ICurabile
{
    [Header("Attributi")]
    [SerializeField] private int vitaMassima = 100;
    [SerializeField] private int forza = 10;
    [SerializeField] private int difesa = 5;

    public int VitaAttuale { get; private set; }
    public int VitaMassima { get { return vitaMassima; }}
    public int Forza { get { return forza; }}
    public int Difesa { get { return difesa; }}

    public event Action ValoriModificati;
    public event Action<Personaggio> Morto;

    public void Awake()
    {
        vitaMassima = Mathf.Max(1, vitaMassima);
        forza = Mathf.Max(0, forza);
        difesa = Mathf.Max(0, difesa);
        VitaAttuale = vitaMassima;
    }

    public void SubisciDanno(int danno)
    {
        SubisciDanno(danno, null);
    }

    public void SubisciDanno(int danno, Personaggio attaccante)
    {
        if (danno <= 0 || VitaAttuale <= 0){ return;}
        int dannoEffettivo = Mathf.Max(1, danno - difesa);
        VitaAttuale = Mathf.Max(0, VitaAttuale - danno);
        ValoriModificati?.Invoke();
        if(VitaAttuale == 0) { Morto?.Invoke(attaccante); }
    }
    public void Cura(int quantita)
    {
        if (quantita <= 0 || VitaAttuale <= 0) { return; }

        int nuovaVita = Mathf.Min(vitaMassima, VitaAttuale + quantita);
        if(nuovaVita == VitaAttuale) { return; }
        VitaAttuale = nuovaVita;
        ValoriModificati?.Invoke();
    }

    public void AumentaStatistiche(int bonusVita, int bonusForza, int bonusDifesa)
    {
        if(VitaAttuale <= 0) { return; }
        vitaMassima += Mathf.Max(0, bonusVita);
        forza += Mathf.Max(0, bonusForza);
        difesa += Mathf.Max(0, bonusDifesa);
        VitaAttuale = VitaMassima;
        ValoriModificati?.Invoke();
    }

    [ContextMenu("TEST - Subisci 20 danni")]
    private void TestDanno()
    {
        SubisciDanno(20);
    }


    [ContextMenu("TEST - Cura 20 Punti")]
    private void TestCura()
    {
        Cura(20);
    }

}
