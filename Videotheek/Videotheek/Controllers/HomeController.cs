using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videotheek.Models;
using Videotheek.Services;

namespace Videotheek.Controllers
{
    public class HomeController : Controller
    {
        private VideoService db = new VideoService();   
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }

        public ActionResult Aanmelden(LoginViewModel form)
        {
                Klant klant = db.GetKlant(form.Familienaam, form.Postcode);
                if (klant != null)
                {
                    Session["Klant"] = klant;
                    return View(klant);
                }         
            return View();
        }
      
        public ActionResult Verhuringen()
        {
            var genreList = db.GetGenres();
            return View(genreList);
        }

        public ActionResult Details(int id, string naam)
        {
            var filmList = db.GetFilms(id);
            ViewBag.Naam = naam;
            return View(filmList);
        }

        public ActionResult Huren(int id)
        {
            var gekozenFilm = db.GetFilm(id);
            Session[id.ToString()] = gekozenFilm;
            return RedirectToAction("Winkelmandje", "Home");
        }

        public ActionResult Winkelmandje()
        {
            List<Film> mandjeFilms = new List<Film>();
            foreach(string nummer in Session)
            {
                int bandNr;
                if(int.TryParse(nummer, out bandNr))
                {
                    var gekozenFilm = db.GetFilm(bandNr);
                    mandjeFilms.Add(gekozenFilm);
                }
            }
            Session["gekozenFilms"] = mandjeFilms;
            return View(mandjeFilms);
        }

        public ActionResult Verwijderen(int id)
        {
            var gekozenFilm = db.GetFilm(id);
            return View(gekozenFilm);
        }
        public ActionResult Verwijdering(int id)
        {
            Session.Remove(id.ToString());
            return RedirectToAction("winkelmandje", "Home");
        }

        public ActionResult Afrekenen()
        {
            decimal totaal = 0;
            List<Film> teHurenFilms = (List<Film>)Session["gekozenFilms"];
            Klant klant = (Klant)Session["Klant"];
            foreach(var film in teHurenFilms)
            {
                Verhuur verhuur = new Verhuur();
                verhuur.BandNr = film.BandNr;
                verhuur.KlantNr = klant.KlantNr;
                verhuur.VerhuurDatum = DateTime.Today;
                totaal += film.Prijs;
                db.BewaarVerhuring(verhuur);
            }
            ViewBag.klant = klant;
            ViewBag.totaal = totaal;
            Session.Clear();
            return View(teHurenFilms);
        }
    }
}