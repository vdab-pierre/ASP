using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Videotheek.Models;

namespace Videotheek.Services
{
    public class VideoService
    {
        public Klant GetKlant(string naam, int postcode)
        {
            using(var db = new VideoVerhuurEntities())
            {
                var klant = (from k in db.Klanten
                            where k.Naam.ToLower() == naam.ToLower() && k.PostCode==postcode
                            select k).FirstOrDefault();
                return klant;
            }
        }

        public List<Genre> GetGenres()
        {
            using(var db = new VideoVerhuurEntities())
            {
                var genreList = (from g in db.Genres
                                 select g).ToList();
                return genreList;
            }
        }

        public Film GetFilm(int id)
        {
            using(var db = new VideoVerhuurEntities())
            {
                var film = db.Films.Find(id);
                return film;
            }
        }

        public List<Film> GetFilms(int id)
        {
            using(var db = new VideoVerhuurEntities())
            {
                var filmList = (from f in db.Films
                                where f.GenreNr == id
                                select f).ToList();
                return filmList;
            }
        }

        public void BewaarVerhuring(Verhuur verhuur)
        {
            using(var db = new VideoVerhuurEntities())
            {
                var film = db.Films.Find(verhuur.BandNr);
                film.InVoorraad -= 1;
                film.UitVoorraad += 1;
                db.Verhuur.Add(verhuur);
                db.SaveChanges();
            }
        }
    }
}