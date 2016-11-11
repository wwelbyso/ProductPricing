using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductPricing.Models;
using ProductPricing.Controllers.Helpers;
using ProductPricing.ViewModels;

namespace ProductPricing.BusinessLogic
{
    public class PremiumCalculator
    {
        private int cSelf = 1000000;
        private int cSpouse = 100000;
        private int cFather = 10000;
        private int cMother = 1000;
        private int cFatherInLaw = 100;
        private int cMotherInLaw = 10;
        private int cKids = 1;

        private readonly ABHIPricingDBContext _context;
        
        public string Message { get; set; }
        public PremiumCalculator(ABHIPricingDBContext context)
        {
            _context = context;
        }
        public PremiumViewModel calculatePremium(PremiumViewModel model)
        {
            PremiumItem prem;
            //Base Premium
            int famComp = 0;
            switch (model.PlanType)
            {
                case "Family Floater":
                    famComp = getFamilyComposition(model);
                    break;
                case "Individual":
                    famComp = (model.Gender == "Male") ? cSelf : (cSelf * 2);
                    break;
            }
            string chronic = "Non Chronic";
            if (model.ProductName == "Platinum" && model.PlanType != "Family Floater")
            {
                if (model.Diabetes) { chronic = "Diabetic"; }
                else if (model.Hypertension) { chronic = "Hypertension"; }
                else if (model.Hyperlipidaemia) { chronic = "Hyperlipidaemia"; }
                else if (model.Asthma) { chronic = "Asthma"; }
            }
            int deductible = model.Deductible;
            if (chronic != "Non Chronic")
            {
                deductible = 0;
            }
            int term = model.Term;
            int thisterm = term;
            int noOffBase = 0;
            //Base Premium Loop  
            PremiumCalc pmc = new PremiumCalc { name = "Base Premium" , LoadingFactor = 1m,DiscountFactor=1m,RelativesFactor=1m,TotalFactor=1m};
            model.Premiums.Add(pmc);
            for (int age = model.Age; age < (term + model.Age); age++)
            {
                noOffBase++;
                prem = getPremiumRecord(model.ProductName, model.PlanName, model.PlanType, famComp, chronic,
                                        model.SumInsured, deductible, age, thisterm, "Base Premium " + thisterm);
                if (prem != null)
                {
                    pmc.BasePremium += prem.Amount;
                    pmc.premiumItems.Add(prem);
                }
                thisterm++;  
            }
            pmc.NetAmount = pmc.BasePremium;

            //Premium Loading 
            /*Co-Morbitity Load  */
            decimal coMorbitLoading = 0m;
            
            if (model.ProductName == "Platinum" && model.PlanType != "Family Floater")
            {
                if (model.Diabetes)
                {
                    coMorbitLoading = (model.Hypertension && model.Hyperlipidaemia && model.Asthma) ? 7050m : coMorbitLoading;
                    coMorbitLoading = (model.Hypertension && model.Hyperlipidaemia && !model.Asthma) ? 5150m : coMorbitLoading;
                    coMorbitLoading = (model.Hypertension && !model.Hyperlipidaemia && model.Asthma) ? 2780m : coMorbitLoading;
                    coMorbitLoading = (!model.Hypertension && model.Hyperlipidaemia && model.Asthma) ? 550m : coMorbitLoading;
                    coMorbitLoading = (model.Hypertension && !model.Hyperlipidaemia && !model.Asthma) ? 1050m : coMorbitLoading;
                    coMorbitLoading = (!model.Hypertension && !model.Hyperlipidaemia && model.Asthma) ? 1400m : coMorbitLoading;
                    coMorbitLoading = (!model.Hypertension && model.Hyperlipidaemia && !model.Asthma) ? 3600m : coMorbitLoading;
                }
                else if (model.Hypertension)
                {
                    coMorbitLoading = (model.Hyperlipidaemia && model.Asthma) ? 5250m : coMorbitLoading;
                    coMorbitLoading = (model.Hyperlipidaemia && !model.Asthma) ? 3350m : coMorbitLoading;
                    coMorbitLoading = (!model.Hyperlipidaemia && model.Asthma) ? 1150m : coMorbitLoading;
                }
                else if (model.Hyperlipidaemia)
                {
                    coMorbitLoading = (model.Asthma) ? 5650m : coMorbitLoading;
                }
            }
            if (coMorbitLoading > 0m)
            {
                //pmc.LoadingPerc += coMorbitLoading;
                //pmc.LoadingFactor *= (1m + coMorbitLoading);
                decimal loadAmount = coMorbitLoading * model.Term;
                pmc.NetAmount = pmc.BasePremium + (loadAmount);
                pmc.Info += string.Format("\nCo-Morbitity Loading  " + loadAmount);
            }

            //Premium Waiver on co-pay for Essential 
            if (model.PremiumWaiver && model.PlanName == "Essential")
            {
                pmc.LoadingFactor *= (1m + 0.25m);
                //pmc.LoadingPerc += 0.25m;
                pmc.Info += "\nPremium Waiver Loading 25%|";

            }
            //pmc.LoadingAmount = pmc.NetAmount * (1m - pmc.LoadingPerc);
            //pmc.NetAmount = pmc.NetAmount * pmc.LoadingAmount;

            //           pmc.NetAmount = pmc.BasePremium;
            /*Discounts*/
            //Term Discount 
            if (noOffBase > 1)
            {
                pmc.Info += "\n" + noOffBase + " year term|";
                if (noOffBase == 2)
                {
                    pmc.DiscountFactor *= (1m - 0.075m);
                    pmc.Info += "\nTerm discount 7.5%|";
                }
                if (noOffBase == 3)
                {
                    pmc.DiscountFactor *= (1m - 0.10m);
                    pmc.Info += "\nTerm discount 10%|";
                }
            }
            //Staff Discount 
            if (model.Staff)
            {
                pmc.DiscountFactor *= (1m -0.10m);
                pmc.Info += "\nStaff discount 10%|";
            }
            //pmc.DiscountAmount = pmc.NetAmount * (1m - pmc.DiscountPerc);
            //pmc.NetAmount = pmc.NetAmount * pmc.DiscountPerc;

            /* Relatives */
            //Zone 
            if (model.Zone > 1)
            {
                if (model.Zone == 2)
                {
                    pmc.RelativesFactor *= (1m-0.10m);
                    pmc.Info += "\nZone Relative 10%|";
                }
                if (model.Zone == 3)
                {
                    pmc.RelativesFactor *= (1m-0.25m);
                    pmc.Info += "\nZone Relative 25%|";
                }
            }
            //Room Type
            if (model.RoomType != 1)
            {
                //Not Single 
                if (model.PlanName == "Essential")
                {
                    if (model.RoomType == 2)//Shared 
                    {
                        pmc.RelativesFactor *= (1m-0.10m);
                        pmc.Info += "\nRoomType Relative 10%|";

                    }
                    if (model.RoomType == 3)//General/Economy 
                    {
                        pmc.RelativesFactor *= (1m-0.20m);
                        pmc.Info += "\nRoomType Relative 20%|";

                    }
                }
                if (model.PlanName == "Enhanced")
                {
                    if (model.RoomType == 2 || model.RoomType == 3)//Shared or General/Economy 
                    {
                        pmc.RelativesFactor *= (1m-0.10m);
                        pmc.Info += "\nRoomType Relative 10%|";

                    }
                    if (model.RoomType == 4)//any room 
                    {
                        pmc.RelativesFactor *= (1m+0.25m);
                        pmc.Info += "\nRoomType Loading Relative +25%|";

                    }

                }

            }

            pmc.TotalFactor = pmc.LoadingFactor * pmc.DiscountFactor * pmc.RelativesFactor;
            pmc.NetAmount = pmc.NetAmount * pmc.TotalFactor;
            //pmc.RelativesAmount = pmc.NetAmount * (1m - pmc.RelativesPerc);
            //pmc.NetAmount = pmc.NetAmount * pmc.RelativesPerc;

            //Hospital Cash 
            if (model.HospitalCash > 0)
            {
                pmc = new PremiumCalc { name = "Hospital Daily Cash" };
                model.Premiums.Add(pmc);
                famComp = (model.Gender == "Male") ? cSelf : (cSelf * 2);
                for (int age = model.Age; age < (model.Term + model.Age); age++)
                {

                    prem = getPremiumRecord("Optional Benefit: Hospital Daily Cash Benefit", "HospCash",
                                            "Optional", famComp, "",
                                            model.HospitalCash, 0, age, model.Term, "Hospital Daily Cash");

                    if (prem != null)
                    {
                        pmc.BasePremium += prem.Amount;
                        pmc.premiumItems.Add(prem);
                    }
                }
                pmc.NetAmount = pmc.BasePremium;

            }

            //OPD
            if (model.OPD > 0)
            {
                pmc = new PremiumCalc { name = "OPD Expenses" };
                model.Premiums.Add(pmc);
                prem = getPremiumRecord("Optional Benefit : OPD Expenses", "OPD",
                                        "Optional", cSelf, "",
                                        model.OPD, 0, 0, model.Term, "OPD Expenses");
                if (prem != null)
                {
                    pmc.NetAmount = prem.Amount * model.Term; //Multiply by Term
                    pmc.BasePremium = prem.Amount;
                    pmc.premiumItems.Add(prem);
                }

            }
            //Maternity
            if (model.Maternity)
            {
                pmc = new PremiumCalc { name = "Maternity Expenses" };
                model.Premiums.Add(pmc);

                for (int age = model.Age; age < (model.Term + model.Age); age++)
                {
                    prem = getPremiumRecord("Optional Benefit: Maternity Expenses", "Maternity",
                                        "Optional", (cSelf * 2), "",
                                        0, 0, age, model.Term, "Maternity Expenses");

                    if (prem != null)
                    {
                        pmc.BasePremium += (prem.Amount/3);
                        pmc.NetAmount = pmc.BasePremium;
                        pmc.premiumItems.Add(prem);
                    }
                }


           /*     
                if (prem != null)
                {
                    pmc.BasePremium = prem.Amount;
                    //Maternity has a 3 Year Premium
                    pmc.NetAmount = (pmc.BasePremium / 3) * noOffBase;
                    pmc.premiumItems.Add(prem);
                }
                */

            }
            //Add a Total Row 
            PremiumCalc totPrem = new PremiumCalc { name = "Total" };
            foreach (PremiumCalc pmc1 in model.Premiums)
            {
                totPrem.NetAmount += pmc1.NetAmount;
                totPrem.BasePremium += pmc1.BasePremium;
                /*totPrem.DiscountPerc += pmc1.DiscountPerc;
                totPrem.DiscountAmount += pmc1.DiscountAmount;
                totPrem.RelativesPerc += pmc1.RelativesPerc;
                totPrem.RelativesAmount += pmc1.RelativesAmount;
                totPrem.LoadingPerc += pmc1.LoadingPerc;
                totPrem.LoadingAmount += pmc1.LoadingAmount;*/
                totPrem.LoadingFactor = (pmc1.LoadingFactor > totPrem.LoadingFactor) ? pmc1.LoadingFactor : totPrem.LoadingFactor;
                totPrem.DiscountFactor = (pmc1.DiscountFactor > totPrem.DiscountFactor) ? pmc1.DiscountFactor : totPrem.DiscountFactor;
                totPrem.RelativesFactor = (pmc1.RelativesFactor > totPrem.RelativesFactor) ? pmc1.RelativesFactor : totPrem.RelativesFactor;
                totPrem.TotalFactor = (pmc1.TotalFactor > totPrem.TotalFactor) ? pmc1.TotalFactor : totPrem.TotalFactor;
                totPrem.Info += pmc1.Info;
            }
            model.Premiums.Add(totPrem);

            return model;
        }
        private PremiumItem getPremiumRecord(string productName, string planName, string planType, int famComp, string chronic,
                                               int sumInsured, int deductible, int age, int thisTerm, string name)
        {
            //int term = age - model.Age + 1;
            if (age > 80) age = 80;

            var premium = from p in _context.Premium
                          where
                          p.Age == age
                          && p.SumDeduct.Deductible == deductible
                          && p.SumDeduct.SumInsured == sumInsured
                          && p.SumDeduct.Product.ProductName == productName
                          && p.SumDeduct.Product.PlanName == planName
                          && p.SumDeduct.Product.ProductType == planType
                          && p.SumDeduct.Product.Condition == chronic
                          && p.SumDeduct.Product.FamilyCompositionId == famComp
                          select p;
            Premium prem = premium.SingleOrDefault();

            PremiumItem pm = null;


            if (prem == null) 
            {
                addMessage("No Premium found - " +
                    "||Age=" + age + ":" + 
                    "Deductible=" + deductible + ":" +
                    "SumInsured=" + sumInsured + ":" +
                    "ProductName=" + productName + ":" +
                    "PlanName=" + planName + ":" +
                    "ProductType=" + planType + ":" +
                    "Condition=" + chronic + ":" +
                    "FamilyCompositionId=" + famComp);
            }
            else
            {
                pm = new PremiumItem
                {
                    Name = name,
                    Amount = Math.Round(prem.Premium1, 0),
                    Info = "Prem Id=" + prem.PremiumId + ",Condition=" + chronic
                };
            }

            return pm;
        }

        private int getFamilyComposition(PremiumViewModel model)
        {
            int famComp = cSelf;
            if (model.Spouse) famComp += cSpouse;
            if (model.Father) famComp += cFather;
            if (model.Mother) famComp += cMother;
            if (model.FatherInLaw) famComp += cFatherInLaw;
            if (model.MotherInLaw) famComp += cMotherInLaw;
            int kids = model.Kids;
            if (kids > 3)
            {
                addMessage("Kids reduced to maximum 3");
                kids = 3;
            }
            if (kids < 0) kids = 0;
            famComp += (kids * cKids);
            addMessage("Family Comp = " + famComp);
            return famComp;
        }

        private void addMessage(String message)
        {
            if (Message == null)
            {
                Message = message;
            }
            else
            {
                Message += "\n" + message;
            }
        }





    }
}