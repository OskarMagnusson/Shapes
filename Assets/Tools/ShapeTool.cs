using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;
using UnityEditor;

[EditorTool("Shape Tool")]
public class ShapeTool : EditorTool
{
    List<Vector2> points = new List<Vector2>();
    int highlighted = -1;
    const float pointSize = 0.5f;
    public override void OnToolGUI(EditorWindow window)
    {
        base.OnToolGUI(window);
        Event e = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;

        if (highlighted >= 0)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(points[highlighted], Quaternion.identity);
        
            if (EditorGUI.EndChangeCheck())
            {
                points[highlighted] = newPosition;
            }
        }

        if (e.type == EventType.MouseDown)
        {
            bool isHighlighted = false;
            for (int i = 0; i < points.Count; i++)
            {
                if ((points[i] - mousePos).magnitude < pointSize)
                {
                    highlighted = i;
                    isHighlighted = true;
                    break;
                }
            }

            if(!isHighlighted)
                highlighted = -1;

            if (points.Count < 3 && highlighted < 0)
            {
                AddNewPoint(mousePos);
            }               
            e.Use();
        }

        if (e.type == EventType.KeyDown)
        {
            if(e.keyCode == KeyCode.R)
            {
                points.Clear();
                highlighted = -1;
            }
            e.Use();
        }

        Draw();
    }

    void AddNewPoint(Vector2 newPosition)
    {
        points.Add(newPosition);
        Debug.Log(newPosition);

        if (points.Count >= 3)
        {            
            Vector2 position = (points[2] + points[0]) - points[1];
            points.Add(position);
        }
    }

    void Draw()
    {
        foreach(Vector2 point in points)
        {
            Vector2 line1 = point;
            line1.x -= pointSize;
            Vector2 line2 = point;
            line2.x += pointSize;
            Handles.color = Color.white;
            Handles.DrawLine(line1, line2);
            line1 = point;
            line1.y -= pointSize;
            line2 = point;
            line2.y += pointSize;
            Handles.DrawLine(line1, line2);
        }
        if(points.Count == 4)
        {
            Handles.color = Color.blue;
            for(int i = 0; i < 4; i++)
            {
                int j = (i + 1) % 4;
                Handles.DrawLine(points[i], points[j]);
            }

            Handles.color = Color.yellow;

            Vector2 center = (points[0] + points[2]) / 2.0f;
            float size = (points[1] - points[0]).magnitude * (points[3] - points[2]).magnitude;
            float radius = Mathf.Sqrt(size/Mathf.PI);
            Handles.DrawWireDisc(center, Vector3.forward, radius);
        }
    }

}

public class OskarAbout : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("About/Shape Tool")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        OskarAbout window = EditorWindow.GetWindow<OskarAbout>();
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("How to use", EditorStyles.boldLabel);
        GUILayout.Label("Click anywhere in the scene to add a point.\n" +
            "When three points have been added a parallelogram and a circle will appear.\n" +
            "Click on any of the points to move them.\n" +
            "Press R to reset all points.", EditorStyles.helpBox);
        GUILayout.Label("Created by Oskar Magnusson", EditorStyles.wordWrappedMiniLabel);
    }
}
