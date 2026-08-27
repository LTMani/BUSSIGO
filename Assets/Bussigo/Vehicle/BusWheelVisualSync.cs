using System;
using UnityEngine;
using Bussigo.Physics;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Synchronizes visual wheel transforms to vehicle speed, steering angle, and suspension compression.
    /// </summary>
    public class BusWheelVisualSync : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusModelRigHierarchy rigHierarchy;

        private float currentWheelRotationDeg = 0f;

        private void Update()
        {
            if (chassisController == null || rigHierarchy == null) return;

            float speedMps = chassisController.currentSpeedKmh / 3.6f;
            float wheelRadius = chassisController.physicsModel.wheelRadiusMeters;
            float angularVelocityRad = speedMps / wheelRadius;
            currentWheelRotationDeg += angularVelocityRad * Mathf.Rad2Deg * Time.deltaTime;

            float currentSteerAngle = chassisController.physicsModel.CalculateSteerAngle(
                steerInput01: chassisController.SteerInput,
                vehicleSpeedKmh: chassisController.currentSpeedKmh
            );

            // 1. Front Left Wheel (Rotation + Steering Yaw)
            if (rigHierarchy.wheelFrontLeft != null)
            {
                rigHierarchy.wheelFrontLeft.localRotation = Quaternion.Euler(currentWheelRotationDeg, currentSteerAngle, 0f);
            }

            // 2. Front Right Wheel (Rotation + Steering Yaw)
            if (rigHierarchy.wheelFrontRight != null)
            {
                rigHierarchy.wheelFrontRight.localRotation = Quaternion.Euler(currentWheelRotationDeg, currentSteerAngle, 0f);
            }

            // 3. Rear Dual Wheels (Rotation only)
            Quaternion rearRotation = Quaternion.Euler(currentWheelRotationDeg, 0f, 0f);

            if (rigHierarchy.wheelRearLeftInner != null) rigHierarchy.wheelRearLeftInner.localRotation = rearRotation;
            if (rigHierarchy.wheelRearLeftOuter != null) rigHierarchy.wheelRearLeftOuter.localRotation = rearRotation;
            if (rigHierarchy.wheelRearRightInner != null) rigHierarchy.wheelRearRightInner.localRotation = rearRotation;
            if (rigHierarchy.wheelRearRightOuter != null) rigHierarchy.wheelRearRightOuter.localRotation = rearRotation;
        }
    }
}
