using System;
using UnityEngine;

[Serializable]
public class ENodeView
{
    public NodeViewSize Size = NodeViewSize.FitText;
    public EColor EColor;
    public ETexture2D ETexture2D;

    public ENodeView()
    {
    }

    public ENodeView(EColor eColor)
    {
        EColor = eColor;
    }

    public ENodeView(Color common)
    {
        EColor = new EColor(common);
    }

    public ENodeView(Color normal, Color hover, Color active)
    {
        EColor = new EColor(normal, hover, active);
    }
}
