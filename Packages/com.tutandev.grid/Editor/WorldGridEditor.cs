using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGrid))]
public class WorldGridEditor : Editor
{
    WorldGrid grid;

    SerializedProperty IsSquareProp;
    SerializedProperty horizontalResolutionProp;
    SerializedProperty verticalResolutionProp;

    SerializedProperty IsCenteredProp;
    SerializedProperty DefineUsingGapsProp;
    SerializedProperty dimensionsProp;
    SerializedProperty horizontalGapLengthProp;
    SerializedProperty verticalGapLengthProp;

    SerializedProperty drawGizmosProp;
    SerializedProperty gizmoColorProp;
    SerializedProperty gizmoSizeProp;
    SerializedProperty showCoordsProp;

    readonly GUIContent drawGizmosContent = EditorGUIUtility.TrTextContent("Draw Gizmos", "Draw spheres in the grid points.");
    readonly GUIContent gizmoColorContent = EditorGUIUtility.TrTextContent("Gizmos Color", "Color used to draw the gizmos.");
    readonly GUIContent gizmoSizeContent = EditorGUIUtility.TrTextContent("Gizmos Size", "Radious used for the Gizmo spheres.");
    readonly GUIContent showCoordsContent = EditorGUIUtility.TrTextContent("Show Coordinates", "Shows the internal indexes of the grid points.");

    private FontStyle _cachedFontStyle;
    private bool showDebug;
    const int labelWidth = 120;

    void OnEnable() 
    {
        grid = (WorldGrid)target;

        IsSquareProp = serializedObject.FindProperty("IsSquare");
        horizontalResolutionProp = serializedObject.FindProperty("horizontalResolution");
        verticalResolutionProp = serializedObject.FindProperty("verticalResolution");

        DefineUsingGapsProp = serializedObject.FindProperty("DefineUsingGaps");
        IsCenteredProp = serializedObject.FindProperty("IsCentered");
        dimensionsProp = serializedObject.FindProperty("dimensions");
        horizontalGapLengthProp = serializedObject.FindProperty("horizontalGapLength");
        verticalGapLengthProp = serializedObject.FindProperty("verticalGapLength");

        drawGizmosProp = serializedObject.FindProperty("drawGizmos");
        gizmoColorProp = serializedObject.FindProperty("gizmoColor");
        gizmoSizeProp = serializedObject.FindProperty("gizmoSize");
        showCoordsProp = serializedObject.FindProperty("showCoords");
    }


    public override void OnInspectorGUI()
    {
        _cachedFontStyle = EditorStyles.label.fontStyle;

        //DrawDefaultInspector();
        serializedObject.Update();

        DrawField("Is SQUARE", IsSquareProp);
        EditorGUILayout.Space();

        if (IsSquareProp.boolValue)
        {
            DrawSquaredSettings();
        }
        else
        {
            DrawSettings();
        }


        EditorGUILayout.Space();
        DrawDebug();

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (!showCoordsProp.boolValue) return;

        foreach (var pos in grid)
        {
            Handles.Label(pos, grid.GetCoords(pos).ToString());
        }
    }


    private void DrawSquaredSettings()
    {
        DrawField("Resolution", horizontalResolutionProp);
        DrawField("Center Grid", IsCenteredProp);

        DrawField("Use Gap distance", DefineUsingGapsProp);
        if (DefineUsingGapsProp.boolValue)
        {
            DrawField("Gap Distance", horizontalGapLengthProp);
        }
        else
        {
            DrawField("Dimensions", dimensionsProp);
        }
    }

    private void DrawSettings()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Resolution", GUILayout.Width(labelWidth));
        EditorGUILayout.PropertyField(horizontalResolutionProp, GUIContent.none);
        EditorGUILayout.PropertyField(verticalResolutionProp, GUIContent.none);
        EditorGUILayout.EndHorizontal();

        DrawField("Center Grid", IsCenteredProp);
        DrawField("Use Gap distance", DefineUsingGapsProp);

        if (DefineUsingGapsProp.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Gap Distance", GUILayout.Width(labelWidth));
            EditorGUILayout.PropertyField(horizontalGapLengthProp, GUIContent.none);
            EditorGUILayout.PropertyField(verticalGapLengthProp, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            DrawField("Dimensions", dimensionsProp);
        }
    }

    private void DrawDebug()
    {
        EditorGUILayout.BeginVertical("Box");
        CreateToggleHEader("DEBUG", ref showDebug);

        if (showDebug)
        {
            EditorGUILayout.PropertyField(drawGizmosProp, drawGizmosContent);
            if(drawGizmosProp.boolValue == true)
            {
                EditorGUILayout.PropertyField(gizmoColorProp, gizmoColorContent);
                EditorGUILayout.PropertyField(gizmoSizeProp, gizmoSizeContent);
                EditorGUILayout.PropertyField(showCoordsProp, showCoordsContent);
            }
        }

        GUILayout.EndVertical();
    }


    void DrawField(string label, SerializedProperty prop)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
        EditorGUILayout.PropertyField(prop, GUIContent.none);
        EditorGUILayout.EndHorizontal();
    }
    void CreateToggleHEader(string s, ref bool b)
    {
        EditorStyles.toolbarButton.fontStyle = FontStyle.Bold;

        if (GUILayout.Button(s, EditorStyles.popup))
        {
            b = !b;
        }

        EditorStyles.toolbarButton.fontStyle = _cachedFontStyle;
    }
}
