using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductPricing.Models;
using ProductPricing.Controllers.Helpers;

namespace ProductPricing.Controllers
{
    public class ProductController : Controller
    {
        private readonly ABHIPricingDBContext _context;

        public ProductController(ABHIPricingDBContext context)
        {
            _context = context;    
        }

        // GET: Pricing
        public async Task<IActionResult> Index(
                string sortOrder,
                string currentFilter,
                string searchString,
                int? page)
        {
            if (!String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "product_asc";
            }

            ViewData["CurrentSort"] = sortOrder;

            //Set view data sort to default 
            ViewData["ProductSortParm"] = "product_asc";
            ViewData["PlanSortParm"] = "plansort_asc";
            ViewData["ProductTypeSortParm"] = "producttypetort_asc";
            ViewData["ConditionSortParm"] = "condition_asc";
            ViewData["FamilySortParm"] = "familysort_asc";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var products = from p in _context.Product
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString)
                                       || p.PlanName .Contains(searchString)
                                       || p.ProductType.Contains(searchString));
            }


            products = products
                .Include(p => p.FamilyComposition);


            switch (sortOrder)
            {
                case "product_asc":
                    products = products.OrderBy(product => product.ProductName);
                    ViewData["ProductSortParm"] = "product_desc";
                    break;
                case "product_desc":
                    products = products.OrderByDescending(product => product.ProductName);
                    ViewData["ProductSortParm"] = "product_asc";
                    break;
                case "plansort_asc":
                    products = products.OrderBy(product => product.PlanName);
                    ViewData["PlanSortParm"] = "plansort_desc";
                    break;
                case "plansort_desc":
                    products = products.OrderByDescending(product => product.PlanName);
                    ViewData["PlanSortParm"] = "plansort_asc";
                    break;
                case "producttypetort_asc":
                    products = products.OrderBy(product => product.ProductType);
                    ViewData["ProductTypeSortParm"] = "producttypetort_desc";
                    break;
                case "producttypetort_desc":
                    products = products.OrderByDescending(product => product.ProductType);
                    ViewData["ProductTypeSortParm"] = "producttypetort_asc";
                    break;
                case "condition_asc":
                    products = products.OrderBy(product => product.Condition);
                    ViewData["ConditionSortParm"] = "condition_desc";
                    break;
                case "condition_desc":
                    products = products.OrderByDescending(product => product.Condition);
                    ViewData["ConditionSortParm"] = "condition_asc";
                    break;
                case "familysort_asc":
                    products = products.OrderBy(product => product.FamilyCompositionId);
                    ViewData["FamilySortParm"] = "familysort_desc";
                    break;
                case "familysort_desc":
                    products = products.OrderByDescending(product => product.FamilyCompositionId);
                    ViewData["FamilySortParm"] = "familysort_asc";
                    break;
                default:
                    products = products.OrderBy(product => product.ProductId);
                    break;
            }

            int pageSize = 18;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Pricing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Product
                   .Include(p => p.FamilyComposition)
                   //.Include(p => p.SumDeduct)
                   //.OrderBy((SumDeduct sm) => sm.Deductible)
                   .AsNoTracking()
                   .SingleOrDefaultAsync(pr => pr.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var sumdeducts = await _context.SumDeduct
                .Where(pr => pr.ProductId == id)
                .OrderBy(s => s.SumInsured)
                .ThenBy(s => s.Deductible)
                .AsNoTracking()
                .ToListAsync();
                

            if (sumdeducts == null)
            {
                return NotFound();
            }

            product.SumDeduct = sumdeducts;
            return View(product);
        }

        // GET: Premium Details 
        public async Task<IActionResult> PremiumDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sumDedct = await _context.SumDeduct
                   //.Include(p => p.Premium)
                   .AsNoTracking()
                   .SingleOrDefaultAsync(pr => pr.SumDeductId == id);
            if (sumDedct == null)
            {
                return NotFound();
            }
            var premiums = await _context.Premium
                .Where(pr => pr.SumDeductId == id)
                .OrderBy(p => p.Age)
                .AsNoTracking()
                .ToListAsync();


            if (premiums == null)
            {
                return NotFound();
            }

            sumDedct.Premium = premiums;

            return View(sumDedct);
        }

        // GET: Premium Details 
        public async Task<IActionResult> PremiumRecord(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sumDedct = await _context.Premium
                   //.Include(p => p.Premium)
                   .AsNoTracking()
                   .OrderBy((Premium p) => p.Age)
                   .SingleOrDefaultAsync(pr => pr.SumDeductId == id);
            if (sumDedct == null)
            {
                return NotFound();
            }
            return View(sumDedct);
        }

        // GET: Pricing/Create
        public IActionResult Create()
        {
            return View();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
