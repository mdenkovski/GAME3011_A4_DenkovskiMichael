using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField]
    private float ExtendTimeAmount = 5;

    [SerializeField]
    private TimerBehaviour Timer;

    [SerializeField]
    private Image ButtonImage;


    private bool IsAbilityEnabled;

    private void Awake()
    {
        DisableAbility();
    }

    public void UseExtendTimeAbility()
    {
        if (!IsAbilityEnabled) return;

        Timer.ReduceElapsedTime(ExtendTimeAmount);
        DisableAbility();
    }

    public void EnableAbility()
    {
        IsAbilityEnabled = true;
        ButtonImage.color = Color.green;
    }

    public void DisableAbility()
    {
        IsAbilityEnabled = false;
        ButtonImage.color = Color.red;
    }

}
