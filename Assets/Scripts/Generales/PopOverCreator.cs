using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopOverCreator : MonoBehaviour
{
    public TextMeshProUGUI Title, Content;
    public Image Icon;


    public void CreatePopUP(string title, string content, Sprite icon){
        Title.text = title;
        Content.text = content;
        Icon.sprite = icon;
    }
}
