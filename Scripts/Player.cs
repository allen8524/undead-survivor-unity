using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3f;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || rigid == null)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Character.Speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (anim != null)
            anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0 && spriter != null)
            spriter.flipX = inputVec.x < 0;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int index = 2; index < transform.childCount; index++)
                transform.GetChild(index).gameObject.SetActive(false);

            if (anim != null)
                anim.SetTrigger("Dead");

            GameManager.instance.GameOver();
        }
    }
}
