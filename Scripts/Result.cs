using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        if (titles != null && titles.Length > 0)
            titles[0].SetActive(true);
    }

    public void Win()
    {
        if (titles != null && titles.Length > 1)
            titles[1].SetActive(true);
    }
}
