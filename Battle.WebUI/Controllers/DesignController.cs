using Battle.Domain.Concrete;
using Battle.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battle.Domain.Abstract;
using WebMatrix.WebData;

namespace Battle.WebUI.Controllers
{
    [Authorize(Roles = "Gamer")]
    public class DesignController : Controller
    {
        private readonly IDesignRepository designRepository;
        private readonly IGamerRepository gamerRepository;

        public DesignController(IDesignRepository designRepo, IGamerRepository gamerRepo) {
            designRepository = designRepo;
            gamerRepository = gamerRepo;
        }

        public ActionResult CreateDesign()
        {
            return View(new Design());
        }

        [HttpPost]
        public ActionResult CreateDesign(Design design)
        {
            if (ModelState.IsValid)
            {
                design.UserId = WebSecurity.GetUserId(User.Identity.Name);
                design.weight = design.shield + design.guns * design.caliber;

                Gamer gamer = gamerRepository.Gamers.FirstOrDefault(x => x.UserId == design.UserId);
                design.ridgamer = gamer.rid;

                designRepository.SaveDesign(design);

                ModelState.Clear();
                return View(new Design());
            }
            else
            {
                return View(design);
            }
        }


    }
}
