using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using main;
using form;

namespace UnlimitedMod
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;

        public override void Load()
        {
            Log = base.Log;
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            Log.LogInfo("Press F1 to toggle the mod menu");
            
            AddComponent<ModBehaviour>();
        }
    }

    public class ModBehaviour : MonoBehaviour
    {
        private bool showMenu = false;
        private Rect windowRect = new Rect(20, 20, 350, 420);
        
        private string moneyAmount = "1000000";
        private string ticketAmount = "100";
        private string happyAmount = "1000";
        private string researchAmount = "1000";
        
        private string statusMessage = "Ready";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                showMenu = !showMenu;
                Plugin.Log.LogInfo(showMenu ? "Menu opened" : "Menu closed");
            }
        }

        private void OnGUI()
        {
            if (!showMenu) return;
            
            windowRect = GUI.Window(12345, windowRect, (GUI.WindowFunction)DrawWindow, "Dream House Days DX - Unlimited Mod");
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            
            GUILayout.Label("Enter amount and click Add:");
            GUILayout.Space(10);
            
            // === MONEY ===
            GUILayout.BeginHorizontal();
            GUILayout.Label("Money:", GUILayout.Width(100));
            moneyAmount = GUILayout.TextField(moneyAmount, GUILayout.Width(100));
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                AddMoney();
            }
            if (GUILayout.Button("MAX", GUILayout.Width(50)))
            {
                moneyAmount = "999999999";
                AddMoney();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            
            // === TICKETS ===
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tickets:", GUILayout.Width(100));
            ticketAmount = GUILayout.TextField(ticketAmount, GUILayout.Width(100));
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                AddTickets();
            }
            if (GUILayout.Button("MAX", GUILayout.Width(50)))
            {
                ticketAmount = "9999";
                AddTickets();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            
            // === HAPPINESS ===
            GUILayout.BeginHorizontal();
            GUILayout.Label("Happiness:", GUILayout.Width(100));
            happyAmount = GUILayout.TextField(happyAmount, GUILayout.Width(100));
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                AddHappy();
            }
            if (GUILayout.Button("MAX", GUILayout.Width(50)))
            {
                happyAmount = "999999";
                AddHappy();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            
            // === RESEARCH POINTS ===
            GUILayout.BeginHorizontal();
            GUILayout.Label("Research:", GUILayout.Width(100));
            researchAmount = GUILayout.TextField(researchAmount, GUILayout.Width(100));
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                AddResearchPoints();
            }
            if (GUILayout.Button("MAX", GUILayout.Width(50)))
            {
                researchAmount = "999999";
                AddResearchPoints();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            
            // === QUICK ADD ALL ===
            GUILayout.Label("─────────────────────────────────");
            GUILayout.Space(5);
            
            if (GUILayout.Button("ADD ALL (Using values above)", GUILayout.Height(35)))
            {
                AddMoney();
                AddTickets();
                AddHappy();
                AddResearchPoints();
                statusMessage = "Added all resources!";
            }
            GUILayout.Space(10);
            
            GUILayout.Label($"Status: {statusMessage}");
            GUILayout.Space(5);
            GUILayout.Label("Press F1 to close this menu");
            GUILayout.Space(5);
            GUILayout.Label("by Sagar Chaulagain");
            
            GUILayout.EndVertical();
            
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void AddMoney()
        {
            try
            {
                var appData = AppData.GetInstance();
                if (appData != null)
                {
                    if (int.TryParse(moneyAmount, out int amount))
                    {
                        long newMoney = appData.AddMoney(amount);
                        statusMessage = $"Money: +{amount:N0} (Total: {newMoney:N0})";
                        Plugin.Log.LogInfo(statusMessage);
                    }
                    else
                    {
                        statusMessage = "Invalid money amount";
                        Plugin.Log.LogWarning(statusMessage);
                    }
                }
                else
                {
                    statusMessage = "Not in game yet";
                    Plugin.Log.LogWarning("AppData instance not found. Make sure you're in-game.");
                }
            }
            catch (System.Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                Plugin.Log.LogError($"Error adding money: {ex.Message}");
            }
        }

        private void AddTickets()
        {
            try
            {
                var appData = AppData.GetInstance();
                if (appData != null)
                {
                    if (int.TryParse(ticketAmount, out int amount))
                    {
                        int newTickets = appData.AddTicket(amount, SubForm.GET_COIN_TYPE.IN_GAME);
                        statusMessage = $"Tickets: +{amount:N0} (Total: {newTickets:N0})";
                        Plugin.Log.LogInfo(statusMessage);
                    }
                    else
                    {
                        statusMessage = "Invalid ticket amount";
                        Plugin.Log.LogWarning(statusMessage);
                    }
                }
                else
                {
                    statusMessage = "Not in game yet";
                    Plugin.Log.LogWarning("AppData instance not found. Make sure you're in-game.");
                }
            }
            catch (System.Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                Plugin.Log.LogError($"Error adding tickets: {ex.Message}");
            }
        }

        private void AddHappy()
        {
            try
            {
                var appData = AppData.GetInstance();
                if (appData != null)
                {
                    if (int.TryParse(happyAmount, out int amount))
                    {
                        int newHappy = appData.AddHappy(amount);
                        statusMessage = $"Happiness: +{amount:N0} (Total: {newHappy:N0})";
                        Plugin.Log.LogInfo(statusMessage);
                    }
                    else
                    {
                        statusMessage = "Invalid happiness amount";
                        Plugin.Log.LogWarning(statusMessage);
                    }
                }
                else
                {
                    statusMessage = "Not in game yet";
                    Plugin.Log.LogWarning("AppData instance not found. Make sure you're in-game.");
                }
            }
            catch (System.Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                Plugin.Log.LogError($"Error adding happiness: {ex.Message}");
            }
        }

        private void AddResearchPoints()
        {
            try
            {
                var appData = AppData.GetInstance();
                if (appData != null)
                {
                    if (int.TryParse(researchAmount, out int amount))
                    {
                        int newResearch = appData.AddResearchPoint(amount);
                        statusMessage = $"Research: +{amount:N0} (Total: {newResearch:N0})";
                        Plugin.Log.LogInfo(statusMessage);
                    }
                    else
                    {
                        statusMessage = "Invalid research amount";
                        Plugin.Log.LogWarning(statusMessage);
                    }
                }
                else
                {
                    statusMessage = "Not in game yet";
                    Plugin.Log.LogWarning("AppData instance not found. Make sure you're in-game.");
                }
            }
            catch (System.Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                Plugin.Log.LogError($"Error adding research points: {ex.Message}");
            }
        }
    }

    public static class MyPluginInfo
    {
        public const string PLUGIN_GUID = "com.sagarchaulagain.unlimitedmod";
        public const string PLUGIN_NAME = "UnlimitedMod";
        public const string PLUGIN_VERSION = "1.0.0";
    }
}
