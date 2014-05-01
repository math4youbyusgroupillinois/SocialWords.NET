using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using API = Pearson.Lexicum.ApiClient;

namespace SocialWords.LookupAPI.Controllers
{
    public class LookupController : ApiController
    {

        public API.LookupResult Get(String id)
        {
            return API.DictLookup.Get(id);
        }
    }
}
