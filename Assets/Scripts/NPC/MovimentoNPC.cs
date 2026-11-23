using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovimentoNPC : MonoBehaviour
{
    [Header("Comportamento")]
    [SerializeField] private float velocita = 2.5f;
    [SerializeField] private Vector2 direzioneIniziale = Vector2.down;
    [SerializeField] private float distanzaArrivo = 0.8f;

    private Rigidbody2D corpo;
    private Animator animatore;
    private Vector2 destinazione;
    private Vector2 direzioneGuardata;
    private float velocitaRichiesta;
    private bool haDestinazione;

    public Vector2 Posizione
    {
        get { return corpo.position; }
    }

    private void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();
        animatore = GetComponent<Animator>();

        direzioneGuardata = direzioneIniziale.sqrMagnitude > 0.1f ? direzioneIniziale.normalized : Vector2.down;
    }

    private void Start()
    {
       AggiornaAnimazione(Vector2.zero, false);
    }

    public void MuoviVerso(Vector2 punto, float velocitaPersonalizzata = -1f)
    {
        destinazione = punto;
        velocitaRichiesta = velocitaPersonalizzata > 0f ? velocitaPersonalizzata : velocita;

        haDestinazione = true;
    }

    public bool Arrivato(Vector2 punto)
    {
        return Vector2.Distance(corpo.position, punto) <= distanzaArrivo;
    }

    public void Ferma()
    {
        haDestinazione = false;
    }

    private void FixedUpdate()
    {
       if(!haDestinazione || Arrivato(destinazione))
       {
            corpo.linearVelocity = Vector2.zero;
            AggiornaAnimazione(Vector2.zero, false);
            return;
       }

        Vector2 spostamento = destinazione - corpo.position;
        Vector2 direzione = spostamento.normalized;

        float velocitaEffettiva = Mathf.Min(
            velocitaRichiesta,
            spostamento.magnitude / Time.fixedDeltaTime
            );

        corpo.linearVelocity = direzione * velocitaEffettiva;

        AggiornaAnimazione(direzione, true);
    }

    private void AggiornaAnimazione(Vector2 direzione, bool cammina)
    {
        if (cammina && direzione.sqrMagnitude > 0.1f)
        {
            direzioneGuardata = direzione.normalized;
        }

        animatore.SetFloat("DirezioneX", direzioneGuardata.x);
        animatore.SetFloat("DirezioneY", direzioneGuardata.y);
        animatore.SetBool("Cammina", cammina);
    }
    public bool PercorsoLibero(Vector2 punto, float raggio, LayerMask ostacoli)
    {
        Vector2 spostamento = punto - corpo.position;
        if (spostamento.sqrMagnitude < 0.01f) { return true; }
        RaycastHit2D risultato = Physics2D.CircleCast(
        corpo.position,
        Mathf.Max(0.01f, raggio),
        spostamento.normalized,
        spostamento.magnitude,
        ostacoli
        );
        return risultato.collider == null;
    }

    public void GuardaVerso(Vector2 punto)
    {
        Vector2 direzione = punto - corpo.position;
        if (direzione.sqrMagnitude < 0.01f) { return; }
        direzioneGuardata = direzione.normalized;
        AggiornaAnimazione(Vector2.zero, false);
    }
    private void OnDisable()
    {
        haDestinazione = false;

        if (corpo != null)
        {
            corpo.linearVelocity = Vector2.zero;
        }

        if(animatore != null)
        {
            AggiornaAnimazione(Vector2.zero, false);
        }
    }
}

