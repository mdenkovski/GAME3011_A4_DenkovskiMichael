using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TextLabel;
    private string content;

    private HackingPanel HackingPanel;

    private Vector2 GridLocation;

    public void Initialize(string Text, int xLocation, int yLocation, HackingPanel panel)
    {
        SetContent(Text);
        HackingPanel = panel;
        GridLocation = new Vector2(xLocation, yLocation);

    }

    public bool HasContent()
    {
        return content != null;
    }

    public void OnMouseDown()
    {
        if (!HackingPanel.IsValidTileSelection(GridLocation)) return;
        if (content == "XX") return;

        HackingPanel.TileSelected(content, GridLocation);
        SetContent("XX");
    }

    private void SetContent(string Text)
    {
        content = Text;
        TextLabel.text = content;
    }
}
