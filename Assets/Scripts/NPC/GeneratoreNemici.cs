using UnityEngine;
using UnityEngine.Rendering;

public class GeneratoreNemici : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefabNemico;
    [Header("Bersaglio")]
    [SerializeField] private BersaglioGiocatore giocatore;
    [Header("Punti di generazione")]
    [SerializeField] private Transform[] puntiGenerazione;

    private void Start()
    {
        if(prefabNemico == null || giocatore == null)
        {
            Debug.LogError("PRefab o giocatore non assegnato.", this);
        }
        foreach(Transform punto in puntiGenerazione)
        {
            if (punto == null) continue;
            GameObject nemico = Instantiate(prefabNemico, punto.position, punto.rotation);
            RilevamentoNPC rilevamento = nemico.GetComponent<RilevamentoNPC>();
            if(rilevamento != null) { rilevamento.ImpostaBersaglio(giocatore); }

        }
        
    }
}
