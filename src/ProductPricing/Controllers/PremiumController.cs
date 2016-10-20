using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductPricing.Models;
using ProductPricing.Controllers.Helpers;
using ProductPricing.ViewModels;
using ProductPricing.BusinessLogic;

namespace ProductPricing.Controllers
{
    public class PremiumController : Controller
    {


        private readonly ABHIPricingDBContext _context;

        public PremiumController(ABHIPricingDBContext context)
        {
            _context = context;    
        }

        public IActionResult Index()
        {
            var model = new PremiumViewModel();
            model.ProductName = "Gold";
            model.PlanName = "Essential";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Pricing
        public IActionResult Index(PremiumViewModel model)
        {
            PremiumCalculator pc = new PremiumCalculator(_context);
            return View(pc.calculatePremium(model));
        }
    }
}
