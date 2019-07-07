using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Admin.Util
{
    public class ListOfDirectoryItems
    {
        private  IList<DirectoryItem> _listOfDItems;
        private int _pocModuleId;
        private bool _isRootDir;
        private string _fullName;
        private int _rootLen;

        public ListOfDirectoryItems(int rootlen, string fullname)
        {
            _listOfDItems = new List<DirectoryItem>();
            //_pocModuleId = moduleid;
            //_isRootDir = isroot;
            _fullName = fullname;
            _rootLen = rootlen;
        }

        public int ModuleId
        {
            get { return _pocModuleId; }
        }
        public int RootDirLen
        {
            get { return _rootLen; }
        }
        public bool IsRootDir
        {
            get { return _isRootDir; }
        }
        public string FullName
        {
            get { return _fullName; }
        }
        public void Add(DirectoryItem ditem)
        {
            _listOfDItems.Add(ditem);
        }

        public IList<DirectoryItem> DirectoryItems
        {
            get { return _listOfDItems; }
        }
    }
}
