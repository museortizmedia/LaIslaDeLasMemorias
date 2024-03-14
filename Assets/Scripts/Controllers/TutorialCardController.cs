using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TutorialCardController : MonoBehaviour
{

    public InteractableArea interactable;
    public TextMeshProUGUI ContentBox;
    public GameObject selectedButton;
    public List<GameObject> buttons = new List<GameObject>();

    public void ShowInfo(int index, string content)
    {
        ResetInfo();

        ContentBox.text = content;
        selectedButton = buttons[index];
        interactable.ButonIdAcepted[0] = index + 1;
        selectedButton.SetActive(true);
    }

    public void ResetInfo()
    {
        interactable.ButonIdAcepted[0] = 0;
        selectedButton?.SetActive(false);
        interactable.VotesCount = 0;
        ContentBox.text = "";
    }
}
