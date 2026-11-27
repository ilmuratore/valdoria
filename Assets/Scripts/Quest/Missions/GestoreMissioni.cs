using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GestoreMissioni : MonoBehaviour
{
    [SerializeField] private Missione[] missioni;
    public IReadOnlyList<Missione> Missioni => missioni ?? Array.Empty<Missione>();
    public Missione MissioneTracciata { get; private set; }
    public event Action MissioniModificate;

    private void OnEnable()
    {
        if (missioni == null) return;
        foreach(Missione missione in missioni)
        {
            if (missione != null) missione.MissioneModificata += MissioneAggiornata;
        }
    }

    private void OnDisable()
    {
        if (missioni == null) return;
        foreach(Missione missione in missioni)
        {
            if (missione != null) missione.MissioneModificata -= MissioneAggiornata;
        }
    }

    private void Start()
    {
        MissioneAggiornata();
    }
    private static bool Attiva(Missione missione)
    {
        return missione != null && (missione.Stato == StatoMissione.InCorso || missione.Stato == StatoMissione.DaConsegnare);
    }

    private void MissioneAggiornata()
    {
        if (!Attiva(MissioneTracciata)) MissioneTracciata = CercaPrimaAttiva();
        MissioniModificate?.Invoke();
    }

    private Missione CercaPrimaAttiva()
    {
        if (missioni == null) return null;
        foreach(Missione missione in missioni)
        {
            if (Attiva(missione)) return missione;
        }
        return null;
    }

    public void ImpostaTracciata(Missione missione)
    {
        if (missioni == null || !Attiva(missione) || Array.IndexOf(missioni, missione) < 0) return;
        MissioneTracciata = missione;
        MissioniModificate?.Invoke();
    }

    public void TracciaSuccessiva()
    {
        if (missioni == null || missioni.Length == 0) return;
        int indiceCorrente = Array.IndexOf(missioni, MissioneTracciata);
        for(int i = 1; i <= missioni.Length; i++)
        {
            int indice = (indiceCorrente + i) % missioni.Length;
            if (!Attiva(missioni[indice])) continue;
            MissioneTracciata = missioni[indice];
            MissioniModificate?.Invoke();
            return;
        }
        MissioneTracciata = null;
        MissioniModificate?.Invoke();
    }
}
