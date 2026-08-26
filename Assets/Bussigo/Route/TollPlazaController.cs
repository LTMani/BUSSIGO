using System;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Route
{
    public class TollPlazaController : MonoBehaviour
    {
        public string tollPlazaName = "Kanchikacherla FASTag Toll Plaza";
        public int tollFeeINR = 135;
        public bool isBarrierOpen = false;
        public Transform barrierArmTransform;

        private bool isPaid = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BusPlayer") && !isPaid)
            {
                isPaid = true;
                isBarrierOpen = true;
                if (BussigoGameManager.Instance != null)
                {
                    BussigoGameManager.Instance.DeductFastagToll(tollFeeINR);
                }
                Debug.Log($"[FASTag] RFID Scanned at {tollPlazaName}. ₹{tollFeeINR} deducted.");
            }
        }

        private void Update()
        {
            if (barrierArmTransform != null)
            {
                float targetAngle = isBarrierOpen ? 60f : 0f;
                Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);
                barrierArmTransform.localRotation = Quaternion.Slerp(barrierArmTransform.localRotation, targetRot, Time.deltaTime * 4f);
            }
        }
    }
}
