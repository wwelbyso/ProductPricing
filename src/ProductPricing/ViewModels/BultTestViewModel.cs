using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ProductPricing.ViewModels
{
    public class BultTesterHeadViewModel
    {
        public int TotalTests { get; set; }
        public int TotalPass { get; set; }
        public int TotalFail { get; set; }
        public List<BultTestViewModel> bulkTest = new List<BultTestViewModel>();
    }


    public class BultTestViewModel
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string Condition { get; set; }

        //Condition Checkboxes 
        public bool Diabetes { get; set; }
        public bool Hypertension { get; set; }
        public bool Hyperlipidaemia { get; set; }   
        public bool Asthma { get; set; }


        public string Gender { get; set; }

        //Family Composition Options
        public string familyString { get; set; }
        public bool Spouse { get; set; }
        public bool Father { get; set; }
        public bool Mother { get; set; }
        public bool FatherInLaw { get; set; }
        public bool MotherInLaw { get; set; }
        public int Kids { get; set; }

            
        public int Age { get; set; }

        public string Channel { get; set; }

        //add staff flag 
        public bool Staff { get; set; }

        public int Term { get; set; }


        public int SumInsured { get; set; }
        public int Deductible { get; set; }
        public int Zone { get; set; }

        public int RoomType { get; set; }

        public int HospitalCash { get; set; }
        public int OPD { get; set; }

        public bool Maternity { get; set; }

        public bool PremiumWaiver { get; set; }

        public decimal inputPremium { get; set; }

        public PremiumCalc TotalPremium { get; set; }
        public bool TestPass { get; set; }

        public List<PremiumCalc> Premiums { get; set; } = new List<PremiumCalc>();
    }
}

