using System;
using UnityEngine;

[Serializable]
public class EColor
{
    public Color Normal;
    public Color Hover;
    public Color Active;

    public EColor()
    {
    }

    public EColor(Color common)
    {
        Normal = common;
        Hover = common;
        Active = common;
    }

    public EColor(Color normal, Color hover, Color active)
    {
        Normal = normal;
        Hover = hover;
        Active = active;
    }
}
