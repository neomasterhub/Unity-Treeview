using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Treeview : MonoBehaviour//, ISerializationCallbackReceiver
{
    /// <summary>
    /// Префикс имен сущностей в контексте древа.
    /// </summary>
    public const string NamePrefix = "Treeview";

    #region Inspector buttons default style 
    private GUIStyle defaultButtonStyle;
    public GUIStyle DefaultButtonStyle
    {
        get
        {
            if (defaultButtonStyle == null)
            {
                throw new Exception("DefaultButtonStyle must be set by calling SaveDefaultButtonStyle() in On...GUI(), not within Treeview.cs.");
            }

            return defaultButtonStyle;
        }
    }
    public bool SavedDefaultButtonStyle => defaultButtonStyle != null;

    /// <summary>
    /// Сохраняет стиль по умолчанию для кнопок инспектора.<br/>
    /// Вызывается только в On...GUI().
    /// </summary>
    public void SaveDefaultButtonStyle()
    {
        if (defaultButtonStyle == null)
        {
            defaultButtonStyle = GUI.skin.button.TunedCopy();
        }
    }
    #endregion

    #region Background
    private Texture2D background;
    private Texture2D defaultBackground;
    private GUIStyle backgroundStyle;
    private Rect backgroundRect = new Rect();
    public Texture2D Background
    {
        get
        {
            if (background == null)
            {
                background = new Texture2D(1, 1);
                background.SetName("Background");
            }

            background.SetPixel(1, 1, BackgroundColor);
            background.Apply();

            return background;
        }
    }
    public Texture2D DefaultBackground
    {
        get
        {
            if (defaultBackground == null)
            {
                defaultBackground = new Texture2D(1, 1, TextureFormat.Alpha8, false);
                defaultBackground.SetPixel(1, 1, new Color32(0, 0, 0, 0));
                defaultBackground.Apply();
                defaultBackground.SetName("DefaultBackground");
            }

            return defaultBackground;
        }
    }
    public GUIStyle BackgroundStyle
    {
        get
        {
            if (backgroundStyle == null)
            {
                backgroundStyle = new GUIStyle();
            }

            backgroundStyle.padding = Padding;
            backgroundStyle.normal.background = Background;

            return backgroundStyle;
        }
    }
    public Rect BackgroundRect
    {
        get
        {
            backgroundRect.x = X;
            backgroundRect.y = Y;
            backgroundRect.width = Width;
            backgroundRect.height = Height;

            return backgroundRect;
        }
    }

    public void DestroyTextures()
    {
        Destroy(background);
        Destroy(defaultBackground);

        Debug.Log("Treeview textures destroyed.");
    }
    #endregion

    #region Node
    private Node root;
    public Node Root
    {
        get
        {
            if (root == null)
            {
                root = new Node("Root", this);
            }

            return root;
        }
    }
    public Node SelectedNode;
    #endregion

    #region Controls
    [Header("Display")]
    public bool DisplayInInspector = false;
    public bool DisplayInGame = true;
    public bool DisplayInScene = true;

    [Header("Treeview")]
    public int X = 10;
    public int Y = 10;
    public int Width = 450;
    public int Height = 350;
    public RectOffset Padding = new RectOffset();
    public int LevelDistance = 0;
    public Color BackgroundColor = new Color(0, 0, 0, 0.65f);

    [Header("Any Node")]
    public RectOffset NodeMargin = new RectOffset();
    public RectOffset NodePadding = new RectOffset();
    public Font NodeFont;
    public int NodeFontSize = 24;
    public TextAnchor NodeTextAnchor = TextAnchor.MiddleLeft;

    [Header("Nodes")]
    public ENodeView NormalENodeView = new ENodeView(new Color(0.8f, 0.8f, 0.8f), Color.white, Color.red);
    public ENodeView SelectedENodeView = new ENodeView(Color.yellow, Color.white, Color.red);

    [Header("Glyphs")]
    public EGlyph NormalEGlyph = new EGlyph(new Color(0.8f, 0.8f, 0.8f), Color.white, Color.red);
    public EGlyph SelectedEGlyph = new EGlyph(Color.yellow, Color.white, Color.red);
    #endregion
    
    #region Serialization
    [Readonly] public int LastNodeId;
    private List<NodeData> nodeDatas = new List<NodeData>();

    private void Serialize(Node node)
    {
        if (node == null)
        {
            node = new Node("Root", this);
        }

        nodeDatas.Add(new NodeData(node));
        node.Children.ForEach(c => Serialize(c));

        Debug.Log("Treeview serialized.");
    }

    private void Deserialize(Node root)
    {
        // Controls the uniqueness of any Id.
        Dictionary<int, Node> nd = nodeDatas.ToDictionary(k => k.Id, v => new Node(v, this));

        foreach (Node node in nd.Values)
        {
            Node altParent;
            if (node.Parent != null && nd.TryGetValue(node.Parent.Id, out altParent))
            {
                node.Parent = altParent;
                altParent.Children.Add(node);
            }
        }

        root = nd.Values.First(x => x.Parent == null); // Root always exists.

        Debug.Log("Treeview deserialized.");
    }

    public void OnBeforeSerialize()
    {
        nodeDatas.Clear();
        Serialize(root);
    }

    public void OnAfterDeserialize()
    {
        Deserialize(root);
    }

    #endregion
    
    private Vector2 scrollPosition;
    public void Display()
    {
        if (NodeFont == null)
        {
            TreeviewHelper.DisplayWarningMessage("Node font not set.");
            return;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, BackgroundStyle, GUILayout.MaxWidth(Width), GUILayout.MaxHeight(Height));
        GUILayout.BeginVertical();

        Root.Display();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void OnGUI()
    {
        if (DisplayInGame && SavedDefaultButtonStyle)
        {
            GUILayout.BeginArea(BackgroundRect);

            Display();

            GUILayout.EndArea();
        }
    }

    private void OnDestroy()
    {
        // Texture2D[] ts = Resources.FindObjectsOfTypeAll<Texture2D>();
        // int tsMaxId = ts.Max(t => t.GetInstanceID());
        // int tsCount = ts.Length;

        // Debug.Log($"textures: {{ max Id: {tsMaxId}, count: {tsCount} }}");
    }

    private void OnDisable()
    {
        DestroyTextures();
    }

    private void Reset()
    {
        SelectedNode = null;
        Root?.Children.Clear();
    }
}
