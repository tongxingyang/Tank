using System.Collections;
using System.Collections.Generic;

using Assets.Editor.CampaignMapEditor;

using UnityEditor;

using UnityEngine;

public class NodeConnection
{

    public CampaignNode source;

    public CampaignNode target;

    protected Rect areaRect = new Rect(0, 0, 50, 10);

    Color connectionColor = new Color(0.5f, 0.5f, 0.8f, 0.8f);

    float lineSize = 2;

    Vector3 lineFromTangent = Vector3.zero;
    Vector3 lineToTangent = Vector3.zero;

    //Draw them
    public void DrawConnectionGUI(Vector3 lineFrom, Vector3 lineTo)
    {

        var tangentX = Mathf.Abs(lineFrom.x - lineTo.x);
        var tangentY = Mathf.Abs(lineFrom.y - lineTo.y);

        GUI.color = connectionColor;
        var arrowRect = new Rect(0, 0, 20, 20);
        arrowRect.center = lineTo;

        if (lineFrom.x <= source.NodeRect.x)
            lineFromTangent = new Vector3(-tangentX, 0, 0);

        if (lineFrom.x >= source.NodeRect.xMax)
            lineFromTangent = new Vector3(tangentX, 0, 0);

        if (lineFrom.y <= source.NodeRect.y)
            lineFromTangent = new Vector3(0, -tangentY, 0);

        if (lineFrom.y >= source.NodeRect.yMax)
            lineFromTangent = new Vector3(0, tangentY, 0);

        if (lineTo.x <= target.NodeRect.x)
        {
            lineToTangent = new Vector3(-tangentX, 0, 0);
            GUI.Box(arrowRect, "", "nodeInputLeft");
        }

        if (lineTo.x >= target.NodeRect.xMax)
        {
            lineToTangent = new Vector3(tangentX, 0, 0);
            GUI.Box(arrowRect, "", "nodeInputRight");
        }

        if (lineTo.y <= target.NodeRect.y)
        {
            lineToTangent = new Vector3(0, -tangentY, 0);
            GUI.Box(arrowRect, "", "nodeInputTop");
        }

        if (lineTo.y >= target.NodeRect.yMax)
        {
            lineToTangent = new Vector3(0, tangentY, 0);
            GUI.Box(arrowRect, "", "nodeInputBottom");
        }

        GUI.color = Color.white;



        Event e = Event.current;

        Rect outPortRect = new Rect(0, 0, 12, 12);
        outPortRect.center = lineFrom;


        //On click select this connection
        if ((e.type == EventType.MouseDown && e.button == 0) && (areaRect.Contains(e.mousePosition) || outPortRect.Contains(e.mousePosition)))
        {
            CampaignMapEditorWin.CurrentSelection = this;
            e.Use();
            return;
        }

        //with delete key, remove connection
        if (CampaignMapEditorWin.CurrentSelection == this && e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
        {
            this.source.RemoveConnection(this);
            e.Use();
            return;
        }
        Handles.DrawBezier(lineFrom, lineTo, lineFrom + lineFromTangent, lineTo + lineToTangent, connectionColor, null, lineSize);

    }

}
