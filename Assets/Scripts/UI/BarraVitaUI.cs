using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarraVitaUI : MonoBehaviour
{
    [SerializeField] private StatistichePersonaggio statistiche;

    [SerializeField] private Image riempimentoVita;

    [SerializeField] private TextMeshProUGUI testoVita;

    [SerializeField] private Animator animatoreTesto;

    private int vitaPrecedente;
    // private int vitaPrecedente2;
    public void Start()
    {
        vitaPrecedente = statistiche.VitaAttuale;
        AggiornaUI();

    }
    private void Update()
    {
        if(statistiche.VitaAttuale == vitaPrecedente)
        {
            return;
        }
        vitaPrecedente = statistiche.VitaAttuale;
        AggiornaUI();

        animatoreTesto.SetTrigger("CambioVita");
    }

    private void AggiornaUI()
    {
        
        float percentualeVita = (float)statistiche.VitaAttuale / statistiche.VitaMassima;

        riempimentoVita.fillAmount = percentualeVita;

        testoVita.text = $"HP: {statistiche.VitaAttuale} / {statistiche.VitaMassima}";

    }

}
