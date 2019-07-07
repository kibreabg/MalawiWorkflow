using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Admin.Util
{
    public enum DirectoryItemType
    {
        DIRECTORY,
        FILE
    }

    public class DirectoryItem
    {
        private string _folderPath;
        private string _fileName;
        private DirectoryItemType _itemType;
        
        public DirectoryItem(string folderpath, string filename, DirectoryItemType itemtype)
        {
            _folderPath = folderpath;
            _fileName = filename;
            _itemType = itemtype;
        }

        private DirectoryItem()
        {
        }

        public string FolderPath
        {
            get { return _folderPath; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public DirectoryItemType ItemType
        {
            get { return _itemType; }
        }
    }
}
