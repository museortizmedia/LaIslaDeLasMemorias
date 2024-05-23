using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Amb5Act1 : ExperienceController
{
    public List<Sprite> redSprites = new List<Sprite>();
    public List<Sprite> blueSprites = new List<Sprite>();
    private List<Sprite> availableRedSprites;
    private List<Sprite> availableBlueSprites;

    public GameObject completedObjectsRed;
    public GameObject completedObjectsBlue;
    public GameObject currentGameObject;
    private Sprite currentSprite;
    private string currentColor;

    public UnityEvent OnFailGroupTutorial, OnGoodGroupTutorial, OnFinishGroupTutorial;

    public override void Start()
    {
        base.Start();
        Debug.Log(Manager.GetManager<InputManager>().name);

        availableRedSprites = new List<Sprite>(redSprites);
        availableBlueSprites = new List<Sprite>(blueSprites);
    }

    public void ShowRandomSprite()
    {
        if (availableRedSprites.Count == 0 && availableBlueSprites.Count == 0)
        {
            OnFinishGroupTutorial?.Invoke();
            return;
        }

        bool useRed = (availableRedSprites.Count != 0 && (Random.Range(0, 2) == 0 || availableBlueSprites.Count == 0));
        List<Sprite> selectedList = useRed ? availableRedSprites : availableBlueSprites;
        int index = Random.Range(0, selectedList.Count);
        currentSprite = selectedList[index];
        currentGameObject.GetComponent<Image>().sprite = currentSprite;
        currentColor = useRed ? "rojo" : "azul";

        selectedList.RemoveAt(index);
    }

    public void CheckAnswer(string color)
    {
        if (color.ToLower() == currentColor)
        {
            if(availableRedSprites.Count != 0 || availableBlueSprites.Count != 0) OnGoodGroupTutorial?.Invoke();
            MarkAsCompleted(currentSprite);
            ShowRandomSprite();
        }
        else
        {
            OnFailGroupTutorial?.Invoke();
        }
    }

    void MarkAsCompleted(Sprite sprite)
    {
        GameObject completedSprite = new GameObject("CompletedSprite");
        Image renderer = completedSprite.AddComponent<Image>();
        renderer.sprite = sprite;

        if(currentColor == "rojo")
        {
            completedSprite.transform.SetParent(completedObjectsRed.transform, false);
        } else
        {
            completedSprite.transform.SetParent(completedObjectsBlue.transform, false);
        }
    }


    void Finalizar(){
        Debug.Log("Terminó la experiencia con éxito");
        EndExperience(); //llamada obligatoria al finalizar
    }
}
