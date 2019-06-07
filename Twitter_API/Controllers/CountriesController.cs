using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twitter_API.Models;
using Twitter_API.ViewModels;

namespace Twitter_API.Controllers
{
    public class CountriesController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        CountriesController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Countries/GetAll")]
        public List<CountriesVM> GetCountries()
        {
            List<CountriesVM> lista = db.Countries.Select(x => new CountriesVM
            {
                CountryID = x.CountryID,
                CountryName = x.CountryName
            }).ToList();

            return lista;
        }
    }
}
