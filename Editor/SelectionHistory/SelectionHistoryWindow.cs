using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hextant
{
    public class SelectionHistoryWindow : EditorWindow
    {
        private GUIStyle _previousLabelStyle;
        private GUIStyle _nextLabelStyle;

        private GUIStyle PreviousLabelStyle
        {
            get
            {
                if( _previousLabelStyle == null )
                {
                    _previousLabelStyle = new GUIStyle(EditorStyles.label);
                    _previousLabelStyle.alignment = TextAnchor.MiddleLeft;
                }

                return _previousLabelStyle;
            }
        }

        private GUIStyle NextLabelStyle
        {
            get
            {
                if( _nextLabelStyle == null )
                {
                    _nextLabelStyle = new GUIStyle(EditorStyles.label);
                    _nextLabelStyle.alignment = TextAnchor.MiddleRight;
                }

                return _nextLabelStyle;
            }
        }

        [MenuItem("Window/SelectionHistory")]
        private static SelectionHistoryWindow OpenSelectionHistoryWindow()
        { 
            var window = EditorWindow.GetWindow<SelectionHistoryWindow>(false, "History");

            var minSize = window.minSize;
            minSize.y = 24;
            window.minSize = minSize;

            return window;
        }

        private void OnEnable()
        {
            SelectionHistory.instance.SelectionUpdated += Repaint;
        }

        private void OnDisable()
        {
            SelectionHistory.instance.SelectionUpdated -= Repaint;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(!this.IsBackEnabled());

            var leftArrowCode = "\u2190";
            if (GUILayout.Button(leftArrowCode, GUILayout.MinWidth(35)))
            {
                this.MoveSelectionBack();
            }

            EditorGUI.EndDisabledGroup();

            {
                var previousObjects = SelectionHistory.instance.GetPreviousObjects();
                var previousLabel = GetNameForSelectionObjects(previousObjects);
                EditorGUILayout.LabelField(
                    previousLabel,
                    PreviousLabelStyle,
                    GUILayout.MinWidth(40),
                    GUILayout.ExpandWidth(true));
            }

            GUILayout.FlexibleSpace();

            {
                var nextObjects = SelectionHistory.instance.GetNewerObjects();
                var nextLabel = GetNameForSelectionObjects(nextObjects);
                EditorGUILayout.LabelField(
                    nextLabel,
                    NextLabelStyle,
                    GUILayout.MinWidth(40),
                    GUILayout.ExpandWidth(true));
            }

            EditorGUI.BeginDisabledGroup(!this.IsForwardEnabled());

            var rightArrowCode = "\u2192";
            if (GUILayout.Button(rightArrowCode, GUILayout.MinWidth(35)))
            {
                this.MoveSelectionForward();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private bool IsBackEnabled()
        {
            return SelectionHistory.instance.HasPreviousEntries();
        }

        private void MoveSelectionBack()
        {
            if ( this.IsBackEnabled() )
            {
                SelectionHistory.instance.Back();
            }
        }

        private bool IsForwardEnabled()
        {
            return SelectionHistory.instance.HasNewerEntries();
        }

        private void MoveSelectionForward()
        {
            if ( this.IsForwardEnabled() )
            {
                SelectionHistory.instance.Forward();
            }
        }

        private string GetNameForSelectionObjects(Object[] objects)
        {
            return (objects != null && objects.Length > 0) ? objects[0].name : string.Empty;
        }
    }
}