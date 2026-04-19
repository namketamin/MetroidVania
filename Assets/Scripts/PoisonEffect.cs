using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 1f;
    [SerializeField] private float tickTime=1.5f;
    [SerializeField] private float duration=10f;

    PlayerHealth ph;
    PlayerAudio pa;
    Coroutine poisonCo;
    private void Awake()
    {
        ph = GetComponentInParent<PlayerHealth>();
        pa=ph.GetComponent<PlayerAudio>();
    }
    public void ApplyPoison()
    {
        if (ph.isImmune) return;
        if (poisonCo != null) return;
        ph.isPoisoned = true;
        poisonCo= StartCoroutine(PoisonRoutine());
    }
    public void StopPoison()
    {
        if (poisonCo != null)
        {
            StopCoroutine(poisonCo);
            poisonCo= null;
        }
    }
    IEnumerator PoisonRoutine()
    {
        float t = 0;
        while (t < duration)
        {
            if (ph.isImmune)
            {
                StopPoison();
                yield break;
            }
            pa.PlaySFXClip(pa.poisonedClip);
            ph.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickTime);
            t+=tickTime;
        }
        ph.isPoisoned = false;
        poisonCo = null;
    }
}
