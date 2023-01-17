#if UNITY_EDITOR
#define MORPEH_DEBUG
#endif
#if !MORPEH_DEBUG
#define MORPEH_DEBUG_DISABLED
#endif

#if ENABLE_MONO || ENABLE_IL2CPP
#define MORPEH_UNITY
#endif

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.Editor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.TestSuite")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.TestSuite.Editor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.Globals")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.Native")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Scellecs.Morpeh.Workaround")]
namespace Scellecs.Morpeh {
    using System;
    using UnityEngine;
    
    public interface IComponent {
    }

    public interface IInitializer : IDisposable {
        World World { get; set; }

        void OnAwake();
    }

    public interface ISystem : IInitializer {
        void OnUpdate(float deltaTime);
    }

    public interface IFixedSystem : ISystem {
    }

    public interface ILateSystem : ISystem {
    }

    public interface ICleanupSystem : ISystem {
    }

    public interface IWorldPlugin {
        void Initialize(World world);
    }

    public interface IValidatable {
        void OnValidate();
    }

    public interface IValidatableWithGameObject {
        void OnValidate(GameObject gameObject);
    }
    
    public interface IFilterExtension {
        Filter Extend(Filter rootFilter);
    }
    
    [AttributeUsage(AttributeTargets.Struct)]
    public class StashSizeAttribute : Attribute {
        internal int size;
        public StashSizeAttribute(int size) {
            this.size = size;
        }
    }
}

namespace Unity.IL2CPP.CompilerServices {
    using System;

#if !EXTERNAL_IL2CPP_ATTRS
    public enum Option {
#else
internal enum Option {
#endif
        NullChecks         = 1,
        ArrayBoundsChecks  = 2,
        DivideByZeroChecks = 3
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
#if !MORPEH_EXTERNAL_IL2CPP_ATTRS
    public class Il2CppSetOptionAttribute : Attribute {
#else
internal class Il2CppSetOptionAttribute : Attribute {
#endif
        public Option Option { get; }
        public object Value  { get; }

        public Il2CppSetOptionAttribute(Option option, object value) {
            this.Option = option;
            this.Value  = value;
        }
    }
}


namespace UnityEngine {
#if !MORPEH_UNITY
    public sealed class SerializeField : System.Attribute { }
    public sealed class GameObject : System.Object { }
#endif
}

namespace UnityEngine.Scripting {
#if !MORPEH_UNITY
    public sealed class Preserve : System.Attribute { }
#endif
}

namespace JetBrains.Annotations {
#if !MORPEH_UNITY
    using System;
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    public sealed class NotNullAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    public sealed class CanBeNullAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    public sealed class PublicAPIAttribute : Attribute { }
#endif
}


namespace Sirenix.OdinInspector {
#if !ODIN_INSPECTOR
    using System;
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class ShowInInspectorAttribute : Attribute { }
#endif
}
