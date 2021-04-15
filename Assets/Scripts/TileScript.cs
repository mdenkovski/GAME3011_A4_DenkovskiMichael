using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TextLabel;
    private string content;


    public void Initialize(string Text)
    {
        content = Text;
        TextLabel.text = content;

    }

    public bool HasContent()
    {
        return content != null;
    }
}
