using System;
using System.Collections.Generic;
using System.IO;

namespace MinTC.Model
{
    class FileModel
    {
        #region variable declarations
        private String[] _allDirectories;
        private String[] _allFiles;
        #endregion

        #region constructor
        public FileModel(string path, string drive)
        {
            FullPath = path;
            try
            {
                _allDirectories = Directory.GetDirectories(path);
                _allFiles = Directory.GetFiles(path);
                foreach (String dir in _allDirectories)
                {
                    Items.Add(dir.Replace(path, "<"+drive[0]+">"));
                }
                foreach (String file in _allFiles)
                {
                    Items.Add(file.Replace(path, ""));
                }
            }
            catch (DirectoryNotFoundException e) { }
        }
        #endregion

        #region properties
        public string FullPath { get; set; } = "";
        public List<String> Items { get; set; } = new List<string>();
        #endregion
    }
}
