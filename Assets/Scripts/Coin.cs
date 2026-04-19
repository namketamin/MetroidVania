using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    IEnumerator DestroyCoin()
    {
        animator.SetTrigger("isDisappear");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();gm.AddScore(1);
            StartCoroutine(DestroyCoin());
        }
    }
}
