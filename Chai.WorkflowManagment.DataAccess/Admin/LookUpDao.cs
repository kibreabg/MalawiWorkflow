using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chai.ZADS.Model;
namespace Chai.ZADS.DataAccess.Admin
{
    public class LookUpDao
    {
        private ZADSEntities db = new ZADSEntities();
        public LookUpDao()
        {
        }

        public void Save(LookUp lookUp)
        {
            db.LookUps.Add(lookUp);
            db.SaveChanges();
        }

        public List<LookUp> GetAll()
        {
            var lookUps = from lu in db.LookUps select lu;
            return lookUps.ToList();
        }

        public LookUp GetLookUp(int luID)
        {
            return db.LookUps.Find(luID);
        }

        public LookUpDetail GetLookUpDetail(int ludID)
        {
            return db.LookUpDetails.Find(ludID);
            //var lookUpDetail = from lud in db.LookUpDetails where lud.ID == ludID select lud;                               
        }

        public void DeleteLookUpDetail(LookUp lu, LookUpDetail lud)
        {
            //var lookUpDetails = from luDet in db.LookUpDetails where luDet.LookUp.Equals(lu) select luDet;
            db.LookUpDetails.Remove(lud);
            db.SaveChanges();
        }    
    
    }
}
