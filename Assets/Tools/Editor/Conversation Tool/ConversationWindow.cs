using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ConversationWindow : EditorWindow
{
    private ConversationGraphView graphView;

    private string fileName;

    [MenuItem("Tools/Graphs/Conversation")]
    public static void OpenWindow()
    {
        var window = GetWindow<ConversationWindow>();
        window.titleContent = new GUIContent("Conversation Graph");
    }

    private void OnEnable()
    {
        GenerateWindow();
        GenerateToolbar();
    }

    private void OnDisable()
    {
       rootVisualElement.Remove(graphView);
    }

    private void GenerateWindow()
    {
        graphView = new ConversationGraphView
        {
            name = "Conversation Graph"
        };

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var textField = new TextField("Conversation Name");
        textField.style.width = 300f;
        textField.SetValueWithoutNotify(fileName);
        textField.MarkDirtyRepaint();
        textField.RegisterValueChangedCallback(OnFileNameTextFieldUpdated);
        toolbar.Add(textField);

        var nodeCreateButton = new Button(() =>
        {
            graphView.CreateNode("Dialog Node");
        });

        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        toolbar.Add(new Button(() => SaveData()) { text = "Create Assets" });

        rootVisualElement.Add(toolbar);
    }

    private void OnFileNameTextFieldUpdated(ChangeEvent<string> evt)
    {
        fileName = evt.newValue;
    }

    private void SaveData()
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid File", "File doesnt exist", "Okay");
            return;
        }
        else
        {
            var graphSaveUtility = ConversationContainer.GetInstance(graphView);
            graphSaveUtility.SaveGraph(fileName);
        }
    }

}
