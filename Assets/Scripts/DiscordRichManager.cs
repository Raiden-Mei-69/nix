using Discord;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DiscordRich
{
    [Serializable, ExecuteAlways]
    public class DiscordRichManager : MonoBehaviour
    {
        public static DiscordRichManager Instance;
        private static readonly HttpClient client = new();
        public Discord.Discord discord;
        public Discord.User user;
        public DiscordData discordData = new();
        public PlayerDataDiscord playerDataDiscord = new();
        public ServerSocket socket = new();
        public DiscordUserData userData = new();
        internal bool ready = false;
        public void Awake()
        {
            Instance = this;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
#else
            if(Application.IsPlaying(this))
#endif
            {
                StartConnection();
            }
        }

        public void StartConnection()
        {
            discord = new(discordData.client_id, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            UpdatePresence();
            ExecuteUpdate().ConfigureAwait(false);
            discord.GetUserManager().OnCurrentUserUpdate += DiscordRichManager_OnCurrentUserUpdate;
        }

        private void DiscordRichManager_OnCurrentUserUpdate()
        {
            Debug.LogWarning("<color=yellow>User Updated</color>");
            user = discord.GetUserManager().GetCurrentUser();
            ready = true;
            userData.Set(user.Username, user.Id);
            Debug.Log(user.Username);
            socket.discord = this;
            socket.ConnectToServer().ConfigureAwait(true);
        }

        private void NewMethod()
        {
            if (Application.IsPlaying(gameObject))
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            socket.discord = this;
            discord = new Discord.Discord(clientId: discordData.client_id, (UInt64)Discord.CreateFlags.Default);
            //Debug.Log($"--- {discord == null} ---");
            UserManager userManager = discord.GetUserManager();
            //Debug.Log($"--- {userManager == null} ---");
            userManager.OnCurrentUserUpdate += UserManager_OnCurrentUserUpdate;
            //Debug.Log($"user:{user.Username}");

            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = "Still Testing!",
                Details = "Bigger Test! HMMM!!!"
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    UnityEngine.Debug.LogWarning("Everything is fine!");
                }
                else
                {
                    UnityEngine.Debug.LogError($"An error occured {res}");
                }
                Debug.Log(res);
            });
            UpdatePresence();
            StartCoroutine(UpdateCallback());
        }

        private void UserManager_OnCurrentUserUpdate()
        {
            var userManager = discord.GetUserManager();
            var user = userManager.GetCurrentUser();
            var premium = userManager.GetCurrentUserPremiumType();
            var currentUser = userManager.GetCurrentUser();
            Debug.Log("<color=green>--- Updated ---</color>");
            //Debug.Log($"<color=yellow>--- {user.Username} ---</color>");
            //Debug.Log($"<color=yellow>--- {premium} ---</color>");
            //Debug.Log($"<color=yellow>--- {currentUser.Username} ---</color>");
            this.user = currentUser;
            Debug.Log($"user:{user.Username}");
            userData.Set(user.Username, user.Id);
            socket.ConnectToServer().ConfigureAwait(true);
            ready = true;
        }

        public void Close()
        {
            if (discord != null)
            {
                discord.Dispose();
                Debug.Log("Discord closed!");
            }
            else
            {
                Debug.Log("Discord was null!");
            }
        }

        public async Task ExecuteUpdate()
        {
            UpdatePresence();
#if UNITY_EDITOR
            while (EditorApplication.isPlaying)
#else
            while(Application.isPlaying)
#endif
            {
                discord.RunCallbacks();
                await Task.Yield();
            }
            discord.Dispose();
            await Task.Yield();
        }

        public IEnumerator UpdateCallback()
        {
            while (Application.isPlaying)
            {
                discord.RunCallbacks();
                Debug.Log("Up");
                yield return new WaitForSeconds(1f);
            }
            Close();
        }

        void UpdatePresence()
        {
            var activityManager = discord.GetActivityManager();
            var applicationManager = discord.GetApplicationManager();
            Discord.ActivityTimestamps activityTimestamps = new Discord.ActivityTimestamps();
            var timeStart = activityTimestamps.Start;
            var timeEnd = activityTimestamps.End;

            var activity = new Discord.Activity
            {
                State = "Solo Survival!",
                Details = "Round 1",
                Timestamps = activityTimestamps,
                Assets = new Discord.ActivityAssets
                {
                    //LargeImage = "danbooru_4774386_02aae334b8909ee7632e91c57e890567",
                    LargeImage = "lucy",
                    LargeText = "Numbani",
                    SmallImage = "lucy",
                    //SmallImage = "danbooru_4774386_02aae334b8909ee7632e91c57e890567",
                    SmallText = "Destroyer - Level 100"
                },
                Party = new ActivityParty
                {
                    Size = new PartySize { CurrentSize = 1, MaxSize = 4 },
                    Id = "ae488379 - 351d - 4a4f - ad32 - 2b9b01c91657",
                    //Privacy = ActivityPartyPrivacy.Public
                },
                Secrets = new ActivitySecrets { Join = "MTI4NzM0OjFpMmhuZToxMjMxMjM= " },
                Name = "UWU!",
                //Elysia
                //ApplicationId = 1024406045218590820,
                //Arlecchino
                ApplicationId = 1030904836176228522,
                Instance = true,
                Type = ActivityType.Streaming,
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    UnityEngine.Debug.LogWarning("Status Updated!");
                }
            });
        }
    }

    [Serializable]
    public class DiscordData
    {
        public long client_id = 1030904836176228522;
        public long client_id_second = 965710459179266078;
        public long application_id = 1030904836176228522;
        public string public_key = "ef7ee84ac72abec794476d8c9eb37bbf8917fb144d5e5a6b40494368da8e0826";
        public string client_secret = "FICuwuYs71uVIzsm8UfXaBczGhyowygf";
        public string redirect = "https://127.0.0.1";
        public string oauth = "https://discord.com/api/oauth2/authorize?client_id=1030904836176228522&redirect_uri=https%3A%2F%2F127.0.0.1&response_type=code&scope=identify%20connections%20rpc%20rpc.notifications.read%20rpc.voice.read%20rpc.voice.write%20rpc.activities.write%20activities.read%20activities.write";
    }

    [Serializable]
    public struct PlayerDataDiscord
    {
        public string characterName;
        public string regionName;
        public int level;

        public void UpdateData(string characterName = "", string regionName = "", int level = 0)
        {
            this.characterName ??= characterName;
            this.regionName ??= regionName;

            this.level = level > 0 ? level : this.level;
        }
    }

    [Serializable]
    public class DiscordUserData
    {
        public string username;
        public long id;
        public DateTime connectedAt;
        public DateTime lastConnectedAt;

        public void Set(string username, long id)
        {
            this.username = username;
            this.id = id;
            if (connectedAt == null)
                connectedAt = DateTime.UtcNow;
            lastConnectedAt = DateTime.UtcNow;
        }
    }

    [Serializable]
    public class ServerSocket
    {
        public static Uri baseUri = new("http://localhost:8888/aponia");
        public string connectionUri = $"{baseUri}/connection";
        public DiscordRichManager discord;

        public async Task ConnectToServer()
        {
            HttpClient client = new();
            string json = JsonUtility.ToJson(discord.userData, true);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            var res = await client.PostAsync(connectionUri, content);
            var result = await res.Content.ReadAsStringAsync();
            Debug.Log(result);
            await Task.Yield();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DiscordRichManager))]
    public class DiscordRichManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DiscordRichManager discord = (DiscordRichManager)target;
            if (GUILayout.Button("Close"))
            {
                discord.Close();
            }
            base.OnInspectorGUI();
        }
    }
#endif
}
