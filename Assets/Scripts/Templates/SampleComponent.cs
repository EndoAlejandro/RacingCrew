using UnityEngine;

namespace Templates
{
    // If we want to make sure that the GameObject has a component we write it like this.
    [RequireComponent(typeof(Rigidbody))]
    public class SampleComponent : MonoBehaviour
    {
        // All private variables are called whit _ first.
        private Rigidbody _rigidbody;
        
        // If we want to be able to see the variable in the inspector, we use SerializeField and do not use the _.
        [SerializeField] private float serializedVariable;
        
        // For public variables we use properties, so we make sure that only this class can modify the value.
        // The get means that any class can read the value of the property.
        // The private set means that only this class can write over the value of the property.
        public float PublicProperty { get; private set; }
        
        // If we want a property to be visible from the inspector, we use a variable and a property combination.
        [SerializeField] private float serializedProperty;
        // This is the same than SerializedProperty { get { return serializedProperty; } };
        public float SerializedProperty => serializedProperty;

        // Use Awake only for Initialize the value of the classes and "GetComponent" calls. 
        private void Awake() => _rigidbody = GetComponent<Rigidbody>();
        
        // Use Start to do any initial configuration of the previously initialized classes.
        private void Start() => _rigidbody.useGravity = false;
    }
}
