using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogGraphView _graphView;
    private string _fileName;
    [MenuItem("Tools/Dialogue Graph")]
    public static void OpenDialogueWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();

        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);


        toolbar.Add(new Button(() => SaveData()) { text = "Save Data"});
        toolbar.Add(new Button(() => LoadData()) { text = "Load Data" });

        var nodeCreateButton = new Button(() =>
        {
            _graphView.CreateNode("Dialog Node");
        });

        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void SaveData()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid File","File doesnt exist","Okay");
            return;
        }
        else
        {
            var graphSaveUtility = GraphSaveUtility.GetInstance(_graphView);
            graphSaveUtility.SaveGraph(_fileName);
        }
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid File", "File doesnt exist", "Okay");
            return;
        }
        else
        {
            var graphUtility = GraphSaveUtility.GetInstance(_graphView);
            graphUtility.LoadGraph(_fileName);
        }
    }
}
