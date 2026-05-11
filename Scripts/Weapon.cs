using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ItemData.ItemType type;
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        if (GameManager.instance != null)
            player = GameManager.instance.player;
    }

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isLive)
            return;

        switch (type)
        {
            case ItemData.ItemType.Melee:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case ItemData.ItemType.Range:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void Init(ItemData data)
    {
        if (data == null || GameManager.instance == null || GameManager.instance.pool == null || player == null)
            return;

        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        prefabId = -1;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        GameObject[] prefabs = GameManager.instance.pool.prefabs;
        if (prefabs != null)
        {
            for (int index = 0; index < prefabs.Length; index++)
            {
                if (data.projectile == prefabs[index])
                {
                    prefabId = index;
                    break;
                }
            }
        }

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            case ItemData.ItemType.Range:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        Hand hand = player.hands != null && player.hands.Length > 0 && data.itemType == ItemData.ItemType.Melee ? player.hands[0] : null;
        if (hand != null && hand.spriter != null && data.hand != null)
        {
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }

        type = data.itemType;
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (type == ItemData.ItemType.Melee)
            Batch();
    }

    public void ApplyGearRate(float rate)
    {
        speed = Mathf.Max(0.05f, speed * (1f - rate));
    }

    void Batch()
    {
        // 근접 무기는 플레이어 주변에 투사체를 배치해 회전 공격을 만든다.
        if (count <= 0 || GameManager.instance == null || GameManager.instance.pool == null)
            return;

        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                GameObject bulletObject = GameManager.instance.pool.Get(prefabId);
                if (bulletObject == null)
                    return;

                bullet = bulletObject.transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
                bulletComponent.Init(damage, -100, Vector3.zero);
        }
    }

    void Fire()
    {
        // 가장 가까운 적 방향으로 원거리 투사체를 발사한다.
        if (player == null || player.scanner == null || player.scanner.nearestTarget == null || GameManager.instance == null || GameManager.instance.pool == null)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        GameObject bulletObject = GameManager.instance.pool.Get(prefabId);
        if (bulletObject == null)
            return;

        Transform bullet = bulletObject.transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent == null)
        {
            bulletObject.SetActive(false);
            return;
        }

        bulletComponent.Init(damage, count, dir);

        if (AudioManager.instance != null)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
