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

using System.Threading;
using System.Threading.Tasks;

namespace SocialWords.Web.Controllers
{
    public class QuestionController : Controller
    {
        Random r = new Random(); // can be done centrally for the entire application
        //
        // GET: /Question/
        public ActionResult Index(String id)
        {
            String word = id;
            String[] titles = new String[] {
                "Can you guess what “" + word + "” means?",
                "Do you know what “" + word + "” means?",
                "What do you think “" + word + "” means?",
                "Do you really know what “" + word + "” is?",
                "What is “" + word + "”?"
            };
            String[] descriptions = new String[] {
                "Write your definition of the word “" + word + "” in a comment below. If you want to read more about it, or if you want to check your answer, click on the link to see the detailed description of the word.",
                "Write your interpretation of “" + word + "” in a comment if you want to test yourself. Open this link to see the answer."
            };

            ViewBag.Word = word;
            ViewBag.Description = descriptions[r.Next(descriptions.Length)];
            ViewBag.Title = titles[r.Next(titles.Length)];

            // todo: make sure name comes from dictionary (so doesn't contain funny characters
            WebPageBitmap.LoadBitmapSTA(word, Config.WebRoot);
            ViewBag.Image = Config.GetImagePath(word, ShareMode.Question);
            ViewBag.Url = Request.Url.AbsoluteUri;  // needs to be set same as actual URL, probably helps FB to know how to cache, can test at https://developers.facebook.com/tools/debug/

            ViewBag.Audio = "http://mapto.ko64eto.com/po-zodia.mp3"; // just a placeholder
            return View();
        }

        public ActionResult Test(String word)
        {
            Thread thread = new Thread(() => WebPageBitmap.InitBitmap("wtest4", ShareMode.Question, ""));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return View();
        }

	}
}