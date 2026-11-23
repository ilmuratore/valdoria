using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class RilevamentoNPC : MonoBehaviour
{
    [Header("Bersaglio")]
    [SerializeField] private MonoBehaviour componenteBersaglio;


    [Header("Distanze")]
    [SerializeField] private float raggioRilevamento = 4f;

    [SerializeField] private float raggioPerdita = 6f;

    [Header("Visibilitá")]
    [SerializeField] LayerMask ostacoliVista;

    private IBersaglio bersaglio;



    public Vector2 PosizioneBersaglio
    {
        get
        {
            return bersaglio != null ? bersaglio.Posizione : Vector2.zero;
        }
    }
    public bool BersaglioAttivo
    {
        get { return bersaglio != null && bersaglio.EAttivo; }
    }

    private void Awake()
    {
       
        bersaglio = componenteBersaglio as IBersaglio;

        if (bersaglio == null)
        {
            Debug.LogError("Il componente assegnato non implementa IBersaglio.", this);
        }
    }

    public bool VedeBersaglio(bool giaInseguendo)
    {
        if(bersaglio == null || !bersaglio.EAttivo)
        {
            return false;
        }

        float raggio = giaInseguendo ? Mathf.Max(raggioPerdita, raggioRilevamento) : raggioRilevamento;
        float distanza = Vector2.Distance(transform.position, bersaglio.Posizione);

        if (distanza > raggio) return false;

        RaycastHit2D ostacolo = Physics2D.Linecast(
            transform.position,
            bersaglio.Posizione,
            ostacoliVista
            );

        return ostacolo.collider == null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, raggioRilevamento);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raggioPerdita);

    }
}
