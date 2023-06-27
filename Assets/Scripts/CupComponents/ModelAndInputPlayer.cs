using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame
{
    public class ModelAndInputPlayer
    {
        public PlayerInput Input { get; private set; }
        public GameObject CarModel { get; private set; }
        public string Scheme { get; private set; }

        public ModelAndInputPlayer(PlayerInput input, GameObject carModel)
        {
            Input = input;
            CarModel = carModel;
            Scheme = Input.currentControlScheme;
        }
    }
}