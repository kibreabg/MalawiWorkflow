using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.ZADS.Model;

namespace Chai.ZADS.DataAccess.Admin
{
    public class StoreDao
    {

        private ZADSEntities db = new ZADSEntities();
        public StoreDao()
        {

        }
        public void SaveOrUpdateStore(Store store, bool flag)
        {
            if (flag == false)
                db.Stores.Add(store);
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            Store cust = db.Stores.Find(id);
            db.Stores.Remove(cust);
            db.SaveChanges();
        }
        public List<Store> GetStores()
        {
            var stores = from st in db.Stores select st;
            return stores.ToList();
        }
        public Store GetStore(int id)
        {
            return db.Stores.Find(id);
        }
    }
}
