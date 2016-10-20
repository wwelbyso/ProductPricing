using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ProductPricing.ViewModels;
using ProductPricing.BusinessLogic;
using ProductPricing.Models;

namespace ProductPricing.Controllers
{
    public class BulkTestController : Controller
    {
        private IHostingEnvironment hostingEnv;

/*        public BulkTestController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
*/
        private readonly ABHIPricingDBContext _context;

        public BulkTestController(ABHIPricingDBContext context, IHostingEnvironment env)
        {
            _context = context;
            this.hostingEnv = env;
        }


        public IActionResult Index()
        {
            ViewData["Message"] = "Default Index";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormFile testFile)
        {
            if(testFile == null)
            {
                ViewData["Message"] = "No File Selected";
                return View();
            }
            
            //List<BultTestViewModel> btvms = new List<BultTestViewModel>();
            BultTesterHeadViewModel bulkTester = new BultTesterHeadViewModel();
            PremiumCalculator pc = new PremiumCalculator(_context);
            

            String inLine;
            StreamReader fs = new StreamReader(testFile.OpenReadStream());
            int inx = 0;
            for(inx=0; (inLine = fs.ReadLine()) != null; inx++)
            {
                if (inx > 0)
                {
                    string[] fields = inLine.Split(',');

                    BultTestViewModel btvm = new BultTestViewModel
                    {
                        ID = fields[0],
                        ProductName = fields[1],
                        PlanName = fields[2],
                        PlanType = fields[5],
                        Condition = fields[4],
                        Gender = fields[6],
                        familyString = fields[19],
                        Age = Int32.Parse(fields[7]),
                        Channel = fields[11],
                        Term = Int32.Parse(fields[12]),
                        SumInsured = Int32.Parse(fields[3])
                    };
                    decimal de;
                    Decimal.TryParse(fields[18],out de);
                    btvm.inputPremium = de;

                    if (btvm.PlanType == "FF") btvm.PlanType = "Family Floater";

                    if (btvm.Gender == "M") btvm.Gender = "Male";
                    else if(btvm.Gender == "F") btvm.Gender = "Female";

                    string[] conditions = fields[4].Split('&');
                    for(int i = 0; conditions != null && i < conditions.Length; i++)
                    {
                        switch (conditions[i].Trim())
                        {
                            case "Diabetes":
                            case "Diabetic":
                                btvm.Diabetes = true;
                                break;
                            case "Hypertension":
                                btvm.Hypertension = true;
                                break;
                            case "Hyperlipidemia":
                            case "Hyperlipidaemia":
                                btvm.Hyperlipidaemia = true;
                                break;
                            case "Asthma":
                                btvm.Asthma = true;
                                break;
                        }
                    }

                    if (btvm.familyString != null && btvm.familyString.Length > 0)
                    {

                        string[] fams = btvm.familyString.Split('+');

                        bool selfFound = false;
                        int ffNo = 0;
                        for (int i = 0; fams != null && i < fams.Length; i++)
                        {
                            string sFam = fams[i].Trim().ToLower();
                            if (sFam.Length > 0 && sFam.Substring(0, 3) == "kid") { sFam = "kid"; }
                            switch (sFam)
                            {
                                case "self":
                                    selfFound = true;
                                    ffNo++;
                                    break;
                                case "spouse":
                                    if(selfFound) btvm.Spouse = true;
                                    ffNo++;
                                    break;
                                case "father":
                                    btvm.Father = true;
                                    ffNo++;
                                    break;
                                case "mother":
                                    btvm.Mother = true;
                                    ffNo++;
                                    break;
                                case "fatherinlaw":
                                case "father-in-law":
                                    btvm.FatherInLaw = true;
                                    ffNo++;
                                    break;
                                case "motherinlaw":
                                case "mother-in-law":
                                    btvm.MotherInLaw = true;
                                    ffNo++;
                                    break;
                                case "kid":
                                    btvm.Kids += 1;
                                    break;      
                            }
                        }
                        if (btvm.Kids > 0) ffNo++;
                        if (ffNo == 2 && ((btvm.Father && btvm.Mother) || (btvm.MotherInLaw && btvm.FatherInLaw)))
                        {
                            btvm.Spouse = false;
                            btvm.Father = false;
                            btvm.Mother = false;
                            btvm.FatherInLaw = false;
                            btvm.MotherInLaw = false;

                            btvm.Spouse = true;
                        }
                    }
                    

                    switch (fields[9].Trim())
                    {
                        case "I":
                            btvm.Zone = 1;
                            break;
                        case "II":
                            btvm.Zone = 2;
                            break;
                        case "III":
                            btvm.Zone = 3;
                            break;
                    }
                    switch (fields[10].Trim())
                    {
                        case "Single":
                            btvm.RoomType = 1;
                            break;
                        case "Any":
                            btvm.RoomType = 4;
                            break;
                        case "Shared":
                            btvm.RoomType = 2;
                            break;
                        case "Economy":
                            btvm.RoomType = 3;
                            break;
                    }
                    if(btvm.Channel == "Direct") { btvm.Staff = true; }
                    int tInt = 0;
                    Int32.TryParse(fields[13], out tInt);
                    btvm.Deductible = tInt;
                    Int32.TryParse(fields[14], out tInt);
                    btvm.OPD = tInt;
                    Int32.TryParse(fields[15], out tInt);
                    btvm.HospitalCash = tInt;
                    if (fields[16] == "Yes") btvm.Maternity = true;
                    if (fields[17] == "Yes") btvm.PremiumWaiver = true;

                    getASinglePremium(btvm, pc); 

                    bulkTester.TotalTests++;
                    if (btvm.TestPass) { bulkTester.TotalPass++; }else { bulkTester.TotalFail++; }

                    bulkTester.bulkTest.Add(btvm);
                    
                }
            }


            ViewData["Message"] = testFile.Name + ",Test Scenarios = " + inx + ",Length=" + testFile.Length;
            return View(bulkTester);
        }

        private void getASinglePremium(BultTestViewModel btvm, PremiumCalculator pc)
        {
            PremiumViewModel pvm = new PremiumViewModel();
            pvm.ProductName = btvm.ProductName;
            pvm.PlanName = btvm.PlanName;
            pvm.PlanType = btvm.PlanType;
            pvm.SumInsured = btvm.SumInsured;
            pvm.Term = btvm.Term;
            pvm.Age = btvm.Age;
            pvm.Gender = btvm.Gender;
            pvm.Diabetes = btvm.Diabetes;
            pvm.Hyperlipidaemia = btvm.Hyperlipidaemia;
            pvm.Hypertension = btvm.Hypertension;
            pvm.Asthma = btvm.Asthma;
            pvm.Spouse = btvm.Spouse;
            pvm.Father = btvm.Father;
            pvm.Mother = btvm.Mother;
            pvm.FatherInLaw = btvm.FatherInLaw;
            pvm.MotherInLaw = btvm.MotherInLaw;
            pvm.Kids = btvm.Kids;
            pvm.RoomType = btvm.RoomType;
            pvm.Zone = btvm.Zone;
            pvm.Channel = btvm.Channel;
            pvm.Staff = btvm.Staff;
            pvm.Deductible = btvm.Deductible;
            pvm.OPD = btvm.OPD;
            pvm.HospitalCash = btvm.HospitalCash;
            pvm.Maternity = btvm.Maternity;
            pvm.PremiumWaiver = btvm.PremiumWaiver;

            pc.calculatePremium(pvm);
            //PremiumCalc prem = null;
            foreach (PremiumCalc prem in pvm.Premiums){
                if(prem.name == "Total")
                {
                    btvm.TotalPremium = prem;
                }
            }
            btvm.Premiums = pvm.Premiums;
            btvm.TestPass = System.Math.Round(btvm.TotalPremium.NetAmount, 2) == System.Math.Round(btvm.inputPremium, 2);
        }
            
    }
}