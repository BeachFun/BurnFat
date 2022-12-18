using System.Text;
using System.IO;
using static System.Environment;

namespace BurnFat
{
    /// <summary>
    /// Система хранения и управления данными программы
    /// </summary>
    internal static class FileManager
    {
        /// <summary>
        /// Инициализирует поле folder, представляющее собой папку с данными программы
        /// </summary>
        static FileManager()
        {
            string appDataPath = GetFolderPath(SpecialFolder.ApplicationData); // Путь к папке C:\Users\ASUS\AppData\Roaming
            string appDataFolder = "BurnFat"; // Название папки программы
            string dirPath = Path.Combine(appDataPath, appDataFolder); // Путь к папке с данными программы

            folder = new DirectoryInfo(dirPath);
        }


        /// <summary>
        /// Представляет собой папку с данными программы
        /// </summary>
        private static DirectoryInfo folder;


        /// <summary>
        /// Сохраняет данные в указанный файл. При этом, предыдущие данные стираются.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        internal static void SaveData(string fileName, string[] data)
        {
            folder.Create();
            string filePath = Path.Combine(folder.FullName, fileName);
            File.WriteAllLines(filePath, data);
        }

        /// <summary>
        /// Чистит указанный текстовый файл
        /// </summary>
        /// <param name="fileName"></param>
        internal static void ClearFile(string fileName)
        {
            folder.Create();
            string filePath = Path.Combine(folder.FullName, fileName);
            File.WriteAllLines(filePath, new string[0]);
        }

        /// <summary>
        /// Извлекает данные из указанного текстового файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string[]? GetData(string fileName)
        {
            folder.Create();
            string filePath = Path.Combine(folder.FullName, fileName);
            try
            {
                return File.ReadAllLines(filePath, Encoding.UTF8); ;
            }
            catch
            {
                return null;
            }
        }
    }
}
