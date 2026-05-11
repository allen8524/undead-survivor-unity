using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.31f, -0.1f, 0);
    Vector3 rightPosReverse = new Vector3(-0.31f, -0.1f, 0);

    Vector3 leftPos = new Vector3(-0.2f, -0.38f, 0);
    Vector3 leftPosReverse = new Vector3(0.2f, -0.38f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, 35);

    void Awake()
    {
        SpriteRenderer[] renderers = GetComponentsInParent<SpriteRenderer>();
        if (renderers.Length > 1)
            player = renderers[1];
    }

    void LateUpdate()
    {
        if (player == null || spriter == null)
            return;

        bool isReverse = player.flipX;

        if (isLeft)
        {
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            transform.localRotation = Quaternion.identity;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
