using System.Collections;
using TreeEditor;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 1f;
    [SerializeField] private float tickTime=1.5f;
    [SerializeField] private float duration=10f;

    PlayerHealth playerHealth;
    PlayerAudio playerAudio;
    Coroutine poisonCo;
    private void Awake()
    {
        Transform player = transform.root;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerAudio=player.GetComponentInChildren<PlayerAudio>();
    }
    public void ApplyPoison()
    {
        if (playerHealth.isImmune) return;
        if (poisonCo != null) return;
        playerHealth.isPoisoned = true;
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
        Debug.Log("playerHealth = " + playerHealth);
        Debug.Log("playerAudio = " + playerAudio);
        float t = 0;
        while (t < duration)
        {
            if (playerHealth.isImmune)
            {
                StopPoison();
                yield break;
            }
            playerAudio.PlaySFXClip(playerAudio.hurtClip);
            playerHealth.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickTime);
            t+=tickTime;
        }
        playerHealth.isPoisoned = false;
        poisonCo = null;
    }
}
