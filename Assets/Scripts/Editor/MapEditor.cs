using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MapEditor : EditorWindow
    {
        private Vector2 cellSize = new Vector2(1.0f, 1.0f);
        private bool paintMode = false;
        private int penMode = 0;
        private int tileIndex = 0;

        [MenuItem("Window/地圖編輯")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<MapEditor>("地圖編輯器");
        }

        private void OnGUI()
        {
            paintMode = GUILayout.Toggle(paintMode, "編輯", "Button", GUILayout.Height(60f));
            penMode = GUILayout.SelectionGrid(penMode, new[] { "繪製", "擦除" }, xCount: 2);

            // Get a list of previews, one for each of our prefabs
            List<GUIContent> tileIcons = new List<GUIContent>();
            foreach (GameObject prefab in tiles)
            {
                // Get a preview for the prefab
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                tileIcons.Add(new GUIContent(texture, tooltip: prefab.name));
            }

            // Display the grid
            tileIndex = GUILayout.SelectionGrid(tileIndex, tileIcons.ToArray(), 6);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (paintMode)
            {
                Vector2 cellCenter = GetSelectedCell();
                DisplayVisualHelp(cellCenter);

                if (Event.current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(0); // Consume the event
                }

                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    switch (penMode)
                    {
                        case 0:
                            HandleSceneViewDraw(cellCenter);
                            break;
                        case 1:
                            HandleSceneViewEarse();
                            break;
                    }
                }

                sceneView.Repaint();
            }
        }

        Vector2 GetSelectedCell()
        {
            // Get the mouse position in world space such as z = 0
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

            // Get the corresponding cell on our virtual grid
            Vector2Int cell = new Vector2Int(Mathf.RoundToInt(mousePosition.x / cellSize.x),
                Mathf.RoundToInt(mousePosition.y / cellSize.y));
            return cell * cellSize;
        }

        void DisplayVisualHelp(Vector2 cellCenter)
        {
            // Vertices of our square
            Vector3 topLeft = cellCenter + Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
            Vector3 topRight = cellCenter - Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
            Vector3 bottomLeft = cellCenter + Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
            Vector3 bottomRight = cellCenter - Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;

            // Rendering
            Handles.color = Color.green;
            Vector3[] lines =
                { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
            Handles.DrawLines(lines);
        }

        void HandleSceneViewDraw(Vector2 cellCenter)
        {
            if (tileIndex < tiles.Count)
            {
                HandleSceneViewEarse();

                // Create the prefab instance while keeping the prefab link
                GameObject prefab = tiles[tileIndex];
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                gameObject.transform.position = cellCenter;

                // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                Undo.RegisterCreatedObjectUndo(gameObject, "");
            }
        }

        void HandleSceneViewEarse()
        {
            var existTile = HandleUtility.PickGameObject(Event.current.mousePosition, true);
            if (existTile)
            {
                DestroyImmediate(existTile);
            }
        }

        [SerializeField] private List<GameObject> tiles = new List<GameObject>();
        private string _path = "Assets/Prefabs/Tiles";

        void RefreshTiles()
        {
            tiles.Clear();

            string[] prefabFiles = System.IO.Directory.GetFiles(_path, "*.prefab");
            foreach (string prefabFile in prefabFiles)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
                if (prefab.GetComponent<Tile>() != null)
                {
                    tiles.Add(prefab);
                }
            }
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
            RefreshTiles();
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
    }
}