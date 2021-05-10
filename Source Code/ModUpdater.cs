extern alias Il2CppNewtonsoft;
using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Il2CppSystem;
using Hazel;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnhollowerBaseLib;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Il2CppNewtonsoft::Newtonsoft.Json.Linq;
using Il2CppNewtonsoft::Newtonsoft.Json;
using Twitch;
using System.Text.RegularExpressions;

namespace TheOtherRoles {
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class ModUpdaterButton {
        private static void Prefix(MainMenuManager __instance) {
            CustomHatLoader.LaunchHatFetcher();
            ModUpdater.LaunchUpdater();
            if (!ModUpdater.hasUpdate) return;
            var template = GameObject.Find("ExitGameButton");
            if (template == null) return;

            var button = UnityEngine.Object.Instantiate(template, null);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 0.6f, button.transform.localPosition.z);

            PassiveButton passiveButton = button.GetComponent<PassiveButton>();
            passiveButton.OnClick = new Button.ButtonClickedEvent();
            passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)onClick);
            
            var text = button.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) => {
                text.SetText("Update\nThe Other Roles");
            })));

            TwitchManager man = DestroyableSingleton<TwitchManager>.Instance;
            ModUpdater.InfoPopup = UnityEngine.Object.Instantiate<GenericPopup>(man.TwitchPopup);

            void onClick() {
                ModUpdater.ExecuteUpdate();
                button.SetActive(false);
                ModUpdater.InfoPopup.TextAreaTMP.fontSize *= 0.7f;
                ModUpdater.InfoPopup.TextAreaTMP.enableAutoSizing = false;
                ModUpdater.InfoPopup.Show("Updating The Other Roles\nPlease wait...");
                __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) => { ModUpdater.setPopupText("Updating The Other Roles\nPlease wait..."); })));
            }
        }
    }

    public class ModUpdater { 
        public static bool running = false;
        public static bool hasUpdate = false;
        public static string updateurlTOR = null;
        public static string updateurlRR = null;
        private static Task updateTask = null;
        public static GenericPopup InfoPopup;

        public static void LaunchUpdater() {
            if (running) return;
            running = true;
            checkForUpdate().GetAwaiter().GetResult();
            clearOldVersions();   
        }

        public static void ExecuteUpdate() {
            if (updateTask == null) {
                if (updateurlTOR != null && updateurlRR != null) {
                    updateTask = downloadUpdate();
                } else {
                    showPopup("This update has to be done manually");
                }
            }
        }
        
        public static void clearOldVersions() {
            try {
                DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(Application.dataPath) + @"\BepInEx\plugins");
                string[] files = d.GetFiles("*.old").Select(x => x.FullName).ToArray(); // Getting old versions
                foreach (string f in files)
                    File.Delete(f);
            } catch (System.Exception e) {
                System.Console.WriteLine("Exception occured when clearing old versions:\n" + e);
            }
        }

        public static async Task<bool> checkForUpdate() {
            try {
                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "TheOtherRoles Updater");
                var response = await http.GetAsync(new System.Uri("https://api.github.com/repos/haoming37/Doc_TheOtherRoles/releases/latest"), HttpCompletionOption.ResponseContentRead);
                // var response = await http.GetAsync(new System.Uri("https://api.github.com/repos/EoF-1141/TheOtherRoles/releases/latest"), HttpCompletionOption.ResponseContentRead);
                if (response.StatusCode != HttpStatusCode.OK || response.Content == null) {
                    System.Console.WriteLine("Server returned no data: " + response.StatusCode.ToString());
                    return false;
                }
                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);
                
                string tagname = data["tag_name"]?.ToString();
                if (tagname == null) {
                    return false; // Something went wrong
                }
                // check version
                System.Version ver = System.Version.Parse(tagname.Replace("v", ""));
                int diff = TheOtherRolesPlugin.Version.CompareTo(ver);
                if (diff < 0) { // Update required
                    hasUpdate = true;
                    JToken assets = data["assets"];
                    if (!assets.HasValues)
                        return false;
                    Regex rgxTOR = new Regex("^https://.*/download/[^/]*/TheOtherRoles-.*dll$");
                    Regex rgxRR = new Regex("^https://.*/download/[^/]*/RevealRoles-.*dll$");
                    for (JToken current = assets.First; current != null; current = current.Next) {
                        string browser_download_url = current["browser_download_url"]?.ToString();
                        if (browser_download_url != null && current["content_type"] != null) {
                            if (current["content_type"].ToString().Equals("application/x-msdownload") &&
                                browser_download_url.EndsWith(".dll")) {
                                TheOtherRolesPlugin.Instance.Log.LogError(browser_download_url);
                                if(rgxTOR.IsMatch(browser_download_url)){
                                    TheOtherRolesPlugin.Instance.Log.LogError(browser_download_url);
                                    updateurlTOR = browser_download_url;
                                }else if(rgxRR.IsMatch(browser_download_url)){
                                    TheOtherRolesPlugin.Instance.Log.LogError(browser_download_url);
                                    updateurlRR = browser_download_url;
                                }
                                if(updateurlTOR != null && updateurlRR != null){
                                    return true;
                                }
                            }
                        }
                    }
                }  
            } catch (System.Exception ex) {
                TheOtherRolesPlugin.Instance.Log.LogError(ex.ToString());
                System.Console.WriteLine(ex);
            }
            return false;
        }

        public static async Task<bool> downloadUpdate() {
            try {
                // 出力先パスの取得
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                System.UriBuilder uri = new System.UriBuilder(codeBase);
                string fullnameTOR = System.Uri.UnescapeDataString(uri.Path);
                string fullnameRR = fullnameTOR.Replace("TheOtherRolese", "RevealRoles");

                // 共通で仕様するhttpclient
                HttpClient http = new HttpClient();

                // TheOtherRolesのダウンロード処理実行
                http.DefaultRequestHeaders.Add("User-Agent", "TheOtherRoles Updater");
                var responseTOR = await http.GetAsync(new System.Uri(updateurlTOR), HttpCompletionOption.ResponseContentRead);
                if (responseTOR.StatusCode != HttpStatusCode.OK || responseTOR.Content == null) {
                    System.Console.WriteLine("Server returned no data: " + responseTOR.StatusCode.ToString());
                    return false;
                }

                if (File.Exists(fullnameTOR + ".old")) // Clear old file in case it wasnt;
                    File.Delete(fullnameTOR + ".old");

                File.Move(fullnameTOR, fullnameTOR + ".old"); // rename current executable to old

                using (var responseStream = await responseTOR.Content.ReadAsStreamAsync()) {
                    using (var fileStream = File.Create(fullnameTOR)) { // probably want to have proper name here
                        responseStream.CopyTo(fileStream); 
                    }
                }

                // RevealRolesのダウンロード処理実行
                var responseRR = await http.GetAsync(new System.Uri(updateurlRR), HttpCompletionOption.ResponseContentRead);
                if (responseRR.StatusCode != HttpStatusCode.OK || responseRR.Content == null) {
                    System.Console.WriteLine("Server returned no data: " + responseRR.StatusCode.ToString());
                    return false;
                }
                if (File.Exists(fullnameRR + ".old")) // Clear old file in case it wasnt;
                    File.Delete(fullnameRR + ".old");

                File.Move(fullnameRR, fullnameRR + ".old"); // rename current executable to old
                using (var responseStream = await responseRR.Content.ReadAsStreamAsync()) {
                    using (var fileStream = File.Create(fullnameRR)) { // probably want to have proper name here
                        responseStream.CopyTo(fileStream); 
                    }
                }

                showPopup("The Other Roles\nupdated successfully\nPlease restart the game.");
                return true;
            } catch (System.Exception ex) {
                TheOtherRolesPlugin.Instance.Log.LogError(ex.ToString());
                System.Console.WriteLine(ex);
            }
            showPopup("Update wasn't successful\nTry again later,\nor update manually.");
            return false;
        }
        private static void showPopup(string message) {
            setPopupText(message);
            InfoPopup.gameObject.SetActive(true);
        }

        public static void setPopupText(string message) {
            if (InfoPopup == null)
                return;
            if (InfoPopup.TextArea != null) {
                InfoPopup.TextArea.Text = message;
            }
            if (InfoPopup.TextAreaTMP != null) {
                InfoPopup.TextAreaTMP.text = message;
            }
        }
    }
}