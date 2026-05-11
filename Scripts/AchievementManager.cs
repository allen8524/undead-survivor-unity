using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject notice;
    public Text noticeText;

    enum Achievement { UnlockSecondCharacter, UnlockThirdCharacter }

    Achievement[] achievements;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achievements = (Achievement[])Enum.GetValues(typeof(Achievement));

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

        foreach (Achievement achievement in achievements)
            CheckAchievement(achievement);
    }

    void Init()
    {
        PlayerPrefs.SetInt("UndeadSurvivorData", 1);

        foreach (Achievement achievement in achievements)
            PlayerPrefs.SetInt(achievement.ToString(), 0);
    }

    void CheckAchievement(Achievement achievement)
    {
        // 코드용 업적 조건을 확인하고 화면에는 별도 안내 문구를 표시한다.
        bool isAchieved = false;

        switch (achievement)
        {
            case Achievement.UnlockSecondCharacter:
                isAchieved = GameManager.instance.kill >= 100;
                break;
            case Achievement.UnlockThirdCharacter:
                isAchieved = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        if (isAchieved && PlayerPrefs.GetInt(achievement.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achievement.ToString(), 1);
            StartCoroutine(NoticeRoutine(achievement));
        }
    }

    IEnumerator NoticeRoutine(Achievement achievement)
    {
        if (noticeText != null)
            noticeText.text = GetAchievementMessage(achievement);

        if (notice != null)
            notice.SetActive(true);

        if (AudioManager.instance != null)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        if (notice != null)
            notice.SetActive(false);

        UnlockCharacter();
    }

    string GetAchievementMessage(Achievement achievement)
    {
        switch (achievement)
        {
            case Achievement.UnlockSecondCharacter:
                return "새 캐릭터가 해금되었습니다!";
            case Achievement.UnlockThirdCharacter:
                return "생존 보상 캐릭터가 해금되었습니다!";
            default:
                return "업적을 달성했습니다!";
        }
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < achievements.Length; index++)
        {
            bool unlocked = PlayerPrefs.GetInt(achievements[index].ToString()) == 1;

            if (lockCharacter != null && index < lockCharacter.Length && lockCharacter[index] != null)
                lockCharacter[index].SetActive(!unlocked);

            if (unlockCharacter != null && index < unlockCharacter.Length && unlockCharacter[index] != null)
                unlockCharacter[index].SetActive(unlocked);
        }
    }
}
