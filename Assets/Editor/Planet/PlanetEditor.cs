using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetEditor : Editor
{
    #region Fields
    private PlanetGenerator _planetGenerator;
    private Editor _shapeEditor;
    private Editor _colourEditor;
    #endregion

    #region Unity
    public override void OnInspectorGUI()
    {
        // Checks if something in the GUI changed
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                if (_planetGenerator.AutoUpdate)
                    _planetGenerator.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planetGenerator.GeneratePlanet();
        }

        DrawSettingsEditor(_planetGenerator.ShapeSettings, _planetGenerator.OnShapeSettingsUpdated, ref _shapeEditor, ref _planetGenerator.ShapeSettingsFoldout);
        DrawSettingsEditor(_planetGenerator.ColourSettings, _planetGenerator.OnColourSettingsUpdated, ref _colourEditor, ref _planetGenerator.ColourSettingsFoldout);
    }

    private void OnEnable()
    {
        _planetGenerator = (PlanetGenerator)target;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Creates the custom Editor for one of our Planet settings
    /// </summary>
    /// <param name="settings">Settings to be displayed</param>
    /// <param name="onSettingsUpdated">Action that is performed, when a value in the editor changed</param>
    /// <param name="editor">Reference to the Editor</param>
    /// <param name="foldout">Is the setting unfolded</param>
    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor, ref bool foldout)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            // Checks if something in the Editor changed
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    // Saving Editor and only creating a new one if needed
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                            onSettingsUpdated.Invoke();
                    }
                }
            }
        }
    } 
    #endregion
}
