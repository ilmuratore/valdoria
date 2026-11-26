using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMissioni : MonoBehaviour
{
    [Header("Missione")]
    [SerializeField] private MissioneVillaggio missione;
    [Header("Dialogo e Storia")]
    [SerializeField] private GameObject pannelloDialogo;
    [SerializeField] private TextMeshProUGUI titoloDialogo;
    [SerializeField] private TextMeshProUGUI testoDialogo;
    [SerializeField] private Button pulsanteAccetta;
    [SerializeField] private Button pulsanteConsegna;
    [SerializeField] private Button pulsanteChiudi;
    [Header("Diaro e HUD")]
    [SerializeField] private GameObject pannelloDiario;
    [SerializeField] private TextMeshProUGUI testoDiario;
    [SerializeField] private TextMeshProUGUI testoObiettivoHUD;

    private InputAction azioneDiario;

    private void Awake()
    {
        azioneDiario = InputSystem.actions.FindAction("Player/QuestJournal", true);
        pannelloDialogo.SetActive(false);
        pannelloDiario.SetActive(false);

        pulsanteAccetta.onClick.AddListener(Accetta);
        pulsanteConsegna.onClick.AddListener(Consegna);
        pulsanteChiudi.onClick.AddListener(ChiudiDialogo);
    }

    private void OnEnable()
    {
        missione.MissioneModificata += AggiornaUI;
    }

    private void Start()
    {
        AggiornaUI();
    }

    private void OnDisable()
    {
        missione.MissioneModificata -= AggiornaUI;
    }

    private void OnDestroy()
    {
        if (pulsanteAccetta != null) pulsanteAccetta.onClick.RemoveListener(Accetta);
        if (pulsanteConsegna != null) pulsanteConsegna.onClick.RemoveListener(Consegna);
        if (pulsanteChiudi != null) pulsanteAccetta.onClick.RemoveListener(ChiudiDialogo);
    }

    private void Update()
    {
        if (!azioneDiario.WasCompletedThisFrame()) return;
        pannelloDialogo.SetActive(false);
        pannelloDiario.SetActive(!pannelloDiario.activeSelf);
    }

    public void ApriOChiudiDialogo()
    {
        if (pannelloDialogo.activeSelf)
        {
            ChiudiDialogo();
            return;
        }
        pannelloDiario.SetActive(false);
        AggiornaUI();
        pannelloDialogo.SetActive(true);
    }

    private void Accetta()
    {
        if (missione.Accetta()) { ChiudiDialogo(); }
    }

    private void Consegna()
    {
        missione.Consegna();
    }

    private void ChiudiDialogo()
    {
        pannelloDialogo.SetActive(false);
    }

    private void AggiornaUI()
    {
        titoloDialogo.text = missione.Titolo;
        pulsanteAccetta.gameObject.SetActive(missione.Stato == StatoMissione.NonAccettata);
        pulsanteConsegna.gameObject.SetActive(missione.Stato == StatoMissione.DaConsegnare);
        switch (missione.Stato)
        {
            case StatoMissione.NonAccettata:
                testoDialogo.text = missione.Descrizione + "\n\nVuoi aiutarci a liberare il villaggio?";
                testoDiario.text = "Non hai ancora accettato la missione.";
                testoObiettivoHUD.text = "";
                break;
            case StatoMissione.InCorso:
                testoDialogo.text = "I goblin sono ancora nel villaggio. \n" + "Torna da me quando li avrai sconfitti";
                testoDiario.text = $"{missione.Titolo}\n" + $"Elimina i nemici: {missione.Eliminati}/{missione.Totale}";
                testoObiettivoHUD.text = $"Nemici eliminati: {missione.Eliminati}/{missione.Totale}";
                break;
            case StatoMissione.DaConsegnare:
                testoDialogo.text = "Hai sconfitto tutti i goblin.\n" + "Grazie per aver salvato il villaggio.";
                testoDiario.text = $"{missione.Titolo}\n" + $"Obiettivo raggiungo: {missione.Eliminati}/{missione.Totale}\n" + "Torna dal monaco per la ricompensa.";
                testoObiettivoHUD.text = "Torna dal monaco al villaggio.";
                break;
            case StatoMissione.Completata:
                testoDialogo.text = "Il villaggio é salvo, grazie mille.\n" + $"Hai ricevuto {missione.RicompensaXP} XP.";
                testoDiario.text = $"{missione.Titolo}\n Missione Completata!.";
                testoObiettivoHUD.text = "";
                break;
        }
    }

}
