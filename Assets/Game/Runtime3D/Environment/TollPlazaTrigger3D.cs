using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Environment
{
    public class TollPlazaTrigger3D : MonoBehaviour
    {
        [Header("Toll Plaza Properties")]
        public string tollPlazaName = "Kanchikacherla FASTag Plaza (NH65)";
        public float tollFeeRupees = 385f;
        public Transform barrierArmTransform;

        [Header("State")]
        public bool isBarrierOpen = false;
        public bool hasTollBeenPaid = false;

        private float barrierAngle01 = 0f;

        private void Update()
        {
            float targetAngle = isBarrierOpen ? 1.0f : 0.0f;
            barrierAngle01 = Mathf.MoveTowards(barrierAngle01, targetAngle, Time.deltaTime * 2.5f);

            if (barrierArmTransform != null)
            {
                // Rotate barrier 80 degrees upward when open
                barrierArmTransform.localRotation = Quaternion.Euler(barrierAngle01 * -80f, 0f, 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BusPlayer") && !hasTollBeenPaid)
            {
                hasTollBeenPaid = true;
                isBarrierOpen = true;
                
                Debug.Log($"[FASTag RFID] Auto-deducted ₹{tollFeeRupees} at {tollPlazaName}. Barrier opened.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BusPlayer"))
            {
                // Close barrier once bus passes through
                isBarrierOpen = false;
            }
        }

        public static GameObject CreateTollPlaza(Vector3 position, Quaternion rotation)
        {
            GameObject plazaRoot = new GameObject("TollPlaza_Kanchikacherla");
            plazaRoot.transform.position = position;
            plazaRoot.transform.rotation = rotation;
            plazaRoot.tag = "TollPlaza";

            // Overhead Gantry Truss
            GameObject gantryObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gantryObj.name = "GantryTruss";
            gantryObj.transform.SetParent(plazaRoot.transform, false);
            gantryObj.transform.localScale = new Vector3(20f, 1.2f, 2.5f);
            gantryObj.transform.localPosition = new Vector3(0f, 6.2f, 0f);

            Material gantryMat = new Material(Shader.Find("Standard"));
            gantryMat.color = new Color(0.2f, 0.45f, 0.75f); // NHAI Blue
            gantryObj.GetComponent<Renderer>().material = gantryMat;

            // Toll Booth Islands
            for (int lane = -2; lane <= 2; lane += 2)
            {
                GameObject booth = GameObject.CreatePrimitive(PrimitiveType.Cube);
                booth.name = $"TollBooth_Lane_{lane}";
                booth.transform.SetParent(plazaRoot.transform, false);
                booth.transform.localScale = new Vector3(1.4f, 3.2f, 4.5f);
                booth.transform.localPosition = new Vector3(lane * 3.8f, 1.6f, 0f);
            }

            // FASTag Barrier Arm
            GameObject barrierPivot = new GameObject("BarrierArm_Pivot");
            barrierPivot.transform.SetParent(plazaRoot.transform, false);
            barrierPivot.transform.localPosition = new Vector3(-3.8f, 1.2f, 2.5f);

            GameObject barrierPole = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrierPole.transform.SetParent(barrierPivot.transform, false);
            barrierPole.transform.localScale = new Vector3(3.8f, 0.15f, 0.15f);
            barrierPole.transform.localPosition = new Vector3(1.9f, 0f, 0f);
            
            Material stripeMat = new Material(Shader.Find("Standard"));
            stripeMat.color = new Color(0.95f, 0.35f, 0.15f);
            barrierPole.GetComponent<Renderer>().material = stripeMat;

            // Trigger Collider for RFID
            BoxCollider triggerCol = plazaRoot.AddComponent<BoxCollider>();
            triggerCol.isTrigger = true;
            triggerCol.size = new Vector3(18f, 5f, 15f);
            triggerCol.center = new Vector3(0f, 2.5f, 0f);

            TollPlazaTrigger3D tollCtrl = plazaRoot.AddComponent<TollPlazaTrigger3D>();
            tollCtrl.barrierArmTransform = barrierPivot.transform;

            return plazaRoot;
        }
    }
}
