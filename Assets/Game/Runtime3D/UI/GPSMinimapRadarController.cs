using System;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;

namespace Bussigo.Game.Runtime3D.UI
{
    public class GPSMinimapRadarController : MonoBehaviour
    {
        public UnityBusController3D playerBus;
        public Vector3 destinationWorldPos = new Vector3(-3.8f, 0f, 2850f);
        public Vector3 tollPlazaWorldPos = new Vector3(0f, 0f, 1200f);

        [Header("Minimap Settings")]
        public float mapRadiusPixels = 75f;
        public float worldRadarRangeMeters = 350f;
        public bool showMinimap = true;

        private void OnGUI()
        {
            if (!showMinimap || playerBus == null) return;

            float mapSize = mapRadiusPixels * 2f;
            float mapX = Screen.width - mapSize - 20f;
            float mapY = 20f;

            GUIStyle radarBoxStyle = new GUIStyle(GUI.skin.box);
            radarBoxStyle.normal.background = Texture2D.whiteTexture;

            Color oldColor = GUI.color;
            
            // Radar Background Circle / Box
            GUI.color = new Color(0.1f, 0.15f, 0.22f, 0.85f);
            GUI.Box(new Rect(mapX, mapY, mapSize, mapSize), "");

            // Radar Center (Player Bus)
            Vector2 radarCenter = new Vector2(mapX + mapRadiusPixels, mapY + mapRadiusPixels);
            GUI.color = new Color(1.0f, 0.85f, 0.15f, 1.0f); // Gold Arrow for Bus
            GUI.Box(new Rect(radarCenter.x - 4, radarCenter.y - 4, 8, 8), "");

            // Draw Toll Plaza Icon on Radar
            DrawRadarBlip(radarCenter, tollPlazaWorldPos, playerBus.transform.position, playerBus.transform.eulerAngles.y, new Color(0.2f, 0.6f, 1.0f));

            // Draw Destination Icon on Radar
            DrawRadarBlip(radarCenter, destinationWorldPos, playerBus.transform.position, playerBus.transform.eulerAngles.y, new Color(0.2f, 0.95f, 0.3f));

            GUI.color = Color.white;
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 11;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(mapX, mapY + mapSize + 4, mapSize, 20), "GPS: NH65 SAT-NAV", labelStyle);

            GUI.color = oldColor;
        }

        private void DrawRadarBlip(Vector2 radarCenter, Vector3 targetWorldPos, Vector3 playerWorldPos, float playerHeadingDegrees, Color blipColor)
        {
            Vector3 diff = targetWorldPos - playerWorldPos;
            Vector2 localDiff = new Vector2(diff.x, diff.z);

            // Rotate relative to bus heading
            float rad = -playerHeadingDegrees * Mathf.Deg2Rad;
            float rx = localDiff.x * Mathf.Cos(rad) - localDiff.y * Mathf.Sin(rad);
            float ry = localDiff.x * Mathf.Sin(rad) + localDiff.y * Mathf.Cos(rad);

            float distanceRatio = Mathf.Clamp01(new Vector2(rx, ry).magnitude / worldRadarRangeMeters);
            Vector2 blipOffset = new Vector2(rx, -ry).normalized * (distanceRatio * (mapRadiusPixels - 8f));

            Vector2 blipPos = radarCenter + blipOffset;

            GUI.color = blipColor;
            GUI.Box(new Rect(blipPos.x - 3, blipPos.y - 3, 6, 6), "");
        }
    }
}
