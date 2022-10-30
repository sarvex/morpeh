using System;
using System.Collections.Generic;

#if UNITY_EDITOR && TRI_INSPECTOR
namespace Morpeh.Unity.Utils.Editor {
    using TriInspector;
    using UnityEditor;
    using UnityEngine;

    internal class EditorReference : IDisposable {
        private GameObject gameObject;
        private Editor editor;
        
        public Vector2 scrollPos;

        public EditorReference(GameObject gameObject, Editor editor) {
            this.gameObject = gameObject;
            this.editor = editor;
        }

        public void HandleInspectorGUI() {
            this.editor.OnInspectorGUI();
        }

        public void Dispose() {
            Object.DestroyImmediate(this.gameObject);
            this.gameObject = null;
            
            Object.DestroyImmediate(this.editor);
            this.editor = null;
        }
    }

    public class WorldBrowser : EditorWindow {
        private List<EditorReference> references;
        
        [MenuItem("Tools/Morpeh/World Browser")]
        private static void OpenWindow() => GetWindow<WorldBrowser>().Show();

        private void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.EnteredPlayMode) {
                this.Initialize();
            }

            if (state == PlayModeStateChange.ExitingPlayMode) {
                this.Flush();
            }
        }

        private void Initialize()
        {
            this.references = new List<EditorReference>();
            
            foreach (var world in World.worlds) {
                var gameObject = new GameObject("MORPEH__WORLD_VIEWER") {hideFlags = HideFlags.HideAndDontSave};
                DontDestroyOnLoad(gameObject);
                var viewer = gameObject.AddComponent<WorldViewer>();
                viewer.World = world;
                var editor = Editor.CreateEditor(viewer);
                this.references.Add(new EditorReference(gameObject, editor));
            }
        }

        private void Flush() {
            foreach (var reference in this.references) {
                reference.Dispose();
            }

            this.references = null;
        }
        
        private void OnEnable() {
            this.titleContent = new GUIContent("World Browser",
                Resources.Load<Texture>("MorpehIcons/64x64_W"),
                "Entities Browser");
            
            EditorApplication.playModeStateChanged += this.EditorApplicationOnplayModeStateChanged;
            if (EditorApplication.isPlaying) {
                this.Initialize();
            }
        }

        private void OnGUI()
        {
            if (this.references == null) {
                return;
            }

            foreach (var reference in this.references) {
                reference.scrollPos = EditorGUILayout.BeginScrollView(reference.scrollPos);
                reference.HandleInspectorGUI();
                EditorGUILayout.EndScrollView();
            }
        }
 
        private void OnDestroy() {
            EditorApplication.playModeStateChanged -= this.EditorApplicationOnplayModeStateChanged;
            this.Flush();
        }
    }
}
#endif
