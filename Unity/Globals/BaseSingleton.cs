﻿namespace Morpeh.Globals {
    using System;
    using JetBrains.Annotations;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    using Unity.IL2CPP.CompilerServices;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class BaseSingleton : ScriptableObject {
        [SerializeField]
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ReadOnly]
#elif UNITY_EDITOR && TRI_INSPECTOR
        // TODO: TRI_INSPECTOR SUPPORT
#endif
        protected int internalEntityID = -1;

        protected Entity internalEntity;

#if UNITY_EDITOR && ODIN_INSPECTOR
        [PropertyOrder(100)]
        [ShowInInspector]
        [Space]
        private Morpeh.Editor.EntityViewerWithHeader entityViewer = new Morpeh.Editor.EntityViewerWithHeader();
#elif UNITY_EDITOR && TRI_INSPECTOR
        // TODO: TRI_INSPECTOR SUPPORT
#endif

        [CanBeNull]
        private protected Entity InternalEntity {
            get {
#if UNITY_EDITOR 
                if (this.internalEntityID > -1 && this.internalEntity == null) {
                    this.internalEntity = World.Default.entities[this.internalEntityID];
                }
#endif
                return this.internalEntity;
            }
        }

        [NotNull]
        public Entity Entity {
            get {
#if UNITY_EDITOR
                if (!Application.isPlaying) {
                    return default;
                }

                this.CheckIsInitialized();
#endif
                return this.InternalEntity;
            }
        }

        protected virtual void OnEnable() {
            this.internalEntity = null;
#if UNITY_EDITOR && ODIN_INSPECTOR
            this.entityViewer = new Morpeh.Editor.EntityViewerWithHeader {getter = () => this.internalEntity};
#elif UNITY_EDITOR && TRI_INSPECTOR
            // TODO: TRI_INSPECTOR SUPPORT
#endif
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += this.OnEditorApplicationOnplayModeStateChanged;
            if (Application.isPlaying) {
#endif
                this.CheckIsInitialized();
#if UNITY_EDITOR
            }
#endif
        }

#if UNITY_EDITOR
        protected virtual void OnEditorApplicationOnplayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.EnteredEditMode) {
                this.internalEntityID = -1;
                this.internalEntity   = null;
            }
        }
#endif
        protected virtual bool CheckIsInitialized() {
            if (this.internalEntityID < 0) {
                var world = World.Default;
                var cache = world.GetCache<SingletonMarker>();
                
                this.internalEntity = world.CreateEntity(out this.internalEntityID);
                cache.AddComponent(this.internalEntity);
                
                return true;
            }

            return false;
        }

        public virtual void Dispose() {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= this.OnEditorApplicationOnplayModeStateChanged;
#endif
            if (this.internalEntityID != -1) {
                var entity = this.InternalEntity;
                if (entity != null && !entity.isDisposed) {
                    World.Default.RemoveEntity(entity);
                }

                this.internalEntityID = -1;
                this.internalEntity   = null;
            }
        }


        protected virtual void OnDestroy() {
            this.Dispose();
        }

        [Serializable]
        private struct SingletonMarker : IComponent {
        }
    }
}