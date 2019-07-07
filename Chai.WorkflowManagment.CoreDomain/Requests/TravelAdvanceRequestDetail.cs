using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class TravelAdvanceRequestDetail : IEntity
    {
        public TravelAdvanceRequestDetail()
        {
            this.TravelAdvanceCosts = new List<TravelAdvanceCost>();
        }
        public int Id { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public string HotelBooked { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string ModeOfTravel { get; set; }
        public int DriverId { get; set; }
        public decimal AirFare { get; set; }
        public virtual TravelAdvanceRequest TravelAdvanceRequest { get; set; }
        public virtual IList<TravelAdvanceCost> TravelAdvanceCosts { get; set; }

        #region TravelAdvanceCost
        public virtual TravelAdvanceCost GetTravelAdvanceCost(int tacId)
        {
            foreach (TravelAdvanceCost TAC in TravelAdvanceCosts)
            {
                if (TAC.Id == tacId)
                    return TAC;
            }
            return null;
        }

        public virtual IList<TravelAdvanceCost> GetTravelAdvanceCostsByTARId(int tarId)
        {
            IList<TravelAdvanceCost> TACs = new List<TravelAdvanceCost>();
            foreach (TravelAdvanceCost TAC in TravelAdvanceCosts)
            {
                if (TAC.TravelAdvanceRequestDetail.Id == tarId)
                    TACs.Add(TAC);
            }
            return TACs;
        }
        public virtual void RemoveTravelAdvanceCost(int Id)
        {
            foreach (TravelAdvanceCost TAC in TravelAdvanceCosts)
            {
                if (TAC.Id == Id)
                    TravelAdvanceCosts.Remove(TAC);
                break;
            }
        }
        #endregion

    }
}
