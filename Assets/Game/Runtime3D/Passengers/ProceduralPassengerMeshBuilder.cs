using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Passengers
{
    public class ProceduralPassengerMeshBuilder : MonoBehaviour
    {
        public static GameObject CreatePassengerCharacter(Transform parent, Vector3 position, Color shirtColor)
        {
            GameObject pax = new GameObject("PassengerCharacter");
            pax.transform.SetParent(parent, false);
            pax.transform.position = position;
            pax.tag = "Passenger";

            // Body / Torso
            GameObject torso = GameObject.CreatePrimitive(PrimitiveType.Cube);
            torso.transform.SetParent(pax.transform, false);
            torso.transform.localScale = new Vector3(0.45f, 0.7f, 0.25f);
            torso.transform.localPosition = new Vector3(0f, 1.05f, 0f);

            Material shirtMat = new Material(Shader.Find("Standard"));
            shirtMat.color = shirtColor;
            torso.GetComponent<Renderer>().material = shirtMat;

            // Head
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.transform.SetParent(pax.transform, false);
            head.transform.localScale = new Vector3(0.28f, 0.32f, 0.28f);
            head.transform.localPosition = new Vector3(0f, 1.6f, 0f);

            Material skinMat = new Material(Shader.Find("Standard"));
            skinMat.color = new Color(0.68f, 0.45f, 0.32f); // Indian Skin Tone
            head.GetComponent<Renderer>().material = skinMat;

            // Legs
            GameObject legs = GameObject.CreatePrimitive(PrimitiveType.Cube);
            legs.transform.SetParent(pax.transform, false);
            legs.transform.localScale = new Vector3(0.4f, 0.7f, 0.22f);
            legs.transform.localPosition = new Vector3(0f, 0.35f, 0f);

            Material pantsMat = new Material(Shader.Find("Standard"));
            pantsMat.color = new Color(0.15f, 0.18f, 0.25f);
            legs.GetComponent<Renderer>().material = pantsMat;

            return pax;
        }
    }
}
