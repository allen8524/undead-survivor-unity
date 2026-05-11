using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        if (images.Length > 1)
            icon = images[1];

        if (icon != null && data != null)
            icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        if (texts.Length >= 3)
        {
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
        }

        if (textName != null && data != null)
            textName.text = data.itemName;
    }

    void OnEnable()
    {
        if (data == null)
            return;

        if (textLevel != null)
            textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (textDesc != null && TryGetDamage(level, out float weaponDamage))
                {
                    int count = GetCount(level);
                    textDesc.text = string.Format(data.itemDesc, weaponDamage * 100, count);
                }
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (textDesc != null && TryGetDamage(level, out float gearRate))
                    textDesc.text = string.Format(data.itemDesc, gearRate * 100);
                break;
            case ItemData.ItemType.Heal:
                if (textDesc != null)
                    textDesc.text = data.itemDesc;
                break;
        }
    }

    public void OnClick()
    {
        if (data == null || !CanSelect())
            return;

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    if (weapon == null || !TryGetDamage(level, out float damageRate))
                        return;

                    float nextDamage = data.baseDamage;
                    int nextCount = GetCount(level);
                    nextDamage += data.baseDamage * damageRate;
                    weapon.LevelUp(nextDamage, nextCount);
                }

                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    if (gear == null || !TryGetDamage(level, out float nextRate))
                        return;

                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Heal:
                if (GameManager.instance != null)
                    GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if (!CanSelect())
        {
            Button button = GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }

        if (GameManager.instance != null && GameManager.instance.uiLevelUp != null)
            GameManager.instance.uiLevelUp.Hide();
    }

    public bool CanSelect()
    {
        if (data == null)
            return false;

        if (data.itemType == ItemData.ItemType.Heal)
            return true;

        return data.damages != null && data.damages.Length > 0 && level < data.damages.Length;
    }

    bool TryGetDamage(int index, out float value)
    {
        value = 0f;

        if (data == null || data.damages == null || data.damages.Length == 0)
            return false;

        int safeIndex = Mathf.Clamp(index, 0, data.damages.Length - 1);
        value = data.damages[safeIndex];
        return true;
    }

    int GetCount(int index)
    {
        if (data == null || data.counts == null || data.counts.Length == 0)
            return 0;

        int safeIndex = Mathf.Clamp(index, 0, data.counts.Length - 1);
        return data.counts[safeIndex];
    }
}
