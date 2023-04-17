using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(UIWindowsManager))]
public class UIWindowsManagerEditor : Editor
{
    
    [MenuItem("GameObject/WindowsManager/Create Manager")]
    private static void CreateManager()
    {
        var duplicated = GameObject.FindObjectOfType<UIWindowsManager>();
        if (duplicated != null)
        {
            Debug.Log("Windows manager is ready");
            return;
        }
        // Get the current selection in the scene hierarchy
        GameObject parentObject = Selection.activeGameObject;

        if (parentObject != null)
        {
            var canvas = parentObject.transform.GetComponentsInParent<Transform>().FirstOrDefault(x=>x.GetComponent<Canvas>()!=null);
            if (canvas != null)
            {
                // Create a new game object as a child of the selected object
                GameObject childObject = new GameObject("WindowsManager",typeof(RectTransform),typeof(UIWindowsManager));
                childObject.transform.SetParent(parentObject.transform);
                Selection.activeGameObject = childObject;
                
            }
            else
            {
                Debug.Log("Canvas not selected");
            }
        }
        else
        {
            Debug.Log("Canvas not selected");
        }
      
    }
    
    [MenuItem("GameObject/WindowsManager/Create window item")]
    private static void CreateWindowMenuItem()
    {
        var manager = GameObject.FindObjectOfType<UIWindowsManager>();
        if (manager != null)
        {
            var prefab = Resources.Load<WindowItem>("window item").gameObject;
            GameObject childObject = Instantiate(prefab,manager.transform);
            childObject.name = "window item";
            Selection.activeGameObject = childObject;
        }
        else
        {
            Debug.Log("Window manager is not found");
        }
      
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        var trg = (UIWindowsManager) target;
        if (GUILayout.Button("Create window"))
        {
            CreateWindowMenuItem();
        }

        var items = FindObjectsOfType<WindowItem>();
        trg.windows = items.ToList();
    }
}
