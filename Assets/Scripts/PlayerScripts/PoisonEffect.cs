using System.Collections;
using TreeEditor;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 1f;
    [SerializeField] private float tickTime=1.5f;
    [SerializeField] private float duration=10f;

    public bool IsPoisoned { get; private set; }

    private PlayerHealth playerHealth;
    private PlayerAudio playerAudio;
    private Coroutine poisonCo;
    private void Awake()
    {
        Transform player = transform.root;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerAudio=player.GetComponentInChildren<PlayerAudio>();
    }
    public void ApplyPoison()
    {
        if (playerHealth.IsImmune) return;
        if (poisonCo != null) return;
        IsPoisoned = true;
        poisonCo= StartCoroutine(PoisonRoutine());
    }
    public void StopPoison()
    {
        if (poisonCo != null)
        {
            StopCoroutine(poisonCo);
            poisonCo= null;
        }
        IsPoisoned = false;
    }
    IEnumerator PoisonRoutine()
    {
        Debug.Log("playerHealth = " + playerHealth);
        Debug.Log("playerAudio = " + playerAudio);
        float t = 0;
        while (t < duration)
        {
            if (playerHealth.IsImmune)
            {
                StopPoison();
                yield break;
            }
            playerAudio.PlaySFXClip(PlayerSFX.Hurt);
            playerHealth.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickTime);
            t+=tickTime;
        }
        IsPoisoned = false;
        poisonCo = null;
    }
}
