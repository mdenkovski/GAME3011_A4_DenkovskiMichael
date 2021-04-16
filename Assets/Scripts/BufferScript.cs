using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BufferScript : MonoBehaviour
{
    [SerializeField]
    private GameObject BufferSelections;

    [SerializeField]
    private GameObject BufferSlotPrefab;

    private List<GameObject> BufferSlots;

    private void OnEnable()
    {
        BufferSlots = new List<GameObject>();
        
    }

    public void Initialize(int NumBuffers)
    {
        for (int i = 0; i < NumBuffers; i++)
        {
            BufferSlots.Add(Instantiate(BufferSlotPrefab, BufferSelections.transform));
        }
    }

    public void SetElementContent(int element, string content)
    {
        BufferSlots[element].GetComponentInChildren<TMP_Text>().text = content;
    }

    private void OnDisable()
    {
        ClearBuffer();
    }

    public void ClearBuffer()
    {
        foreach (GameObject slot in BufferSlots)
        {
            Destroy(slot);
        }
    }
}
