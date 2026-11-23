using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovimentoGiocatore : MonoBehaviour
{
    [SerializeField] private float velocita = 5f;
    private Rigidbody2D corpo;
    private Animator animatore;
    private InputAction azioneMovimento;
    private Vector2 direzione;

    private AttaccoPersonaggio attacco;
    private StatistichePersonaggio statistiche; 

    private void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();
        animatore = GetComponent<Animator>();
        attacco = GetComponent<AttaccoPersonaggio>();
        statistiche = GetComponent<StatistichePersonaggio>();
        azioneMovimento = InputSystem.actions.FindAction("Player/Move", true);
    }

    private void Update()
    {
        if(statistiche.VitaAttuale <= 0 || attacco.StaAttaccando)
        {
            direzione = Vector2.zero;
            animatore.SetBool("isMoving", false);
            return;
        }

        direzione = azioneMovimento.ReadValue<Vector2>();
        direzione = Vector2.ClampMagnitude(direzione, 1f);

        bool inMovimento = direzione.sqrMagnitude > 0.001f;

        if (inMovimento)
        {
            Vector2 orientamento = direzione.normalized;
            animatore.SetFloat("MoveX", orientamento.x);
            animatore.SetFloat("MoveY", orientamento.y);
        }

        animatore.SetBool("isMoving", inMovimento);
    }

    private void FixedUpdate()
    {
        if (statistiche.VitaAttuale <= 0 || attacco.StaAttaccando)
        {
            corpo.linearVelocity = Vector2.zero;
            return;
        }
        corpo.linearVelocity = direzione * velocita;
    }
}
