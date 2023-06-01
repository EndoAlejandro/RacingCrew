using UnityEngine;

namespace Templates
{
    // All interfaces start with a capital I in the name.
    public interface ISample
    {
        // If we need to know the transform of a component that implements an interface, we use this.
        Transform transform { get; }

        // We can not implement variables in interfaces, so we use properties instead.
        float SampleProperty { get; }

        // Since all in the interface is public, we do not write that.
        void SampleInterfaceMethod();
    }
}