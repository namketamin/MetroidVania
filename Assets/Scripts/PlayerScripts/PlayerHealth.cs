using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType
{
    Enemy,
    Spike
}
public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;

    private Animator animator;
    private PlayerController playerCtrl;
    private PlayerAudio playerAudio;

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;

    PoisonEffect poisonEffect;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private TextMeshProUGUI antidoteEffectTime;

    public bool IsDeath { get; private set; }
    public bool IsImmune { get; private set; }

    Coroutine immuneCoroutine;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCtrl = GetComponent<PlayerController>();
        playerAudio = GetComponentInChildren<PlayerAudio>();
        poisonEffect=GetComponentInChildren<PoisonEffect>();
    }
    void Start()
    {
        slider.maxValue=maxHP;
        currentHP = maxHP;
    }
    void Update()
    {
        UpdateHealthBar();
        UpdateHealthText();
        statusUI.SetActive(IsImmune);
    }

    public void TakeDamage(float dmg)
    {
        animator.SetTrigger("isHurt");
        currentHP -= dmg;
        playerAudio.PlaySFXClip(PlayerSFX.Hurt);

        if (currentHP <= 0)
        {
            animator.SetTrigger("isDeath");

        }
    }
    public void TakeDamage(float dmg,Transform attacker)
    {
        TakeDamage(dmg);
        playerCtrl.ApplyKnockbackForce(attacker);
    }
    private void UpdateHealthBar()
    {
        slider.maxValue = maxHP;
    }
    private void UpdateHealthText()
    {
        healthText.text = currentHP + "/100";
    }
    public void Death()
    {
        IsDeath = true;
        Destroy(gameObject);
    }
    public void CurePoison(float time)
    {
        if (poisonEffect.IsPoisoned)
        {
            poisonEffect.StopPoison();
        }
        if (immuneCoroutine != null) StopCoroutine(immuneCoroutine);

        immuneCoroutine= StartCoroutine(PoisonImunityRoutine(time));
        IsImmune = true;
    }
    IEnumerator PoisonImunityRoutine(float time)
    {
        IsImmune = true;
        while (time > 0)
        {
            antidoteEffectTime.text = Mathf.CeilToInt(time) + "s";
            yield return null;
            time -=Time.deltaTime;
        }
        antidoteEffectTime.text = "";
        IsImmune=false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            poisonEffect.ApplyPoison();
        }
    }
}
