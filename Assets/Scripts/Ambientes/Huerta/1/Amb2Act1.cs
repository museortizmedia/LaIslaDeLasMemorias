using System.Collections;
using System.Collections.Generic;
using MuseCoderLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Amb2Act1 : ExperienceController
{
    public Sprite unfertilizedSprite;
    public Sprite fertilizedSprite;
    public GameObject[] allPots;
    public TextMeshProUGUI roundTextMesh;
    [SerializeField] private GameObject[] vibratingPots = new GameObject[5];
    [SerializeField] private List<GameObject> unfertilizedPots = new List<GameObject>();
    [SerializeField] private int _currentRound = 1;
    public int currentRound{
        get => _currentRound;
        set {
            _currentRound = value;
            roundTextMesh.text = _currentRound.ToString();
        }
        
    }
    [SerializeField] private int currentPotIndex = 0;

    public UnityEvent OnFailPotTutorial, OnFailFertilizantedTutorial, OnGoodTutorial, OnEndTutorial;

    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);
    }

    public void InitializeVibratingPots()
    {
        List<int> indices = new List<int>();
        while (indices.Count < 5)
        {
            int randomIndex = Random.Range(0, allPots.Length);
            if (!indices.Contains(randomIndex))
            {
                indices.Add(randomIndex);
                vibratingPots[indices.Count - 1] = allPots[randomIndex];
            }
        }
        StartRound();
    }

    public void StartRound()
    {
        currentPotIndex = 0;
        roundTextMesh.text = currentRound.ToString();
        ChangePotStatus();
        ResetPositions();
        StartCoroutine(VibrateAllPotsForRound());
    }

    public void VibratePots()
    {
        StopAllCoroutines();
        foreach (var pot in allPots)
        {
            pot.GetComponent<VibrateImage>().StopVibration();
        }
        StartCoroutine(VibrateAllPotsForRound());
    }

    IEnumerator VibrateAllPotsForRound()
    {
        for (int i = 0; i < currentRound; i++)
        {
            if (vibratingPots[i] != null)
            {
                VibrateImage macetaComponent = vibratingPots[i].GetComponent<VibrateImage>();
                if (macetaComponent != null)
                {
                    macetaComponent.Vibrar();
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    void ChangePotStatus()
    {
        foreach (GameObject pot in vibratingPots)
        {
            if (pot != null && pot.GetComponent<Image>().sprite != unfertilizedSprite && Random.Range(0, 100) < 10)
            {
                pot.GetComponent<Maceta>().ChangeSprite(unfertilizedSprite);
                unfertilizedPots.Add(pot);
            }
        }
    }

    public void UserSelectedPot(GameObject selectedPot)
    {
        // Verificar si hay macetas sin abono
        if (unfertilizedPots.Count > 0)
        {
            OnFailFertilizantedTutorial?.Invoke();
            return;
        }

        // Comprobar si la maceta seleccionada es la correcta en la secuencia
        if (selectedPot == vibratingPots[currentPotIndex])
        {
            selectedPot.GetComponent<Maceta>().ChangePosition((currentPotIndex + 1).ToString());
            currentPotIndex++;

            if (currentPotIndex == currentRound)
            {
                if (currentRound < 5)
                {
                    currentRound++;
                    OnGoodTutorial?.Invoke();
                }
                else
                {
                    OnEndTutorial?.Invoke();
                }
            }
            
        }
        else
        {
            OnFailPotTutorial?.Invoke();
        }
    }

    public void FertilizeAllUnfertilizedPots()
    {
        Debug.Log(unfertilizedPots.Count);
        foreach (GameObject pot in unfertilizedPots)
        {
            if (pot != null)
            {
                pot.GetComponent<Maceta>().ChangeSprite(fertilizedSprite);
            }
        }
        unfertilizedPots.Clear();
    }

    public void ResetPositions()
    {
        foreach (GameObject pot in allPots)
        {
            if (pot != null)
            {
                pot.GetComponent<Maceta>().ChangePosition("");
            }
        }
    }

    public void Finalizar()
    {
        EndExperience();
    }
}
