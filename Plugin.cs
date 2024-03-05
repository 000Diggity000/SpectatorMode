using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;
using SpectatorMode;
using static SpectatorMode.Plugin;
using System;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.TextCore;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

namespace SpectatorMode
{
    [BepInPlugin("com.000diggity000.SpectatorMode", "SpectatorMode", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public bool godmode = false;
        public GameObject player;
        public Vec2 oldPos;
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");//feel free to remove this
            Harmony harmony = new Harmony("com.Diggity.SpectatorMode");
            MethodBase original = AccessTools.Method(typeof(PlayerHandler), "HasAliveTeammate");
            harmony.Patch(original, new HarmonyMethod(AccessTools.Method(typeof(myPatches), "HasAliveTeammate")));
        }
        private void Start()
        {
            //string modPath = Path.GetDirectoryName(Info.Location);
            //AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(modPath, "Assets/splotch.bundle"));
            //GameObject testPrefab = (GameObject)bundle.LoadAsset("Square");
            //GameObject.Instantiate(testPrefab, new Vector2(0, 0), Quaternion.identity);
        }
        private void Update()
        {
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                foreach (SlimeController c in GameObject.FindObjectsOfType<SlimeController>())
                {
                    if (c.playerNumber == 1)
                    {
                        player = c.gameObject;
                    }
                }
                if (godmode)
                {
                    //GameObject playerturnOff = GameObject.FindObjectOfType<SlimeController>().gameObject;
                    
                    player.GetComponent<SlimeController>().enabled = true;
                    player.GetComponent<PlayerPhysics>().enabled = true;
                    player.GetComponent<DPhysicsBox>().enabled = true;
                    player.GetComponent<PlayerBody>().enabled = true;
                    player.GetComponent<DestroyIfOutsideSceneBounds>().enabled = true;
                    player.GetComponent<SlimeTrailHandler>().enabled = true;
                    player.GetComponent<FixTransform>().position = oldPos;
                    FindObjectOfType<PlayerAverageCamera>().enabled = true;
                    godmode = false;
                    return;
                }
                //GameObject player = GameObject.FindObjectOfType<SlimeController>().gameObject;
                oldPos = player.GetComponent<FixTransform>().position;
                FindObjectOfType<PlayerAverageCamera>().cameraSpeed = 9999;
                player.GetComponent<FixTransform>().position = new Vec2((Fix)0, (Fix)5.5);
                FindObjectOfType<PlayerAverageCamera>().cameraSpeed = 1.5f;
                player.GetComponent<SlimeController>().enabled = false;
                player.GetComponent<PlayerPhysics>().enabled = false;
                player.GetComponent<DPhysicsBox>().enabled = false;
                player.GetComponent<PlayerBody>().enabled = false;
                player.GetComponent<DestroyIfOutsideSceneBounds>().enabled = false;
                player.GetComponent<SlimeTrailHandler>().enabled = false;
                FindObjectOfType<PlayerAverageCamera>().enabled = false;
                player.GetComponent<FixTransform>().position = oldPos;
                godmode = true;

            }
            if (!godmode)
            {
                return;
            }
            if (Keyboard.current[Key.W].isPressed)
            {
                player.GetComponent<FixTransform>().position.y += (Fix)1;
                GameObject.FindObjectOfType<Camera>().transform.position = new Vector2(0, GameObject.FindObjectOfType<Camera>().transform.position.y + 1);
            }
            if (Keyboard.current[Key.S].isPressed)
            {
                player.GetComponent<FixTransform>().position.y -= (Fix)1;
                GameObject.FindObjectOfType<Camera>().transform.position = new Vector2(0, GameObject.FindObjectOfType<Camera>().transform.position.y - 1);
            }
            if (Keyboard.current[Key.D].isPressed)
            {
                player.GetComponent<FixTransform>().position.x += (Fix)1;
                GameObject.FindObjectOfType<Camera>().transform.position = new Vector2(GameObject.FindObjectOfType<Camera>().transform.position.x + 1, 0);
            }
            if (Keyboard.current[Key.A].isPressed)
            {
                player.GetComponent<FixTransform>().position.x -= (Fix)1;
                GameObject.FindObjectOfType<Camera>().transform.position = new Vector2(GameObject.FindObjectOfType<Camera>().transform.position.x - 1, 0);
            }
        }
    }
    public class myPatches
    {
        public static bool HasAliveTeammate(int playerId, ref bool __result)
        {
            __result = true;
            return false;
        }

    }

}
