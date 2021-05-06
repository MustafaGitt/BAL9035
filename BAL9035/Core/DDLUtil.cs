using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAL9035.Core
{    
    /*
     * Contains all the Drop Down Population Methods
     */
    public static class DDLUtil
    {
        // Returns the all the State Options
        public static IEnumerable<SelectListItem> GetState(object selectedValue)
        {
            if (selectedValue == null)
            {
                selectedValue = "";
            }
            return new List<SelectListItem>
        {
            new SelectListItem{ Text="-- SELECT --", Value = "0", Selected = "0" == selectedValue.ToString()},
            new SelectListItem{ Text="ALABAMA", Value ="Alabama", Selected ="Alabama" == selectedValue.ToString()},
            new SelectListItem{ Text="ALASKA", Value ="Alaska", Selected ="Alaska" == selectedValue.ToString()},
            new SelectListItem{ Text="ARIZONA", Value ="Arizona", Selected ="Arizona" == selectedValue.ToString()},
            new SelectListItem{ Text="ARKANSAS", Value ="Arkansas", Selected ="Arkansas" == selectedValue.ToString()},
            new SelectListItem{ Text="CALIFORNIA", Value ="California", Selected ="California" == selectedValue.ToString()},
            new SelectListItem{ Text="COLORADO", Value ="Colorado", Selected ="Colorado" == selectedValue.ToString()},
            new SelectListItem{ Text="CONNECTICUT", Value ="Connecticut", Selected ="Connecticut" == selectedValue.ToString()},
            new SelectListItem{ Text="DISTRICT OF COLUMBIA", Value ="DC", Selected ="DC" == selectedValue.ToString()},
            new SelectListItem{ Text="DELAWARE", Value ="Delaware", Selected ="Delaware" == selectedValue.ToString()},
            new SelectListItem{ Text="FLORIDA", Value ="Florida", Selected ="Florida" == selectedValue.ToString()},
            new SelectListItem{ Text="GEORGIA", Value ="Georgia", Selected ="Georgia" == selectedValue.ToString()},
            new SelectListItem{ Text="HAWAII", Value ="Hawaii", Selected ="Hawaii" == selectedValue.ToString()},
            new SelectListItem{ Text="IDAHO", Value ="Idaho", Selected ="Idaho" == selectedValue.ToString()},
            new SelectListItem{ Text="ILLINOIS", Value ="Illinois", Selected ="Illinois" == selectedValue.ToString()},
            new SelectListItem{ Text="INDIANA", Value ="Indiana", Selected ="Indiana" == selectedValue.ToString()},
            new SelectListItem{ Text="IOWA", Value ="Iowa", Selected ="Iowa" == selectedValue.ToString()},
            new SelectListItem{ Text="KANSAS", Value ="Kansas", Selected ="Kansas" == selectedValue.ToString()},
            new SelectListItem{ Text="KENTUCKY", Value ="Kentucky", Selected ="Kentucky" == selectedValue.ToString()},
            new SelectListItem{ Text="LOUISIANA", Value ="Louisiana", Selected ="Louisiana" == selectedValue.ToString()},
            new SelectListItem{ Text="MAINE", Value ="Maine", Selected ="Maine" == selectedValue.ToString()},
            new SelectListItem{ Text="MARYLAND", Value ="Maryland", Selected ="Maryland" == selectedValue.ToString()},
            new SelectListItem{ Text="MASSACHUSETTS", Value ="Massachusetts", Selected ="Massachusetts" == selectedValue.ToString()},
            new SelectListItem{ Text="MICHIGAN", Value ="Michigan", Selected ="Michigan" == selectedValue.ToString()},
            new SelectListItem{ Text="MINNESOTA", Value ="Minnesota", Selected ="Minnesota" == selectedValue.ToString()},
            new SelectListItem{ Text="MISSISSIPPI", Value ="Mississippi", Selected ="Mississippi" == selectedValue.ToString()},
            new SelectListItem{ Text="MISSOURI", Value ="Missouri", Selected ="Missouri" == selectedValue.ToString()},
            new SelectListItem{ Text="MONTANA", Value ="Montana", Selected ="Montana" == selectedValue.ToString()},
            new SelectListItem{ Text="NEBRASKA", Value ="Nebraska", Selected ="Nebraska" == selectedValue.ToString()},
            new SelectListItem{ Text="NEVADA", Value ="Nevada", Selected ="Nevada" == selectedValue.ToString()},
            new SelectListItem{ Text="NEW HAMPSHIRE", Value ="New Hampshire", Selected ="New Hampshire" == selectedValue.ToString()},
            new SelectListItem{ Text="NEW JERSEY", Value ="New Jersey", Selected ="New Jersey" == selectedValue.ToString()},
            new SelectListItem{ Text="NEW MEXICO", Value ="New Mexico", Selected ="New Mexico" == selectedValue.ToString()},
            new SelectListItem{ Text="NEW YORK", Value ="New York", Selected ="New York" == selectedValue.ToString()},
            new SelectListItem{ Text="NORTH CAROLINA", Value ="North Carolina", Selected ="North Carolina" == selectedValue.ToString()},
            new SelectListItem{ Text="NORTH DAKOTA", Value ="North Dakota", Selected ="North Dakota" == selectedValue.ToString()},
            new SelectListItem{ Text="NORTHERN MARIANA ISLANDS", Value ="NORTHERN MARIANA ISLANDS", Selected ="NORTHERN MARIANA ISLANDS" == selectedValue.ToString()},
            new SelectListItem{ Text="OHIO", Value ="Ohio", Selected ="Ohio" == selectedValue.ToString()},
            new SelectListItem{ Text="OKLAHOMA", Value ="Oklahoma", Selected ="Oklahoma" == selectedValue.ToString()},
            new SelectListItem{ Text="OREGON", Value ="Oregon", Selected ="Oregon" == selectedValue.ToString()},
            new SelectListItem{ Text="PENNSYLVANIA", Value ="Pennsylvania", Selected ="Pennsylvania" == selectedValue.ToString()},
            new SelectListItem{ Text="PUERTO RICO", Value ="Puerto Rico", Selected ="Puerto Rico" == selectedValue.ToString()},
            new SelectListItem{ Text="RHODE ISLAND", Value ="Rhode Island", Selected ="Rhode Island" == selectedValue.ToString()},
            new SelectListItem{ Text="SOUTH CAROLINA", Value ="South Carolina", Selected ="South Carolina" == selectedValue.ToString()},
            new SelectListItem{ Text="SOUTH DAKOTA", Value ="South Dakota", Selected ="South Dakota" == selectedValue.ToString()},
            new SelectListItem{ Text="TENNESSEE", Value ="Tennessee", Selected ="Tennessee" == selectedValue.ToString()},
            new SelectListItem{ Text="TEXAS", Value ="Texas", Selected ="Texas" == selectedValue.ToString()},
            new SelectListItem{ Text="UTAH", Value ="Utah", Selected ="Utah" == selectedValue.ToString()},
            new SelectListItem{ Text="VERMONT", Value ="Vermont", Selected ="Vermont" == selectedValue.ToString()},
            new SelectListItem{ Text="VIRGINIA", Value ="Virginia", Selected ="Virginia" == selectedValue.ToString()},
            new SelectListItem{ Text="WASHINGTON", Value ="Washington", Selected ="Washington" == selectedValue.ToString()},
            new SelectListItem{ Text="WEST VIRGINIA", Value ="West Virginia", Selected ="West Virginia" == selectedValue.ToString()},
            new SelectListItem{ Text="WISCONSIN", Value ="Wisconsin", Selected ="Wisconsin" == selectedValue.ToString()},
            new SelectListItem{ Text="WYOMING", Value ="Wyoming", Selected ="Wyoming" == selectedValue.ToString()},
        };

        }
        // Returns the all the Year Options
        public static IEnumerable<SelectListItem> GetYear(object selectedValue)
        {
            if (selectedValue == null)
            {
                selectedValue = "";
            }
            List<SelectListItem> sl = new List<SelectListItem>();
            sl.Add(new SelectListItem { Text = "-- SELECT --", Value = "0", Selected = "0" == selectedValue.ToString() });
            int getYear = Convert.ToInt32(DateTime.Now.Year);
            getYear = getYear - 2;
            for (int i = 0; i < 3; i++)
            {
                sl.Add(new SelectListItem { Text = getYear.ToString(), Value = getYear.ToString(), Selected = getYear.ToString() == selectedValue.ToString() });
                getYear++;

            }
            return sl;
            //return new List<SelectListItem>
            //{
            //    new SelectListItem{ Text="-- SELECT --", Value = "0", Selected = "0" == selectedValue.ToString()},

            //    new SelectListItem{ Text="2018", Value = "2018", Selected = "2018" == selectedValue.ToString()},
            //    new SelectListItem{ Text="2019", Value = "2019", Selected = "2019" == selectedValue.ToString()},
            //    new SelectListItem{ Text="2020", Value = "2020", Selected = "2020" == selectedValue.ToString()},

            //};
        }
        // Returns the all the Per Options
        public static IEnumerable<SelectListItem> GetPer(object selectedValue)
        {
            if (selectedValue == null)
            {
                selectedValue = "";
            }
            return new List<SelectListItem>
        {
            new SelectListItem{ Text="-- SELECT --", Value = "0", Selected = "0" == selectedValue.ToString()},
            new SelectListItem{ Text="Hour", Value = "Hour", Selected = "Hour" == selectedValue.ToString()},
            new SelectListItem{ Text="Week", Value = "Week", Selected = "Week" == selectedValue.ToString()},
            new SelectListItem{ Text="Bi-Weekly", Value = "Bi-Weekly", Selected = "Bi-Weekly" == selectedValue.ToString()},
            new SelectListItem{ Text="Month", Value = "Month", Selected = "Month" == selectedValue.ToString()},
            new SelectListItem{ Text="Year", Value = "Year", Selected = "Year" == selectedValue.ToString()},

        };
        }
    }
}