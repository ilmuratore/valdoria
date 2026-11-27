using UnityEngine;

public class MissioneEsplorazione : Missione
{
    [Header("Esplorazione")]
    [SerializeField] private string nomeLuogo = "rovine";
    [SerializeField] private AreaObiettivoMissione areaDestinazione;
    private bool luogoRaggiunto;

    public override string TestoObiettivo => luogoRaggiunto 
        ? $"{nomeLuogo}: obiettivo raggiunto, torna indietro" 
        : $"Raggiungi {nomeLuogo}";

    protected override bool VerificaPreparazione()
    {
        return areaDestinazione != null && areaDestinazione.Missione == this;
    }

    protected override void AvviaObiettivi()
    {
        luogoRaggiunto = false;
    }

    public void RegistraArrivo(BersaglioGiocatore personaggio)
    {
        if (personaggio != giocatore || luogoRaggiunto || Stato != StatoMissione.InCorso) return;
        luogoRaggiunto = true;
        ObiettiviCompletati();
    }
}
