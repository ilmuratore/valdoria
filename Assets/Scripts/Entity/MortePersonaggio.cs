using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(StatistichePersonaggio))]
[RequireComponent(typeof(Animator))]
public class MortePersonaggio : MonoBehaviour
{
    [Header("UI giocatore")]
    [SerializeField] private GameObject pannelloGameOver;

    private StatistichePersonaggio statistiche;
    private Animator animatore;
    private bool morto;

    private void Awake()
    {
        statistiche = GetComponent<StatistichePersonaggio>();
        animatore = GetComponent<Animator>();
        if(pannelloGameOver != null) { pannelloGameOver.SetActive(false); }
    }

    private void OnEnable()
    {
        statistiche.Morto += GestisciMorte;
    }

    private void OnDisable()
    {
        statistiche.Morto -= GestisciMorte;
    }

    private void GestisciMorte(Personaggio attaccante)
    {
        if(morto) { return;  }
        morto = true;
        CervelloNPC cervello = GetComponent<CervelloNPC>();
        if(cervello != null) { cervello.enabled = false;  }
        MovimentoNPC movimentoNPC = GetComponent<MovimentoNPC>();
        if(movimentoNPC != null) { movimentoNPC.enabled = false;  }
        MovimentoGiocatore movimentoGiocatore = GetComponent<MovimentoGiocatore>();
        if(movimentoGiocatore != null) { movimentoGiocatore.enabled = false; }
        AttaccoGiocatore attaccoGiocatore = GetComponent<AttaccoGiocatore>();
        if (attaccoGiocatore != null) { attaccoGiocatore.enabled = false; }
        AttaccoPersonaggio attacco = GetComponent<AttaccoPersonaggio>();
        if(attacco != null ) { attacco.enabled = false; }
        Rigidbody2D corpo = GetComponent<Rigidbody2D>();
        if(corpo != null)
        {
            corpo.linearVelocity = Vector2.zero;
            corpo.simulated = false;
        }
        if(GetComponent<Giocatore>() != null) { animatore.SetBool("isMoving", false);  }
        animatore.ResetTrigger("Attacca");
        animatore.SetTrigger("Morte");
    }

    public void FineMorte()
    {
        if (!morto) { return; }
        if(GetComponent<Nemico>() != null) { Destroy(gameObject); return; }
        if(GetComponent<Giocatore>() != null /*&& pannelloGameOver != null*/) { pannelloGameOver.SetActive(true);  }
    }

    public void Ricomincia()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
