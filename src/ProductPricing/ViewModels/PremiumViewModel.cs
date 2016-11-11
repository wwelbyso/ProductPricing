using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ProductPricing.ViewModels
{
    public class PremiumViewModel
    {
        public string ProductName { get; set; }
        public List<SelectListItem> ProductNames { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Gold", Text = "Gold" },
            new SelectListItem { Value = "Platinum", Text = "Platinum" }
        };

        public string PlanName { get; set; }
        public List<SelectListItem> PlanNames { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Essential", Text = "Essential" },
            new SelectListItem { Value = "Enhanced", Text = "Enhanced" }
        };

        public string PlanType { get; set; }
        public List<SelectListItem> PlanTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Individual", Text = "Individual" },
            new SelectListItem { Value = "MultiIndividual", Text = "Multi Individual" },
            new SelectListItem { Value = "Family Floater", Text = "Family Floater" }
        };

        public string Condition { get; set; }
        public List<SelectListItem> Conditions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Non Chronic", Text = "Non Chronic" },
            new SelectListItem { Value = "Diabetic", Text = "Diabetes" },
            new SelectListItem { Value = "Cardiac", Text = "Hypertension" },
            new SelectListItem { Value = "Hyperlipidaemia", Text = "Hyperlipidemia" },
            new SelectListItem { Value = "Asthma", Text = "Asthma" }
        };

        //Condition Checkboxes 
        public bool Diabetes { get; set; }
        public bool Hypertension { get; set; }
        public bool Hyperlipidaemia { get; set; }
        public bool Asthma { get; set; }


        public string Gender { get; set; }
        public List<SelectListItem> Genders { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" }
        };

        //Family Composition Options
        //public bool gender { get; set; }
        public bool Spouse { get; set; }
        public bool Father { get; set; }
        public bool Mother { get; set; }
        public bool FatherInLaw { get; set; }
        public bool MotherInLaw { get; set; }

        public int Kids { get; set; }

        public int Age { get; set; }

        public string Channel { get; set; }
        public List<SelectListItem> Channels { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Agency", Text = "Agency" },
            new SelectListItem { Value = "Banca", Text = "Banca" },
            new SelectListItem { Value = "Affliate", Text = "Affliate" },
            new SelectListItem { Value = "Worksite", Text = "Worksite" },
            new SelectListItem { Value = "CorporateAgent", Text = "Corporate Agent" },
            new SelectListItem { Value = "Broker", Text = "Broker" },
            new SelectListItem { Value = "Online", Text = "Online" },
            new SelectListItem { Value = "Direct", Text = "Direct" }
        };

        //add staff flag 
        public bool Staff { get; set; }

        public int Term { get; set; }
        public List<SelectListItem> Terms { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "1 year" },
            new SelectListItem { Value = "2", Text = "2 years" },
            new SelectListItem { Value = "3", Text = "3 years" }
        };


        public int SumInsured { get; set; }
        public List<SelectListItem> SumInsureds { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "50000", Text = "50000" },
            new SelectListItem { Value = "75000", Text = "75000" },
            new SelectListItem { Value = "100000", Text = "100000" },
            new SelectListItem { Value = "200000", Text = "200000" },
            new SelectListItem { Value = "300000", Text = "300000" },
            new SelectListItem { Value = "400000", Text = "400000" },
            new SelectListItem { Value = "500000", Text = "500000" },
            new SelectListItem { Value = "600000", Text = "600000" },
            new SelectListItem { Value = "700000", Text = "700000" },
            new SelectListItem { Value = "800000", Text = "800000" },
            new SelectListItem { Value = "900000", Text = "900000" },
            new SelectListItem { Value = "1000000", Text = "1000000" },
            new SelectListItem { Value = "1500000", Text = "1500000" },
            new SelectListItem { Value = "2000000", Text = "2000000" },
            new SelectListItem { Value = "2500000", Text = "2500000" },
            new SelectListItem { Value = "3000000", Text = "3000000" },
            new SelectListItem { Value = "4000000", Text = "4000000" },
            new SelectListItem { Value = "5000000", Text = "5000000" },
            new SelectListItem { Value = "10000000", Text = "10000000" },
            new SelectListItem { Value = "20000000", Text = "20000000" }
        };

        public int Deductible { get; set; }
        public List<SelectListItem> Deductibles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "0" },
            new SelectListItem { Value = "25000", Text = "25000" },
            new SelectListItem { Value = "50000", Text = "50000" },
            new SelectListItem { Value = "100000", Text = "100000" },
            new SelectListItem { Value = "200000", Text = "200000" },
            new SelectListItem { Value = "300000", Text = "300000" },
            new SelectListItem { Value = "400000", Text = "400000" },
            new SelectListItem { Value = "500000", Text = "500000" }
        };

        public int Zone { get; set; }
        public List<SelectListItem> Zones { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Zone 1" },
            new SelectListItem { Value = "2", Text = "Zone 2" },
            new SelectListItem { Value = "3", Text = "Zone 3" }
        };

        public int RoomType { get; set; }
        public List<SelectListItem> RoomTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Single" },
            new SelectListItem { Value = "2", Text = "Shared" },
            new SelectListItem { Value = "3", Text = "General/Economy Ward" },
            new SelectListItem { Value = "4", Text = "Any Room" }
        };

        public int HospitalCash { get; set; }
        public List<SelectListItem> HospitalCashs { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "0" },
            new SelectListItem { Value = "500", Text = "500" },
            new SelectListItem { Value = "1000", Text = "1000" },
            new SelectListItem { Value = "1500", Text = "1500" },
            new SelectListItem { Value = "2000", Text = "2000" },
            new SelectListItem { Value = "2500", Text = "2500" },
            new SelectListItem { Value = "3000", Text = "3000" },
            new SelectListItem { Value = "3500", Text = "3500" },
            new SelectListItem { Value = "3500", Text = "3500" },
            new SelectListItem { Value = "4000", Text = "4000" },
            new SelectListItem { Value = "4500", Text = "4500" },
            new SelectListItem { Value = "5000", Text = "5000" }
        };

        public int OPD { get; set; }
        public List<SelectListItem> OPDs { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "0" },
            new SelectListItem { Value = "1000", Text = "1000" },
            new SelectListItem { Value = "2000", Text = "2000" },
            new SelectListItem { Value = "3000", Text = "3000" },
            new SelectListItem { Value = "4000", Text = "4000" },
            new SelectListItem { Value = "5000", Text = "5000" },
            new SelectListItem { Value = "6000", Text = "6000" },
            new SelectListItem { Value = "7000", Text = "7000" },
            new SelectListItem { Value = "8000", Text = "8000" },
            new SelectListItem { Value = "9000", Text = "9000" },
            new SelectListItem { Value = "10000", Text = "10000" },
            new SelectListItem { Value = "11000", Text = "11000" },
            new SelectListItem { Value = "12000", Text = "12000" },
            new SelectListItem { Value = "13000", Text = "13000" },
            new SelectListItem { Value = "14000", Text = "14000" },
            new SelectListItem { Value = "15000", Text = "15000" },
            new SelectListItem { Value = "16000", Text = "16000" },
            new SelectListItem { Value = "17000", Text = "17000" },
            new SelectListItem { Value = "18000", Text = "18000" },
            new SelectListItem { Value = "19000", Text = "19000" },
            new SelectListItem { Value = "20000", Text = "20000" }
        };

        public bool Maternity { get; set; }

        public bool PremiumWaiver { get; set; }

        public string Message { get; set; }

        //List of Premium Objects 
        //public virtual List<PremiumCalc> PremiumCalcs { get; } = new List<PremiumCalc>();
        public List<PremiumCalc> Premiums { get; set; } = new List<PremiumCalc>();

    }

    public class PremiumCalc
    {

        public string name { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BasePremium { get; set; }
        public List<PremiumItem> premiumItems { get; set; } = new List<PremiumItem>();

        public decimal LoadingFactor { get; set; }
        public decimal DiscountFactor { get; set; }
        public decimal RelativesFactor { get; set; }
        public decimal TotalFactor { get; set; }

        public string Info { get; set; }

    }

    public class PremiumItem
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        //public decimal DiscountAmount { get; set; }
        //public decimal RelativitiesAmount { get; set; }
        public string Info { get; set; }

    }


    
}