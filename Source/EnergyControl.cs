using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.EnergyControl
{
    public static class EnergyControlPlayerComponentsExtension
    {
        public static EnergyControl GetEnergyControl(this PlayerComponents player)
        {
            return player.GetMonoByIndex<EnergyControl>(EnergyControl.MonoRegistrarIdx);
        }
    }

    public class EnergyControl : MonoBehaviour
    {
        public class CostScalar
        {
            public float Scale = 0.0f;
        }

        public const float BaseDashCost = 100.0f;
        public float DashCostScale { get; private set; } = 1.0f;
        public static int MonoRegistrarIdx { get; private set; }

        public void RegisterDashCostScalar(CostScalar costScalar)
        {
            _dashCostScalars.Add(costScalar);
        }

        public void UnregisterDashCostScalar(CostScalar costScalar)
        {
            _dashCostScalars.Remove(costScalar);
        }

        protected void Start()
        {
            PlayerEvents.PreUpdate += PrePlayerUpdate;
        }

        protected void OnDestroy()
        {
            PlayerEvents.PreUpdate -= PrePlayerUpdate;
        }

        protected void FixedUpdate()
        {
            DashCostScale = 1.0f;

            foreach (var scalar in _dashCostScalars)
            {
                DashCostScale *= scalar.Scale;
            }
        }
        
        private HashSet<CostScalar> _dashCostScalars = new HashSet<CostScalar>(8);

        private void PrePlayerUpdate(NewMovement player)
        {
            if (!player.slowMode && MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame)
            {
                if (((bool)player.groundProperties && !player.groundProperties.canDash) || player.modNoDashSlide)
                {
                    return;
                }

                if (player.boostCharge >= BaseDashCost * DashCostScale && !((player.groundProperties && !player.groundProperties.canDash) || player.modNoDashSlide))
                {
                    player.boostCharge += BaseDashCost - BaseDashCost * DashCostScale;
                }
            }
        }

        internal static void Initialize()
        {
            MonoRegistrarIdx = PlayerComponents.MonoRegistrar.Register<EnergyControl>();
        }
    }
}