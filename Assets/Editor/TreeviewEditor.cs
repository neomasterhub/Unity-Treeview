using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Treeview))]
public class TreeviewEditor : Editor
{
    /// <summary>
    /// Добавляет в заголовок Unity меню "Neomaster" с пунктом "Add Treeview".
    /// </summary>
    [MenuItem("Neomaster/Add Treeview")]
    public static void AddTreeview()
    {
        GameObject go = Selection.activeGameObject;

        if (!(go is GameObject))
        {
            Debug.LogError("No game object selected.");
            return;
        }

        Treeview treeview = go.AddComponent<Treeview>();
        // ...
    }

    /// <summary>
    /// Рисует древо в окне Scene.<br/>
    /// Не рисует во время игры.
    /// </summary>
    private void OnSceneGUI()
    {
        Treeview tv = (Treeview)target;
        tv.SaveDefaultButtonStyle();

        if (!Application.isPlaying && tv.DisplayInScene)
        {
            GUILayout.BeginArea(new Rect(tv.X, tv.Y, tv.Width, tv.Height));

            tv.Display();

            GUILayout.EndArea();
        }
    }

    /// <summary>
    /// Создает кнопки интерфейса в Inspector.<br/>
    /// Перерисовывает древо после изменения.<br/>
    /// Кнопки перерисовываются после наведения мыши на Inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        Treeview tv = (Treeview)target;
        tv.SaveDefaultButtonStyle();

        GUILayout.BeginVertical();

        bool changed = tv.InspectorButton_AddChild("Add child")
            || tv.InspectorButton_Clear("Clear")
            || tv.InspectorButton_Delete("Delete")
            || tv.InspectorButton_MoveUp("Move Up")
            || tv.InspectorButton_MoveDown("Move Down")
            || SelectedNodeEditor(tv);

        GUILayout.EndVertical();

        base.OnInspectorGUI();

        if (tv.DisplayInInspector)
        {
            tv.Display();
        }

        if (changed /* && mouse enter */)
        {
            Repaint();
            SceneView.RepaintAll();
        }
    }

    /// <summary>
    /// Создает элементы интерфейса выбранного узла и применяет их новые значения.
    /// </summary>
    /// <returns>true, если выбранный узел изменен.</returns>
    public bool SelectedNodeEditor(Treeview treeview)
    {
        TreeviewHelper.InspectorHeader("Selected Node");

        if (treeview.SelectedNode == null)
        {
            GUILayout.Label("null", GUI.skin.label.TunedCopy());
            return false;
        }

        bool result = false;

        #region Controls
        GUIStyle textFieldStyle = GUI.skin.textField.TunedCopy();

        // п/п

        string text = EditorGUILayout.TextField("Text", treeview.SelectedNode.Text, textFieldStyle);

        EditorGUILayout.PrefixLabel("Description", GUI.skin.label.TunedCopy());
        string description = EditorGUILayout.TextArea(treeview.SelectedNode.Description, GUI.skin.textArea.TunedCopy(), GUILayout.Height(100));

        float width = EditorGUILayout.FloatField("Width", treeview.SelectedNode.Width, textFieldStyle);

        float hright = EditorGUILayout.FloatField("Height", treeview.SelectedNode.Height, textFieldStyle);

        bool sizeApplied = EditorGUILayout.Toggle("Apply Individual Size", treeview.SelectedNode.SizeApplied, GUI.skin.toggle.TunedCopy());
        #endregion

        #region Changes
        if (treeview.SelectedNode.Text != text)
        {
            result = true;

            if (text.Length > Node.TextMaxLength)
            {
                treeview.SelectedNode.Text = text.Substring(0, Node.TextMaxLength);
            }
            else if (text.Length >= Node.TextMinLength)
            {
                treeview.SelectedNode.Text = text;
            }
        }

        if (treeview.SelectedNode.Description != description)
        {
            result = true;

            if (description.Length > Node.DescriptionMaxLength)
            {
                treeview.SelectedNode.Description = description.Substring(0, Node.TextMaxLength);
            }
            else
            {
                treeview.SelectedNode.Description = description;
            }
        }

        if (sizeApplied != treeview.SelectedNode.SizeApplied)
        {
            treeview.SelectedNode.SizeApplied = sizeApplied;
            result = true;
        }

        if (treeview.SelectedNode.Width != width)
        {
            treeview.SelectedNode.Width = width;
            result = true;
        }

        if (treeview.SelectedNode.Height != hright)
        {
            treeview.SelectedNode.Height = hright;
            result = true;
        }
        #endregion

        return result;
    }
}
