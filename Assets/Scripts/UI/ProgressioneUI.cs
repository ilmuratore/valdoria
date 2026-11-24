using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ProgressioneUI : MonoBehaviour
{
    [Header("Dati")]
    [SerializeField] private EsperienzaGiocatore esperienza;
    [SerializeField] private StatistichePersonaggio statistiche;
    [Header("Scheda Personaggio")]
    [SerializeField] private GameObject pannelloPersonaggio;
    [SerializeField] private TextMeshProUGUI testoLivello;
    [SerializeField] private TextMeshProUGUI testoEsperienza;
    [SerializeField] private TextMeshProUGUI testoStatistiche;
    [SerializeField] private Image riempimentoEsperienza;
    [Header("Messaggio level UP")]
    [SerializeField] private TextMeshProUGUI testoLevelUp;
    [SerializeField] private float durataMessaggio;

    private InputAction azioneScheda;
    private Coroutine messaggioCorrente;

    private void Awake()
    {
        azioneScheda = InputSystem.actions.FindAction("Player/Character", true);
        pannelloPersonaggio.SetActive(false);
        testoLevelUp.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        esperienza.ProgressioneModificata += AggiornaUI;
        esperienza.LivelloAumentato += MostraLevelUp;
        statistiche.ValoriModificati += AggiornaUI;
    }

    private void Start()
    {
        AggiornaUI();
    }

    private void Update()
    {
        if (azioneScheda.WasPressedThisFrame())
        {
            pannelloPersonaggio.SetActive(!pannelloPersonaggio.activeSelf);
        }
    }

    private void OnDisable()
    {
        esperienza.ProgressioneModificata -= AggiornaUI;
        esperienza.LivelloAumentato -= MostraLevelUp;
        statistiche.ValoriModificati -= AggiornaUI;
    }

    private void AggiornaUI()
    {
        testoLivello.text = $"Livello {esperienza.Livello}";
        testoEsperienza.text = $"XP: {esperienza.Esperienza} / {esperienza.EsperienzaNecessaria}";
        testoStatistiche.text =
            $"Vita: {statistiche.VitaAttuale} / {statistiche.VitaMassima}\n" +
            $"Forza: {statistiche.Forza}\n" +
            $"Difesa: {statistiche.Difesa}";
        riempimentoEsperienza.fillAmount = (float)esperienza.Esperienza / esperienza.EsperienzaNecessaria;
    }

    private void MostraLevelUp(int livello)
    {
        if(messaggioCorrente != null)
        {
            StopCoroutine(messaggioCorrente);
        }
        messaggioCorrente = StartCoroutine(MessaggioTemporaneo(livello));
    }

    private IEnumerator MessaggioTemporaneo(int livello)
    {
        testoLevelUp.text = $"LEVEL UP! Livello {livello}";
        testoLevelUp.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(durataMessaggio);
        testoLevelUp.gameObject.SetActive(false);
        messaggioCorrente = null;
    }

}
