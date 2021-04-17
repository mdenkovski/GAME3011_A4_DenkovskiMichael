using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverCanvas;
    [SerializeField]
    private GameObject GameWinCanvas;

    [SerializeField]
    private Image EasyButtonImage;
    [SerializeField]
    private Image MediumButtonImage;
    [SerializeField]
    private Image HardButtonImage;

    [SerializeField]
    private AbilityPanel AbilityPanel;

    private void OnEnable()
    {
        ResetUI();
    }

    public void ResetUI()
    {
        GameOverCanvas.SetActive(false);
        GameWinCanvas.SetActive(false);
    }

    public void GameWin()
    {
        GameWinCanvas.SetActive(true);
        AbilityPanel.DisableAbility();
    }

    public void GameOver()
    {
        GameOverCanvas.SetActive(true);
        AbilityPanel.DisableAbility();

    }

    public void EasyDifficultyChosen()
    {
        EasyButtonImage.color = Color.green;
        MediumButtonImage.color = Color.white;
        HardButtonImage.color = Color.white;
        AbilityPanel.EnableAbility();
    }

    public void MediumDifficultyChosen()
    {
        EasyButtonImage.color = Color.white;
        MediumButtonImage.color = Color.green;
        HardButtonImage.color = Color.white;
        AbilityPanel.EnableAbility();
    }

    public void HardDifficultyChosen()
    {
        EasyButtonImage.color = Color.white;
        MediumButtonImage.color = Color.white;
        HardButtonImage.color = Color.green;
        AbilityPanel.EnableAbility();
    }
}
