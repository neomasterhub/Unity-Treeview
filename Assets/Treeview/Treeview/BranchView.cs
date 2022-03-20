using UnityEngine;

/// <summary>
/// Знак ветки древа в строке псевдографического рисунка.
/// </summary>
public class BranchView
{
    public string View;
    public GUIStyle Style;
    public GUILayoutOption[] Options;

    public BranchView(char view, GUIStyle style, GUILayoutOption[] options)
    {
        View = view.ToString();
        Style = style;
        Options = options;
    }
}
