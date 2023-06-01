// No "using UnityEngine" required because the parent already do this.

using UnityEngine;

namespace Templates
{
    public class SampleChild : SampleParent
    {
        // This class inherit from SampleParent so need to implement this method.
        protected override void MandatoryMethod()
        {
            // We can call and modify this variable because it's protected.
            childrenCanCallMe = 0f;
        }

        // We can override this method because it has the word "virtual" on the parent class.
        protected override void ThisMethodCanBeOverride()
        {
            // With this line we are calling the body of the method on the father.
            base.ThisMethodCanBeOverride();
            
            // Here we can implement new behaviour.
            Debug.Log("Hello World.");
        }
    }
}
