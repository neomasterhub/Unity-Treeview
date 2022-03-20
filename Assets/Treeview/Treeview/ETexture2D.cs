using System;
using UnityEngine;

[Serializable]
public class ETexture2D
{
    public Texture2D Normal;
    public Texture2D Hover;
    public Texture2D Active;

    public ETexture2D()
    {
    }
    
    public ETexture2D(Texture2D normal, Texture2D hover, Texture2D active)
    {
        Normal = normal;
        Hover = hover;
        Active = active;
    }
}
