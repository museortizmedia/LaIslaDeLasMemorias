using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Maceta : MonoBehaviour
{
    public void ChangeSprite(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }

    public void ChangePosition(string value)
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value;
    }
}
