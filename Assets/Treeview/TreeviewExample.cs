using UnityEngine;
using UnityEngine.UI;

public class TreeviewExample : MonoBehaviour
{
    private const string treeviewComponentNotFound = "Treeview component not found.";
    private const string treeviewDisplayingByEditorDisabled = "Treeview displaying has been disabled in component \"Treeview\".";
    private Treeview treeview;
    public Text Log;

    private void Awake()
    {
        if (!gameObject.TryGetComponent<Treeview>(out treeview))
        {
            Debug.LogError(treeviewComponentNotFound);
            return;
        }

        if (Log != null)
        {
            treeview.Root.SelectHandler = new Node.NodeEventHandler((s, e) => Log.text = $"Selected: {{Id: {e.Node.Id}, Text: \"{e.Node.Text}\"}}");
            // TODO: treeview.Root.*Handler = ...
        }

        treeview.Root
            .AddChild("Lorem ipsum")
            .AddChild("Dolor sit amet", ReturnedNode.Created)
                .AddChild("Velit esse")
                .AddChild("Cillum")
                .AddChild("Fugiat nulla", ReturnedNode.Root)
            .AddChild("Consectetur")
            .AddChild("Adipiscing", ReturnedNode.Created)
                .AddChild("Excepteur", ReturnedNode.Created)
                    .AddChild("Cupidatat")
                    .AddChild("Non proident").Parent
                .AddChild("Sint occaecat")
            .AddChild("Ut aliquip", ReturnedNode.Root)
            .AddChild("Duis aute")
            .AddChild("Sunt in culpa")
            .AddChild("Qui officia")
            ;
    }
    
    private void OnGUI()
    {
        treeview.SaveDefaultButtonStyle();

        if (treeview == null)
        {
            Debug.LogError(treeviewComponentNotFound);
            return;
        }

        if (treeview.DisplayInGame)
        {
            Debug.Log(treeviewDisplayingByEditorDisabled);
            treeview.DisplayInGame = false;
        }

        treeview.X = (Screen.width - treeview.Width) / 2;

        GUILayout.BeginArea(treeview.BackgroundRect);

        treeview.Display();

        GUILayout.EndArea();
    }
}
