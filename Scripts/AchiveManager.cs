using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject notice;
    public Text noticeText;

    enum Achive { UnlockSecondCharacter, UnlockThirdCharacter }

    Achive[] achives;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("UndeadSurvivorData"))
            Init();

        wait = new WaitForSecondsRealtime(3f);
    }

    void Start()
    {
        UnlockCharacter();
    }

    void LateUpdate()
    {
        if (GameManager.instance == null)
            return;

        foreach (Achive achive in achives)
            CheckAchive(achive);
    }

    void Init()
    {
        PlayerPrefs.SetInt("UndeadSurvivorData", 1);

        foreach (Achive achive in achives)
            PlayerPrefs.SetInt(achive.ToString(), 0);
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockSecondCharacter:
                isAchive = GameManager.instance.kill >= 100;
                break;
            case Achive.UnlockThirdCharacter:
                isAchive = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
            StartCoroutine(NoticeRoutine(achive));
        }
    }

    IEnumerator NoticeRoutine(Achive achive)
    {
        if (noticeText != null)
            noticeText.text = achive.ToString();

        if (notice != null)
            notice.SetActive(true);

        if (AudioManager.instance != null)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        if (notice != null)
            notice.SetActive(false);

        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < achives.Length; index++)
        {
            bool unlocked = PlayerPrefs.GetInt(achives[index].ToString()) == 1;

            if (lockCharacter != null && index < lockCharacter.Length && lockCharacter[index] != null)
                lockCharacter[index].SetActive(!unlocked);

            if (unlockCharacter != null && index < unlockCharacter.Length && unlockCharacter[index] != null)
                unlockCharacter[index].SetActive(unlocked);
        }
    }
}
