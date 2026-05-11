using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        if (rect == null || GameManager.instance == null || !Next())
            return;

        rect.localScale = Vector3.one;
        GameManager.instance.Stop();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.instance.EffectBgm(true);
        }
    }

    public void Hide()
    {
        if (rect != null)
            rect.localScale = Vector3.zero;

        if (GameManager.instance != null)
            GameManager.instance.Resume();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.EffectBgm(false);
        }
    }

    public void Select(int index)
    {
        if (items != null && index >= 0 && index < items.Length)
            items[index].OnClick();
    }

    bool Next()
    {
        if (items == null || items.Length == 0)
            return false;

        foreach (Item item in items)
        {
            if (item != null)
                item.gameObject.SetActive(false);
        }

        List<Item> candidates = new List<Item>();
        foreach (Item item in items)
        {
            if (item != null && item.CanSelect())
                candidates.Add(item);
        }

        if (candidates.Count == 0)
            return false;

        Shuffle(candidates);

        int showCount = Mathf.Min(3, candidates.Count);
        for (int index = 0; index < showCount; index++)
            candidates[index].gameObject.SetActive(true);

        return true;
    }

    void Shuffle(List<Item> candidates)
    {
        for (int index = 0; index < candidates.Count; index++)
        {
            int randomIndex = Random.Range(index, candidates.Count);
            Item temp = candidates[index];
            candidates[index] = candidates[randomIndex];
            candidates[randomIndex] = temp;
        }
    }
}
