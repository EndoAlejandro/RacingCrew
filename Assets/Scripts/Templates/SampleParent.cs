using UnityEngine;

namespace Templates
{
    public abstract class SampleParent : MonoBehaviour
    {
        // This variable is visible only for the class that inherit from this class.
        protected float childrenCanCallMe;

        // This variable is only visible for this class.
        private float _noChildCanSeeMe;

        // This method is only visible for this class.         
        private void OnlyFatherCanCallMe() => Debug.Log(_noChildCanSeeMe);

        // This method can me called by the children but they can not change the body of the method.
        protected void ChildrenCanCallMeButNoOverWrite() => Debug.Log(_noChildCanSeeMe);
        
        // Children can call and change the body of the method.
        protected virtual void ThisMethodCanBeOverride() => Debug.Log(_noChildCanSeeMe);
        // Each child needs to define this method.
        protected abstract void MandatoryMethod();
    }
}