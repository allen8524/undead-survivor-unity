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
                if (textDesc != null)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (textDesc != null)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case ItemData.ItemType.Heal:
                if (textDesc != null)
                    textDesc.text = data.itemDesc;
                break;
        }
    }

    public void OnClick()
    {
        if (data == null)
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
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];
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
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.MaxHealth;
                break;
        }

        if (level == data.damages.Length)
            GetComponent<Button>().interactable = false;

        if (GameManager.instance.uiLevelUp != null)
            GameManager.instance.uiLevelUp.Hide();
    }
}
