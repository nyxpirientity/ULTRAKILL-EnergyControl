using UnityEngine;
using BepInEx;

namespace Nyxpiri.ULTRAKILL.EnergyControl
{
    [BepInPlugin("nyxpiri.ultrakill.energy-control", "Energy Control", "0.0.0.1")]
    [BepInProcess("ULTRAKILL.exe")]
    public class EnergyControlPlugin : BaseUnityPlugin
    {
        protected void Awake()
        {
            Log.Initialize(Logger);
            EnergyController.Initialize();
        }

        protected void Start()
        {
        }

        protected void Update()
        {

        }

        protected void LateUpdate()
        {

        }
    }
}
