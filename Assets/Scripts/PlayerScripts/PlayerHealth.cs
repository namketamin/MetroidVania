using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;

    private Animator anim;
    private PlayerController playerCtrl;

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;

    PoisonEffect poisonEffect;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private TextMeshProUGUI antidoteEffectTime;

    public bool isDeath;
    public bool isPoisoned;
    public bool isImmune;

    Coroutine immuneCoroutine;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerCtrl = GetComponent<PlayerController>();
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
        statusUI.SetActive(isImmune);
    }
    public void TakeDamage(float dmg)
    {
        anim.SetTrigger("isHurt");
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            anim.SetTrigger("isDeath");
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
        Destroy(gameObject);
        isDeath = true;
    }
    public void CurePoison(float time)
    {
        if (isPoisoned)
        {
            isPoisoned = false;
            poisonEffect.StopPoison();
        }
        if (immuneCoroutine != null) StopCoroutine(immuneCoroutine);

        immuneCoroutine= StartCoroutine(PoisonImunityRoutine(time));
        isImmune = true;
    }
    IEnumerator PoisonImunityRoutine(float time)
    {
        isImmune = true;
        while (time > 0)
        {
            antidoteEffectTime.text = Mathf.CeilToInt(time) + "s";
            yield return null;
            time -=Time.deltaTime;
        }
        antidoteEffectTime.text = "";
        isImmune=false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            poisonEffect.ApplyPoison();
        }
    }
}
