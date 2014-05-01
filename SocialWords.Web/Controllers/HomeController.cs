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

namespace SocialWords.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(String word)
        {
            if (word == null)
            {
                ViewBag.Word = "syzygy";
                ViewBag.Title = "Do you know what #" + word + " means? #english #dictionary";
                ViewBag.Description = "the straight line configuration of 3 celestial bodies (as the sun and earth and moon) in a gravitational system";
            }
            else
            {
                ViewBag.Word = word;
                ViewBag.Title = "Share your search results";
                ViewBag.Description = "You can share the word definition or let other to search it";
            }
            ViewBag.Url = Request.Url.AbsoluteUri;  // needs to be set same as actual URL, probably helps FB to know how to cache, can test at https://developers.facebook.com/tools/debug/
            ViewBag.Image = "http://catalyst.lexicum.net/catalyst/facebook-story.png"; // just a placeholder, needs to be read dynamically. But what do we want to show as a facebook thumbnail to the home page?

            ViewBag.Audio = "http://mapto.ko64eto.com/po-zodia.mp3"; // just a placeholder

            return View();
        }
	}
}