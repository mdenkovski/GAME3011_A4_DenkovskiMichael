using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetSequenceScript : MonoBehaviour
{
    [SerializeField]
    private GameObject SlotPrefab;

    private List<GameObject> TargetSlots = new List<GameObject>();



    public void Initialize(List<string> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            GameObject TargetSlot = Instantiate(SlotPrefab, transform);
            TargetSlot.GetComponentInChildren<TMP_Text>().text = sequence[i];
            TargetSlots.Add(TargetSlot);

        }
    }


    private void OnDisable()
    {
        ClearBuffer();
    }

    public void ClearBuffer()
    {
        foreach (GameObject slot in TargetSlots)
        {
            Destroy(slot);
        }
        TargetSlots.Clear();
    }
}
