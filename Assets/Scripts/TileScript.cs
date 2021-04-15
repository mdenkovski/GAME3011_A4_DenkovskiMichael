using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TextLabel;
    private string content;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
