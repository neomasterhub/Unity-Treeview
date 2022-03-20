using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public delegate void NodeEventHandler(object sender, NodeEventArgs args);
    public const int TextMinLength = 1;
    public const int TextMaxLength = 30;
    public const int DescriptionMaxLength = 50;

    public int Id;
    public int Level;
    public string Text;
    public string Description;
    public Treeview Treeview;
    public Node Parent;
    public List<Node> Children = new List<Node>();
    public NodeEventHandler SelectHandler;
    public float Width = 100;
    public float Height = 20;
    public bool SizeApplied = false;
    public bool IsExpanded = true;
    public bool IsParent => Children.Any();
    public bool IsRoot => Parent == null;
    public bool IsSelected => this == Treeview.SelectedNode;

    #region Specialized constructors
    /// <summary>
    /// Создает узел с указанным Id в десериализации.
    /// </summary>
    private Node(int id, Treeview treeview)
    {
        Id = id;
        Treeview = treeview;
    }

    /// <summary>
    /// Создает корень с Id 1.
    /// </summary>
    public Node(string text, Treeview treeview)
    {
        Treeview = treeview;
        Id = System.Threading.Interlocked.Exchange(ref Treeview.LastNodeId, 1);
        Level = 0;
        Text = text;
    }

    /// <summary>
    /// Создает ребенка указанному родителю.
    /// </summary>
    public Node(string text, Node parent)
    {
        Treeview = parent.Treeview;
        Id = System.Threading.Interlocked.Increment(ref Treeview.LastNodeId);
        Text = text;
        Parent = parent;
        Level = parent.Level + 1;
        SelectHandler = parent.SelectHandler;
    }

    /// <summary>
    /// Проецирует экземпляр NodeData в Node с родителем.<br/>
    /// Используется для проекции соответствующих списков в десериализации,<br/>
    /// из-за чего Id следующего элемента задается полю Treeview.LastNodeId.
    /// </summary>
    public Node(NodeData nodeData, Treeview treeview)
    {
        Id = nodeData.Id;
        Level = nodeData.Level;
        Text = nodeData.Text;
        Description = nodeData.Description;
        Width = nodeData.Width;
        Height = nodeData.Height;
        SizeApplied = nodeData.SizeApplied;
        Treeview = treeview;
        Treeview.LastNodeId = System.Threading.Interlocked.Exchange(ref Treeview.LastNodeId, Id);

        if (nodeData.ParentId > 0)
        {
            Parent = new Node(nodeData.ParentId, treeview);
        }
    }
    #endregion
    
    /// <summary>
    /// Рисует узел с учетом выбранности и все его потомки рекурсивно.
    /// </summary>
    public void Display()
    {
        GUILayout.BeginHorizontal();
        
        this.GenGlyphString();
        bool selected = this.Display_IsSelected();

        GUILayout.EndHorizontal();
        GUILayout.Space(Treeview.LevelDistance);

        if (selected)
        {
            Treeview.SelectedNode = this;
            SelectHandler?.Invoke(this, new NodeEventArgs(Treeview.SelectedNode));
        }

        if (IsParent && IsExpanded)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Display();
            }
        }
    }

    public Node AddChild(string text, ReturnedNode returnedNode = ReturnedNode.Parent)
    {
        Node child = new Node(text, this);
        this.Children.Add(child);

        switch (returnedNode)
        {
            case ReturnedNode.Created:
                return child;
            case ReturnedNode.Root:
                return Treeview.Root;
            case ReturnedNode.Parent:
            default:
                return this;
        }
    }
}
