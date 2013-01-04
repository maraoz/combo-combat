using UnityEngine;
using System;
using System.Collections.Generic;

public static class GUIX {
    public static Vector2 BeginScrollView(
        Rect position,
        Vector2 scrollPosition,
        Rect contentRect,
        bool useHorizontal,
        bool useVertical,
        GUIStyle hStyle,
        GUIStyle vStyle) {

        Vector2 scrollbarSize = new Vector2(hStyle.CalcSize(GUIContent.none).y, vStyle.CalcSize(GUIContent.none).x);
        Rect viewArea = position;
        if (useHorizontal)
            viewArea.height -= scrollbarSize.x;
        if (useVertical)
            viewArea.width -= scrollbarSize.y;
        if (useHorizontal) {
            Rect hScrRect = new Rect(position.x, position.y + viewArea.height, viewArea.width, scrollbarSize.x);
            scrollPosition.x = GUI.HorizontalScrollbar(hScrRect, scrollPosition.x, viewArea.width, 0, contentRect.width);
        }
        if (useVertical) {
            Rect vScrRect = new Rect(position.x + viewArea.width, position.y, scrollbarSize.y, viewArea.height);
            scrollPosition.y = GUI.VerticalScrollbar(vScrRect, scrollPosition.y, viewArea.height, 0, contentRect.height);
        }
        GUI.BeginGroup(viewArea);
        contentRect.x = -scrollPosition.x;
        contentRect.y = -scrollPosition.y;
        GUI.BeginGroup(contentRect);
        return scrollPosition;
    }

    public static Vector2 BeginScrollView(
        Rect position,
        Vector2 scrollPosition,
        Rect contentRect,
        bool useHorizontal,
        bool useVertical) {
        return BeginScrollView(position, scrollPosition, contentRect, useHorizontal, useVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
    }

    public static void EndScrollView() {
        GUI.EndGroup();
        GUI.EndGroup();
    }
}