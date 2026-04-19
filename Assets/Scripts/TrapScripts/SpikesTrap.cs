using System.Collections;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private float interval = 2f;
    [SerializeField] private float holdExtendTime=1f;

    [SerializeField] private AnimationClip extendClip;
    [SerializeField] private AnimationClip retractClip;

    [SerializeField] private float spikesDmg = 1f;
    BoxCollider2D dmgCol;
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        dmgCol = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        dmgCol.enabled = false;
        StartCoroutine(SpikeRoutine());
    }
    private void Update()
    {

    }
    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            anim.Play("SpikesIdle"); 
            yield return new WaitForSeconds(interval);

            anim.Play("SpikesExtend");
            yield return new WaitForSeconds(extendClip.length);
            dmgCol.enabled = true;
            yield return new WaitForSeconds(holdExtendTime);

            anim.Play("SpikesRetract");
            dmgCol.enabled = false;
            yield return new WaitForSeconds(retractClip.length);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var ph = collision.gameObject.GetComponent<PlayerHealth>();
            ph.TakeDamage(spikesDmg);
        }
    }
}
