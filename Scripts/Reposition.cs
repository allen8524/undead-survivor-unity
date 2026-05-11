using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 40);
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);
                break;
            case "Enemy":
                if (coll != null && coll.enabled)
                {
                    Vector3 dist = playerDir.normalized * 20;
                    Vector3 ran = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);
                    transform.Translate(dist + ran);
                }
                break;
        }
    }
}
