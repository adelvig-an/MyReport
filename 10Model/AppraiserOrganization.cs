using _10Model.Customer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _10Model
{
    public class AppraiserOrganization : Organization
    {
        public int InsurancePolicieId { get; set; }
        public virtual InsurancePolicie InsurancePolicie { get; set; }
        public virtual ICollection<Appraiser> Appraisers { get; set; }
    }
}
