using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopovLaba3
{
    public static class FileHelper
    {
        public static string GetItem(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(baseDirectory);

            if (parentDirectory != null)
            {
                DirectoryInfo grandParentDirectory = parentDirectory.Parent;

                if (grandParentDirectory != null)
                {
                    DirectoryInfo grandGrandParentDirectory = grandParentDirectory.Parent;

                    if (grandGrandParentDirectory != null)
                    {
                        return Path.Combine(grandGrandParentDirectory.FullName, "data\\objects", fileName);
                    }
                }
            }

            // Если не удалось получить две родительские директории, возвращаем путь в текущей директории
            return Path.Combine(baseDirectory, "data\\objects", fileName);
        }

        public static string GetDB(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo parentDirectory = Directory.GetParent(baseDirectory);

            if (parentDirectory != null)
            {
                DirectoryInfo grandParentDirectory = parentDirectory.Parent;

                if (grandParentDirectory != null)
                {
                    DirectoryInfo grandGrandParentDirectory = grandParentDirectory.Parent;

                    if (grandGrandParentDirectory != null)
                    {
                        return Path.Combine(grandGrandParentDirectory.FullName, "data\\database", fileName);
                    }
                }
            }

            // Если не удалось получить две родительские директории, возвращаем путь в текущей директории
            return Path.Combine(baseDirectory, "data\\database", fileName);
        }
    }
}
