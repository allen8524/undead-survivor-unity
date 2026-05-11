using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;
    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();

        isLive = true;

        if (coll != null)
            coll.enabled = true;

        if (rigid != null)
            rigid.simulated = true;

        if (spriter != null)
            spriter.sortingOrder = 2;

        if (anim != null)
            anim.SetBool("Dead", false);

        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;

        if (anim != null && animCon != null && data.spriteType >= 0 && data.spriteType < animCon.Length)
            anim.runtimeAnimatorController = animCon[data.spriteType];
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || !isLive || target == null || rigid == null)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive || !isLive || target == null || spriter == null)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null)
            return;

        health -= bullet.damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            if (anim != null)
                anim.SetTrigger("Hit");

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;

            if (coll != null)
                coll.enabled = false;

            if (rigid != null)
                rigid.simulated = false;

            if (spriter != null)
                spriter.sortingOrder = 1;

            if (anim != null)
                anim.SetBool("Dead", true);

            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        if (target == null || rigid == null)
            yield break;

        Vector3 playerPos = target.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
