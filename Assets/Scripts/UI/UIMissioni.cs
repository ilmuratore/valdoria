using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMissioni : MonoBehaviour
{
    [Header("Missioni")]
    [SerializeField] private GestoreMissioni gestore;
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
    [SerializeField] private Button pulsanteTracciaSuccessiva;

    private InputAction azioneDiario;
    private Missione missioneDialogo;

    private void Awake()
    {
        azioneDiario = InputSystem.actions.FindAction("Player/QuestJournal", true);
        pannelloDialogo.SetActive(false);
        pannelloDiario.SetActive(false);

        pulsanteAccetta.onClick.AddListener(Accetta);
        pulsanteConsegna.onClick.AddListener(Consegna);
        pulsanteChiudi.onClick.AddListener(ChiudiDialogo);
        if (pulsanteTracciaSuccessiva != null) pulsanteTracciaSuccessiva.onClick.AddListener(TracciaSuccessiva);
    }

    private void OnEnable()
    {
        if(gestore != null)
            gestore.MissioniModificate += AggiornaUI;
       
    }

    private void Start()
    {
        AggiornaUI();
    }

    private void OnDisable()
    {
        if(gestore != null)
            gestore.MissioniModificate -= AggiornaUI;
    }

    private void OnDestroy()
    {
        if (pulsanteAccetta != null) pulsanteAccetta.onClick.RemoveListener(Accetta);
        if (pulsanteConsegna != null) pulsanteConsegna.onClick.RemoveListener(Consegna);
        if (pulsanteChiudi != null) pulsanteAccetta.onClick.RemoveListener(ChiudiDialogo);
        if (pulsanteTracciaSuccessiva != null) pulsanteTracciaSuccessiva.onClick.RemoveListener(TracciaSuccessiva);
    }

    private void Update()
    {
        if (!azioneDiario.WasCompletedThisFrame()) return;
        pannelloDialogo.SetActive(false);
        pannelloDiario.SetActive(!pannelloDiario.activeSelf);
    }

    public void ApriDialogo(Missione missione)
    {
        if (missione == null) return;
        if (pannelloDialogo.activeSelf)
        {
            ChiudiDialogo();
            return;
        }
        missioneDialogo = missione;
        pannelloDiario.SetActive(false);
        AggiornaDialogo();
        pannelloDialogo.SetActive(true);
    }

    private void Accetta()
    {
        if (missioneDialogo != null && missioneDialogo.Accetta())
        {
            gestore.ImpostaTracciata(missioneDialogo);
            ChiudiDialogo(); 
        }
    }

    private void Consegna()
    {
        if(missioneDialogo != null)
        {
            missioneDialogo.Consegna();
        }
    }

    private void ChiudiDialogo()
    {
        pannelloDialogo.SetActive(false);
    }

    private void TracciaSuccessiva()
    {
        if(gestore != null)
        {
            gestore.TracciaSuccessiva();
        }
    }

    private void AggiornaDialogo()
    {
        if (missioneDialogo == null) return;
        titoloDialogo.text = missioneDialogo.Titolo;
        testoDialogo.text = missioneDialogo.TestoDialogo;
        pulsanteAccetta.gameObject.SetActive(missioneDialogo.Stato == StatoMissione.NonAccettata);
        pulsanteConsegna.gameObject.SetActive(missioneDialogo.Stato == StatoMissione.DaConsegnare);
    }

    private void AggiornaUI()
    {
        AggiornaDialogo();
        if (gestore == null) return;

        StringBuilder diario = new StringBuilder();
        foreach(Missione missione in gestore.Missioni)
        {
            if (missione == null || missione.Stato == StatoMissione.NonAccettata) continue;
            string indicatore = gestore.MissioneTracciata == missione ? "> " : "";
            diario.AppendLine(indicatore + missione.Titolo);
            diario.AppendLine(missione.Stato.ToString());
            if(missione.Stato != StatoMissione.Completata)
            {
                diario.AppendLine(missione.TestoObiettivo);
            }
            diario.AppendLine();
        }

        testoDiario.text = diario.Length == 0
            ? "Non hai ancora accettato missioni"
            : diario.ToString();

        Missione tracciata = gestore.MissioneTracciata;
        testoObiettivoHUD.text = tracciata == null
            ? ""
            : tracciata.Titolo + "\n" + tracciata.TestoObiettivo;
    }

}
