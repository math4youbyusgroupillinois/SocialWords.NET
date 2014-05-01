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

using System.Web.Configuration;
//using System.Configuration;

namespace SocialWords.Web
{
    public enum ShareMode
    {
        Question,
        Definition
    }
    public class Config
    {
        public static String WebRoot
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                    System.Web.HttpContext.Current.Request.ApplicationPath;
            }
        }

        public static String GetImagePath(String word, ShareMode mode)
        {
            return Config.WebRoot + "img/" + WebPageBitmap.GetFilename(word, ShareMode.Question);
        }

        public static String SaveFolder
        {
            get
            {
                String value = WebConfigurationManager.AppSettings["SaveFolder"];
                return value != null ? value : String.Empty;
            }
        }
        public static String ApiKey
        {
            get
            {
                String value = WebConfigurationManager.AppSettings["PearsonApiKey"];
                return value != null ? value : String.Empty;
            }
        }
        public static Boolean TestMode
        {
            get
            {
                String value = WebConfigurationManager.AppSettings["TestMode"];
                Boolean retVal = false;
                if (!String.IsNullOrEmpty(value))
                    Boolean.TryParse(value, out retVal);
                return retVal;
            }
        }
    }
}