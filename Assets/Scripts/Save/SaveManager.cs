using System;
using System.IO;
using Environment = System.Environment;

namespace Save
{
    public static class SaveManager
    {
        private static readonly string savePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/")}/My Games/Arlecchino/save.json";
        private static readonly string savePathBak = $"{savePath}.bak";
        private static readonly string FlutSave = "C:\\Users\\lukas\\OneDrive\\Bureau\\RavenTasha\\raven\\aponia\\Arlecchino\\assets\\Data\\characters.json";
        public static void SaveGame(string data)
        {
            if (!File.Exists(savePath))
            {
                CreateFile();
            }
            if (!File.Exists(savePathBak))
            {
                CreateBak();
            }
            File.WriteAllText(savePath, data);
            File.WriteAllText(savePathBak, data);
        }

        private static void CreateBak()
        {
            FileInfo info = new(savePathBak);
            info.Directory.Create();
        }

        private static void CreateFile()
        {
            FileInfo info = new(savePath);
            info.Directory.Create();
        }

        public static string LoadSave()
        {
            if (!File.Exists(savePath))
                return string.Empty;
            string content = File.ReadAllText(savePath);
            if (content == string.Empty)
            {
                return File.ReadAllText(savePathBak);
            }
            else
                return content;
        }

        public static bool FileExist() =>
            File.Exists(savePath);

        public static int SaveFlut(string data)
        {
            try
            {
                if (!File.Exists(FlutSave))
                {
                    FileInfo info = new(FlutSave);
                    info.Directory.Create();
                }
                File.WriteAllText(FlutSave, data);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}
