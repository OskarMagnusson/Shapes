using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;
using UnityEditor;

struct Point
{
    public Vector2 position;
    public bool highlighted;
}

[EditorTool("Shape Tool")]
public class ShapeTool : EditorTool
{
    List<Point> points = new List<Point>();
    public override void OnToolGUI(EditorWindow window)
    {
        base.OnToolGUI(window);
        Event e = Event.current;
        if (e.type == EventType.MouseDown && points.Count < 3)
        {
            Point newPoint = new Point();
            newPoint.position = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin; ;
            points.Add(newPoint);
            Debug.Log(newPoint.position);

            if (points.Count >= 3)
            {
                CalculateFourthPoint();
            }
            e.Use();
        }

        if (e.type == EventType.KeyDown)
        {
            if(e.keyCode == KeyCode.R)
            {
                points.Clear();
            }
            e.Use();
        }

        Draw();
    }

    void CalculateFourthPoint()
    {
        Point point = new Point();
        point.position = (points[2].position + points[0].position) - points[1].position;
        points.Add(point);
    }

    void Draw()
    {
        const float pointSize = 0.5f;
        foreach(Point point in points)
        {
            Vector2 line1 = point.position;
            line1.x -= pointSize;
            Vector2 line2 = point.position;
            line2.x += pointSize;
            Handles.color = Color.white;
            Handles.DrawLine(line1, line2);
            line1 = point.position;
            line1.y -= pointSize;
            line2 = point.position;
            line2.y += pointSize;
            Handles.DrawLine(line1, line2);
        }
        if(points.Count == 4)
        {
            Handles.color = Color.blue;
            for(int i = 0; i < 4; i++)
            {
                int j = (i + 1) % 4;
                Handles.DrawLine(points[i].position, points[j].position);
            }          
        }
    }

}
