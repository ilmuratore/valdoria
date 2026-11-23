
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StatistichePersonaggio))]
public class AttaccoPersonaggio : MonoBehaviour
{
    [Header("Area di attacco")]
    [SerializeField] private float distanzaColpo = 0.55f;
    [SerializeField] private float raggioColpo = 0.45f;

    [Header("Tempi")]
    [SerializeField] private float intervalloAttacchi = 0.8f;

    [Header("Collisioni")]
    [SerializeField] private LayerMask layerBersagli;
    [SerializeField] private LayerMask ostacoliColpo;

    private Animator animatore;
    private StatistichePersonaggio statistiche;

    private Vector2 direzioneAttacco = Vector2.down;
    private float prossimoAttacco;
    private bool colpoApplicato;

    public bool StaAttaccando { get; private set; }

    private void Awake()
    {
        animatore = GetComponent<Animator>();
        statistiche = GetComponent<StatistichePersonaggio>();
    }

    public bool ProvaAttacco(Vector2 direzione)
    {
        if (statistiche.VitaAttuale <= 0 ||
            StaAttaccando ||
            Time.time < prossimoAttacco ||
            direzione.sqrMagnitude < 0.001f)
        {
            return false;
        }

        direzioneAttacco = direzione.normalized;

        StaAttaccando = true;
        colpoApplicato = false;

        prossimoAttacco = Time.time + intervalloAttacchi;

        animatore.SetTrigger("Attacca");

        return true;
    }

    public void ApplicaColpo()
    {
        if (!StaAttaccando ||
            colpoApplicato ||
            statistiche.VitaAttuale <= 0)
        {
            return;
        }

        colpoApplicato = true;

        Vector2 centro = (Vector2)transform.position +
            direzioneAttacco * distanzaColpo;

        Collider2D[] colpiti = Physics2D.OverlapCircleAll(
            centro,
            raggioColpo,
            layerBersagli
        );

        HashSet<IDanneggiabile> bersagliColpiti =
            new HashSet<IDanneggiabile>();

        foreach (Collider2D collider in colpiti)
        {
            IDanneggiabile bersaglio =
                collider.GetComponentInParent<IDanneggiabile>();

            if (bersaglio == null ||
                bersaglio == (IDanneggiabile)statistiche)
            {
                continue;
            }

            if (bersaglio is StatistichePersonaggio vitaBersaglio &&
                vitaBersaglio.VitaAttuale <= 0)
            {
                continue;
            }

            if (!bersagliColpiti.Add(bersaglio))
                continue;

            RaycastHit2D ostacolo = Physics2D.Linecast(
                transform.position,
                collider.ClosestPoint(transform.position),
                ostacoliColpo
            );

            if (ostacolo.collider != null)
                continue;

            bersaglio.SubisciDanno(statistiche.Forza);
        }
    }
    public void FineAttacco()
    {
        StaAttaccando = false;
    }

    private void OnDisable()
    {
        StaAttaccando = false;
        colpoApplicato = false;

        if (animatore != null)
            animatore.ResetTrigger("Attacca");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 centro = (Vector2)transform.position +
            direzioneAttacco * distanzaColpo;

        Gizmos.DrawWireSphere(centro, raggioColpo);
    }
}
