using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupClip;
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
            GameManager gm = FindAnyObjectByType<GameManager>();gm.AddScore(1);
            audioSource.PlayOneShot(pickupClip);
            StartCoroutine(DestroyCoin());
        }
    }
}
