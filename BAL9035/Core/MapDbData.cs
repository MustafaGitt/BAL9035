using BAL9035.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BAL9035.Core
{
    /* MAP SQL Results with the Model Objects*/
    public class MapDbData
    {
        // Map SQL Results to objects
        public Form9035 MapdbResult(DataTable dt)
        {
            try
            {
                Form9035 form9035 = new Form9035();
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["CaseSubType"].ToString() == "H-1B" || dataRow["CaseSubType"].ToString() == "E-3" || dataRow["CaseSubType"].ToString() == "H-1B1")
                    {
                        form9035.SectionA.A1 = dataRow["CaseSubType"].ToString();
                        if (!string.IsNullOrEmpty(dataRow["CaseSubType"].ToString()) && dataRow["CaseSubType"].ToString() == "H-1B1")
                        {
                            form9035.SectionA.A1Radio = citizenshipValue(dataRow);
                        }
                        form9035.SectionB.B1 = dataRow["Beneficiary Job Title"].ToString();
                        form9035.SectionB.B2 = dataRow["SocCode"].ToString();
                        form9035.SectionB.B3 = dataRow["SocOccupation"].ToString();

                        if (dataRow["BeginOfValidity"].ToString() != null && dataRow["BeginOfValidity"].ToString() != "")
                        {
                            DateTime BeginDate = Convert.ToDateTime(dataRow["BeginOfValidity"].ToString());
                            form9035.SectionB.B5 = BeginDate.ToString("MM/dd/yyyy");
                        }
                        if (dataRow["EndOfValidity"].ToString() != null && dataRow["EndOfValidity"].ToString() != "")
                        {
                            DateTime EndDate = Convert.ToDateTime(dataRow["EndOfValidity"].ToString());
                            form9035.SectionB.B6 = EndDate.ToString("MM/dd/yyyy");
                        }
                        if (!string.IsNullOrEmpty(dataRow["NumberOfPositions"].ToString()))
                        {
                            form9035.SectionB.B7 = dataRow["NumberOfPositions"].ToString();
                            form9035.SectionF.F1 = dataRow["NumberOfPositions"].ToString();
                        }
                        else
                        {
                            form9035.SectionB.B7 = "1";
                            form9035.SectionF.F1 = "1";
                        }
                        form9035.SectionF.F10CheckBox = true;
                        form9035.SectionF.F3CheckBox = true;
                        form9035.SectionF.F10From = dataRow["WageRangeLow"].ToString().Replace(",", "");
                        form9035.SectionF.F10To = dataRow["WageRangeHigh"].ToString().Replace(",", "");
                        form9035.SectionF.F11 = dataRow["PrevailingWage"].ToString().Replace(",", "");
                        bool entity = false;
                        bool company = false;
                        if (dataRow["Company H-1B Dependent"] != null && dataRow["Company H-1B Dependent"].ToString() != "")
                        {
                            company = Convert.ToBoolean(dataRow["Company H-1B Dependent"]);
                        }
                        if (dataRow["Entity H-1B Dependent"] != null && dataRow["Entity H-1B Dependent"].ToString() != "")
                        {
                            entity = Convert.ToBoolean(dataRow["Entity H-1B Dependent"]);
                        }
                        if (dataRow["Sponsoring Entity"].ToString().Equals("") && company == true)
                        {
                            form9035.SectionH.H1 = true;
                        }
                        else if (dataRow["Sponsoring Entity"].ToString().Equals("") && company == false)
                        {
                            form9035.SectionH.H1 = false;
                        }
                        else if (!dataRow["Sponsoring Entity"].ToString().Equals("") && entity == true)
                        {
                            form9035.SectionH.H1 = true;
                        }
                        else if (!dataRow["Sponsoring Entity"].ToString().Equals("") && entity == false)
                        {
                            form9035.SectionH.H1 = false;
                        }
                        form9035.SectionJ.J1 = dataRow["Signer Last Name"].ToString();
                        form9035.SectionJ.J2 = dataRow["Signer First Name"].ToString();
                        if (!dataRow["Signer Middle Name"].ToString().Equals("") && dataRow["Signer Middle Name"].ToString() != null)
                        {
                            form9035.SectionJ.J3 = dataRow["Signer Middle Name"].ToString().Substring(0, 1);
                        }
                        string assistantFirstInitial = dataRow["Assistant First Name"].ToString();
                        string assistantLastInitial = dataRow["Assistant Last Name"].ToString();
                        form9035.SectionJ.J4 = dataRow["Signer Job"].ToString() + " (" + dataRow["BALNumber"].ToString() + "/" + assistantFirstInitial[0].ToString() + assistantLastInitial[0].ToString() + ")";
                       
                        
                        form9035.SectionK.K1= dataRow["Assistant Last Name"].ToString();
                        form9035.SectionK.K2= dataRow["Assistant First Name"].ToString();
                        form9035.SectionK.K3= !string.IsNullOrEmpty(dataRow["Assistant Middle Name"].ToString())  ? dataRow["Assistant Middle Name"].ToString()[0].ToString() : null  ;
                        form9035.SectionK.K5 = dataRow["Assistant Email"].ToString();
                        break;
                    }
                }

                return form9035;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Map Lists Objects
        public Lists CreateLists(DataTable dt, string balno)
        {
            try
            {
                string AddressLine1 = "";
                string City = "";
                Lists lists = new Lists();
                lists.BALNumber = balno;
                List<string> caseSubType = new List<string>();
                List<string> parentCaseSubType = new List<string>();
                List<Location> locations = new List<Location>();
                int increment = 1;
                foreach (DataRow dataRow in dt.Rows)
                {
                    SectionFModal sectionFModal = new SectionFModal();
                    // Creating SectionFModal Lists
                    if (dataRow["PrevailingWageSource"].ToString().ToLower().Equals("oflc online data center"))
                    {
                        sectionFModal.F13 = true;
                        if (dataRow["WageLevel"].ToString() != null && dataRow["WageLevel"].ToString() != "")
                        {
                            switch (dataRow["WageLevel"].ToString())
                            {
                                case "1":
                                    sectionFModal.F13a = "I";
                                    break;
                                case "2":
                                    sectionFModal.F13a = "II";
                                    break;
                                case "3":
                                    sectionFModal.F13a = "III";
                                    break;
                                case "4":
                                    sectionFModal.F13a = "IV";
                                    break;
                                case "N/A":
                                    sectionFModal.F13a = "NA";
                                    break;
                                default:
                                    {
                                        sectionFModal.F13a = "";
                                        break;
                                    }
                            }
                        }


                        //Set By Defaule Values
                        sectionFModal.CollectionType = true;
                        sectionFModal.AreaBasedOn = true;
                        sectionFModal.RnDPosition = false;
                        sectionFModal.HCPosition = false;
                        sectionFModal.Per = "Year";
                    }
                    else if (dataRow["PrevailingWageSource"].ToString().ToLower().Equals("other"))
                    {
                        sectionFModal.F14 = true;
                        sectionFModal.F14a = "Other";
                        sectionFModal.F14b = dataRow["PrevailingWagePublishedYear"].ToString();
                        sectionFModal.F14c = dataRow["PrevailingWageOther"].ToString();
                        sectionFModal.F14d = dataRow["PrevailingWageOther"].ToString();
                    }
                    // Creating Case Subtypes
                    if (dataRow["CaseSubType"].ToString() != null && dataRow["CaseSubType"].ToString() != "")
                    {
                        caseSubType.Add(dataRow["CaseSubType"].ToString());
                    }
                    // Creating Parent Case Subtypes
                    if (dataRow["ParentCaseSubType"].ToString() != null && dataRow["ParentCaseSubType"].ToString() != "")
                    {
                        parentCaseSubType.Add(dataRow["ParentCaseSubType"].ToString());
                    }
                    // Creating Location Rows
                    if (!string.IsNullOrEmpty(dataRow["AddressLine1"].ToString()) || !string.IsNullOrEmpty(dataRow["City"].ToString()))
                    {
                        Location loc = new Location();
                        if (AddressLine1 != dataRow["AddressLine1"].ToString() || City != dataRow["City"].ToString())
                        {
                            loc.LocationId = increment;
                            increment++;
                        }
                        loc.Address1 = dataRow["AddressLine1"].ToString();
                        AddressLine1 = loc.Address1;
                        loc.Address2 = dataRow["AddressLine2"].ToString();
                        if (!string.IsNullOrEmpty(dataRow["AddressLine2"].ToString()) && !string.IsNullOrEmpty(dataRow["Suite"].ToString()))
                        {
                            loc.Address2 += ", " + dataRow["Suite"].ToString();
                        }
                        else if (string.IsNullOrEmpty(dataRow["AddressLine2"].ToString()) && !string.IsNullOrEmpty(dataRow["Suite"].ToString()))
                        {
                            loc.Address2 = dataRow["Suite"].ToString();
                        }
                        loc.City = dataRow["City"].ToString();
                        City = loc.City;
                        loc.State = dataRow["State"].ToString();
                        loc.PostalCode = dataRow["ZipCode"].ToString();

                        loc.LocWageFrom= dataRow["WageRangeLow"].ToString().Replace(",", "");
                        loc.LocWageTo= dataRow["WageRangeHigh"].ToString().Replace(",", "");

                        loc.FmodalObject = sectionFModal;
                        locations.Add(loc);
                    }
                }
                lists.caseSubTypes = caseSubType.Distinct().ToList();
                lists.parentCaseSubTypes = parentCaseSubType.Distinct().ToList();
                lists.LocationsList = locations.GroupBy(i => new { i.Address1, i.City }).Select(g => g.First()).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AssignValue(Form9035 form, List<string> pSubTypes)
        {
            if (form.SectionA.A1 == "H-1B" && form.SectionB.B7 == "1" && pSubTypes.Count > 0)
            {
                if ((pSubTypes.Contains("Change of Employer") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("Consular"))
           || (pSubTypes.Contains("Change of Employer") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("EOS"))
           || (pSubTypes.Contains("Change of Employer") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("Consular"))
           || (pSubTypes.Contains("Change of Employer") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("EOS"))
           )
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "1";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Counted"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("Consular"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("COS"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("EOS"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Exempt Employer"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("Consular"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("COS"))
                || (pSubTypes.Contains("Amend - Work Location Only") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("EOS"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Counted"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("Consular"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("COS"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("EOS"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Exempt Employer"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("Consular"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("COS"))
                || (pSubTypes.Contains("Amend") && pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("EOS"))
                )
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "1";
                }
                else if ((pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("Consular"))
                  || (pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("COS"))
                  || (pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("Consular"))
                  || (pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("COS"))
                  || (pSubTypes.Contains("Cap Subject (Master's Cap)"))
                  || (pSubTypes.Contains("Cap Subject (Regular Cap)"))
                   )
                {
                    form.SectionB.B7a = "1";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Cap Counted") && pSubTypes.Contains("EOS"))
                  || (pSubTypes.Contains("Cap Exempt Employer") && pSubTypes.Contains("EOS")))
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "1";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
            }

            if (form.SectionA.A1 == "E-3" && form.SectionB.B7 == "1" && pSubTypes.Count > 0)
            {
                if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Initial Filing")) || (pSubTypes.Contains("COS")))
                {
                    form.SectionB.B7a = "1";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Renewal")) || (pSubTypes.Contains("EOS")))
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "1";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Change of Employer")) || (pSubTypes.Contains("Change of Employer") && pSubTypes.Contains("EOS")))
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "1";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Amend")) || (pSubTypes.Contains("Amend") && pSubTypes.Contains("EOS")))
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "1";
                }
            }

            if (form.SectionA.A1 == "H-1B1" && form.SectionB.B7 == "1" && pSubTypes.Count > 0)
            {
                if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Initial")) || (pSubTypes.Contains("COS") && pSubTypes.Contains("Initial")))
                {
                    form.SectionB.B7a = "1";
                    form.SectionB.B7b = "0";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
                else if ((pSubTypes.Contains("Consular") && pSubTypes.Contains("Renewal")) || (pSubTypes.Contains("EOS") && pSubTypes.Contains("Renewal")))
                {
                    form.SectionB.B7a = "0";
                    form.SectionB.B7b = "1";
                    form.SectionB.B7c = "0";
                    form.SectionB.B7d = "0";
                    form.SectionB.B7e = "0";
                    form.SectionB.B7f = "0";
                }
            }
        }
        private string citizenshipValue(DataRow dataRow)
        {
            bool isChileExist = false;
            bool isSingaporeExist = false;
            string returnValue = "";
            if (dataRow["Citizenship 1"].ToString().ToLower() == "chile" || dataRow["Citizenship 2"].ToString().ToLower() == "chile" || dataRow["Citizenship 3"].ToString().ToLower() == "chile" || dataRow["Citizenship 4"].ToString().ToLower() == "chile" || dataRow["Citizenship 5"].ToString().ToLower() == "chile" ||
                dataRow["Citizenship 6"].ToString().ToLower() == "chile" || dataRow["Citizenship 7"].ToString().ToLower() == "chile" || dataRow["Citizenship 8"].ToString().ToLower() == "chile" || dataRow["Citizenship 9"].ToString().ToLower() == "chile" || dataRow["Citizenship 10"].ToString().ToLower() == "chile")
            {
                isChileExist = true;
            }
            if (dataRow["Citizenship 1"].ToString().ToLower() == "singapore" || dataRow["Citizenship 2"].ToString().ToLower() == "singapore" || dataRow["Citizenship 3"].ToString().ToLower() == "singapore" || dataRow["Citizenship 4"].ToString().ToLower() == "singapore" || dataRow["Citizenship 5"].ToString().ToLower() == "singapore" ||
                dataRow["Citizenship 6"].ToString().ToLower() == "singapore" || dataRow["Citizenship 7"].ToString().ToLower() == "singapore" || dataRow["Citizenship 8"].ToString().ToLower() == "singapore" || dataRow["Citizenship 9"].ToString().ToLower() == "singapore" || dataRow["Citizenship 10"].ToString().ToLower() == "singapore")
            {
                isSingaporeExist = true;
            }


            if(isChileExist == true && isSingaporeExist == false)
            {
                returnValue = "Chile";
            }
            else if (isChileExist == false && isSingaporeExist == true)
            {
                returnValue = "Singapore";
            }
            else
            {
                returnValue = string.Empty;
            }

            return returnValue;
        }

    }
}