using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetEditor : Editor
{
    private PlanetGenerator _planetGenerator;
    private Editor _shapeEditor;
    private Editor _colourEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planetGenerator.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
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

   private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor ,ref bool foldout)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

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
}
