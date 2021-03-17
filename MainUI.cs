using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Transform hearts;
    public Sprite[] heartImage;

    void Awake()
    {
        DrawMaxHP();
    }

    void Update()
    {
        DrawHP();
    }

    void DrawMaxHP()
    {
        for (int i = 1; i < Game.sav.maxHp; i++)
        {
            Transform h = Instantiate(hearts.GetChild(0));
            h.SetParent(hearts, false);
            h.name = "h" + (i + 1);
        }
        DrawHP();
    }

    void DrawHP()
    {
        float hp = Game.sav.hp;
        foreach (Image img in hearts.GetComponentsInChildren<Image>())
        {
            img.sprite = heartImage[1];
        }

        for (int i = 1; i <= hp; i++)
        {
            hearts.GetChild(i - 1).GetComponent<Image>().sprite = heartImage[0];
        }

    }
}
