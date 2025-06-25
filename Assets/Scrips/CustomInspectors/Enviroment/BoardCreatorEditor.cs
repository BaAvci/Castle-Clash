using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        BoardCreator controller = (BoardCreator)target;


        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("Generate Enviroment"))
        {
            controller.GenerateEnviroment();
        }


        if (GUILayout.Button("Generate TileMap"))
        {
            controller.GenerateTileMap();
        }


        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Generate Board"))
        {
            controller.GenerateBoard();
        }

        if (GUILayout.Button("Clear"))
        {
            controller.Clear();
        }
    }
}
