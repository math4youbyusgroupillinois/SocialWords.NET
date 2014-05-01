/*  Copyright 2014 Lexicum Ltd. and Pearson plc
 
    This file is part of Pearson/Lexicum Social Words 

    Social Words is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Social Words is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Social Words.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

using Pearson.Lexicum.ApiClient;

namespace SocialWords.Web.Controllers
{
    public class DefinitionController : Controller
    {
        Random r = new Random(); // can be done centrally for the entire application

        //
        // GET: /Definition/
        //[Route("def/{id}")]
        public ActionResult Index(String id = null)
        {

            String word = id;
            if (word == null)
            {
                word = "syzygy";
            }
            LookupResult result = DictLookup.Get(word);

            String def = result.Definition.First().Text;
            String[] titles = new String[] {
                "Definition of the word “" + word + "”",
                "This is what we mean by “" + word + "”",
                "“" + word + "” means...",
                "“" + word + "” has the meaning...",
                "When we use “" + word + "” we mean...",
                "What is “" + word + "”?",
                "What does “" + word + "” mean?"
            };
            String[] descriptions = new String[] {
                "“" + word + "” is " + def,
                word + " - " + def,
                "Definition: " + def
            };

            ViewBag.Word = word;
            ViewBag.Description = descriptions[r.Next(descriptions.Length)];
            ViewBag.Title = titles[r.Next(titles.Length)];

            // todo: make sure name comes from dictionary (so doesn't contain funny characters
            WebPageBitmap.LoadBitmapSTA(word, Config.WebRoot);

            ViewBag.Image = Config.GetImagePath(word, ShareMode.Definition);
            ViewBag.Url = Request.Url.AbsoluteUri;  // needs to be set same as actual URL, probably helps FB to know how to cache, can test at https://developers.facebook.com/tools/debug/

            //ViewBag.Audio = "http://mapto.ko64eto.com/po-zodia.mp3"; // just a placeholder
            return View();
        }
	}
}