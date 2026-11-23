using UnityEngine;

public class StatistichePersonaggio : MonoBehaviour, IDanneggiabile, ICurabile
{
    [Header("Attributi")]
    [SerializeField] private int vitaMassima = 100;
    [SerializeField] private int forza = 10;
    [SerializeField] private int difesa = 5;

    public int VitaAttuale { get; private set; }
    public int VitaMassima
    {
        get { return vitaMassima; }
    }
    public int Forza
    {
        get { return forza; }
    }

    public void Awake()
    {
        vitaMassima = Mathf.Max(1, vitaMassima);

        VitaAttuale = vitaMassima;
    }

    public void SubisciDanno(int danno)
    {
        if (danno <= 0)
        {
            return;
        }

        VitaAttuale = Mathf.Max(0, VitaAttuale - danno);
        Debug.Log(VitaAttuale);
    }
    public void Cura(int quantita)
    {
        if (quantita <= 0)
        {
            return;
        }
        VitaAttuale = Mathf.Min(vitaMassima, VitaAttuale + quantita);
        Debug.Log(VitaAttuale);
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
