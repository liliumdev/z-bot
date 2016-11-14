// TO-DO:
//  - Make sure it gets a job properly
// made in the 1930s ;)

//#define FIREFOX
#define IE
#define DISABLE_IMAGES
#define AUTO_CAPTCHA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Security;
using System.Runtime.InteropServices;
using System.Media;
using System.Net;
using System.Threading;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using WatiN.Core;
using mshtml;

namespace Z_Bot
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public ArrayList AccountUsernames = new ArrayList();
        public ArrayList AccountPasswords = new ArrayList();
        public ArrayList ProxyList = new ArrayList();
#if IE
        WatiN.Core.IE ie;
#endif
#if FIREFOX
        WatiN.Core.FireFox ie;
#endif
        public string accountID = "";
        public Settings settings;
        int started = 0;
        public int gotCaptcha = 0;
        public Currency[] accountBank;
        public double accountWellness = 0.00;
        TextWriter logFile;
        Decaptcher decaptcha = new Decaptcher();
        string thisLauncherVersion = "1.30";

        public Form1()
        {
            InitializeComponent();
            //WatiN.Core.Settings.MakeNewIeInstanceVisible = false;
            LoadAccounts();
            LoadProxies();
            logFile = new StreamWriter("log.txt");
            statusBox.Text = "Welcome to Z-Bot " + thisLauncherVersion + " !\r\n\r\n";
            LoadSettingsFromFile();
            string zlauncherversion = GetSource("http://mafioso.nihplod.com/zlauncherversion.txt");
            if(zlauncherversion != thisLauncherVersion)
            {
                MessageBox.Show("Please download the newest launcher from http://mafioso.nihplod.com/zbot ! You're using an outdated version!", "Z-Bot Update Warning");
                System.Environment.Exit(0);
            }
        }

        // Starts the multi account loop
        private void Bot()
        {
            int p = 0;

            for (int i = 0; i < AccountUsernames.Count; i++)
            {
                try
                {
                    if (gotCaptcha == 1)
                    {  
#if IE
                        ResetIE();
#endif
                        gotCaptcha = 0;
                    }

                    string ipBlock = "";

                    // Renew IP using modem
                    if (settings.renewIpType == 1)
                    {
                        Status("Renewing IP ...");

                        string oldip = GetMyIP();

                        RenewIPModem();

                        if(settings.modemType == "Thomson TG782")
                            ie.GoTo("http://mafioso.nihplod.com/ip.php");

                        if (settings.modemType != "Thomson TG782")
                        {
                            ie.Refresh();
                            System.Threading.Thread.Sleep(200);
                            ie.Refresh();
                        }

                        string newip = GetMyIP();

                        if (oldip == newip)
                        {
                            DialogResult result = MessageBox.Show("IP before and after renewing is the same! Please check the IP renewing settings.\r\nDo you want Z-Bot to continue?", "Z-Bot Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if (result == DialogResult.No)
                            {
                                ie.Close();
                                System.Environment.Exit(0);
                            }
                        }

                        ipBlock = " [" + oldip + " -> " + newip + "]";
                    }

                    // Renew IP using connection
                    if (settings.renewIpType == 2)
                    {
                        Status("Renewing IP ...");

                        string oldip = GetMyIP();

                        RenewIPConnection();

                        ie.Refresh();
                        System.Threading.Thread.Sleep(1500);
                        ie.Refresh();

                        string newip = GetMyIP();

                        if(oldip == newip)
                        {
                            DialogResult result = MessageBox.Show("IP before and after renewing is the same! Please check the IP renewing settings.\r\nDo you want Z-Bot to continue?", "Z-Bot Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if (result == DialogResult.No)
                            {
                                ie.Close();
                                System.Environment.Exit(0);
                            }
                        }

                        ipBlock = " [" + oldip + " -> " + newip + "]";
                    }

    #if ACO_BRACO
    #else
                    // Renew IP using hotspot
                    if (settings.renewIpType == 3)
                    {
                        Status("Renewing IP ...");

                        // Reconnect
                        settings.hotSpotURL.Replace("confirm_disconnect", "connect");
                        settings.hotSpotURL.Replace("disconnect", "connect");
                        ie.GoTo(settings.hotSpotURL);
                        while (ie.Url == settings.hotSpotURL)
                        {
                            System.Threading.Thread.Sleep(20);
                        }
                        // Now, let's check do we have to click Connect
                        if (ie.Url != "http://www.mefeedia.com/anchorfree")
                        {
                            ie.Div(Find.ById("start")).Click();
                        }
                    }
    #endif
                    // Renew IP using proxy
                    if(settings.renewIpType == 4)
                    {
                        RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                        registry.SetValue("ProxyEnable", 1);
                        registry.SetValue("ProxyServer", ProxyList[p].ToString());
                        if(p >= ProxyList.Count) p = 0;
                        else if(p < ProxyList.Count) p++;
                        ResetIE();
                    }

                    Status("------- [" + AccountUsernames[i].ToString() + "] " + ipBlock + " [" + (i + 1).ToString() + "/" + AccountUsernames.Count + "] -------");

                    Login(AccountUsernames[i].ToString(), AccountPasswords[i].ToString());
                    CaptchaCheck();
                    if (!LoggedIn()) continue;
                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // First let's check currencies
                    accountBank = CheckCurrencies();
                    string currencies = "[";
                    for (int g = 0; g < accountBank.Length; g++)
                    {
                        if (g != accountBank.Length - 1)
                            currencies += accountBank[g].name + " : " + accountBank[g].amount + "] [";
                        else
                            currencies += accountBank[g].name + " : " + accountBank[g].amount + "]";
                    }
                    Status(currencies);
                    
                    // Buy stuff
                    if (settings.buy)
                    {
                        Buy();
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Get a job if possible
                    if (settings.getJob)
                    {
                        GetAJob();
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Eat before work
                    if (settings.eatBeforeWork)
                    {
                        Eat(settings.eatBeforeWorkHowMuch);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Work
                    if (settings.work)
                    {
                        Work(settings.workBooster);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Train
                    if (settings.train)
                    {
                        Train(settings.trainBooster);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Eat after train
                    if (settings.eatAfterTrain)
                    {
                        ie.GoTo("http://www.erepublik.com");
                        Eat(1);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Get reward
                    if (settings.work && settings.train)
                    {
                        try
                        {
                            ie.GoTo("http://www.erepublik.com/en");
                            if (ie.Link(Find.ByTitle("Get reward")).Exists)
                            {
                                ie.Link(Find.ByTitle("Get reward")).Click();
                                System.Threading.Thread.Sleep(4000);
                                Status("Got reward.");
                            }
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            continue;
                        }
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Fight
                    if (settings.fight)
                    {
                        Fight(settings.battleId.ToString(), settings.fightTimes);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Vote
                    if (settings.vote)
                    {
                        Vote(settings.voteId.ToString());
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Subscribe to article's author
                    if (settings.sub)
                    {
                        Subscribe();
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }

                    // Comment
                    if (settings.comment)
                    {
                        Comment();            
                    }

                    // Donate
                    if(settings.donate)
                    {
                        Donate(settings.donateId, settings.donateCurrency, settings.donateHowMuch);
                    }

                    if (gotCaptcha == 1) { ResetIE(); gotCaptcha = 0; continue; }                               

                    Status("Logging out from [" + AccountUsernames[i].ToString() + "]");
                    Logout();
                    Status("------------------------------------------------------------------");
    #if IE
                    ResetIE();
    #endif
    #if FIREFOX
                    ie = new FireFox("http://www.erepublik.com");
    #endif
                }
                catch(Exception e)
                {
                    Status("Encountered an error at account [" + AccountUsernames[i] + "] : \r\n" + e.ToString() + "\r\n\r\n");
                    ResetIE();
                    continue;
                }
            }

            if (settings.modemType != "Thomson TG782")
                ie.Close();

            // Disable proxy
            if (settings.renewIpType == 4)
            {
                RegistryKey registry2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                registry2.SetValue("ProxyEnable", 0);
            }

            Status("Renewing IP one last time");

            // Renew IP using modem
            if (settings.renewIpType == 1)
            {
                RenewIPModem();
            }

            // Renew IP using connection
            if (settings.renewIpType == 2)
            {                   
                RenewIPConnection();
            }

            if (settings.modemType == "Thomson TG782")
                ie.Close();

#if DISABLE_IMAGES
            RegistryKey ieKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main");
            ieKey.SetValue("Display Inline Images", "yes");
#endif
            Status("\r\nEverything done! If you want to start again, please press Start again.");
#if FIREFOX
            ie.Close();
#endif

            EnableStart();

            logFile.Close();
        }

        // Login
        private void Login(string username, string password)
        {
            Status("Logging in ...");
            ie.GoTo("http://www.erepublik.com/en");
            if(ie.TextField(Find.ById("citizen_name")).Exists)
            {
                ie.TextField(Find.ById("citizen_name")).TypeText(username);
                ie.TextField(Find.ById("citizen_password")).TypeText(password);
                ie.Button(Find.ByValue("Login")).Click();
            }

            //if (!LoggedIn()) CaptchaCheck();
            if (ie.Url == "http://www.erepublik.com/en/login") CaptchaCheck();
            if (gotCaptcha != 1)
            {
                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Show);
                if (!LoggedIn()) MessageBox.Show("Please login the account manually and click OK. It's possible that the username and/or the password are wrong.", "Z-Bot Warning");
                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                
                // Get this account ID
                string src = ie.Div(Find.ByClass("avatarholder")).InnerHtml;
                Match match = Regex.Match(src, @"/en/citizen/profile/([0-9]*)", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    accountID = match.Groups[1].Value;
                }

                //accountWellness = Convert.ToDouble(ie.Element(Find.ById("wellnessBar")).Text);
            }
            else
            {
            }
        }

        // Log out
        private void Logout()
        {
            ie.GoTo("http://www.erepublik.com/en/logout");
        }

        private bool CaptchaCheck()
        {
            string source = ie.Html;
            int findcaptcha = source.IndexOf("recaptcha_response_field");
            if (findcaptcha != -1)
            {
                SoundPlayer simpleSound = new SoundPlayer("captcha.wav");
                simpleSound.Play();
                Status("Captcha appeared ...");
                if(settings.skipAccountsWithCaptcha)
                {
                    Status("Skipping this account ...");
                    gotCaptcha = 1;
                    return false;
                }
                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Show);
#if AUTO_CAPTCHA
                // http://www.google.com/recaptcha/api/image?c=03AHJ_VusQCNRzW5WyktdS6GU_4k4jOs7LY1cGxp0WxdyV3rhLRJa1frL0FJC3ykF0Vhf1E0wffOG_pw-lRHND13f4aXTF9rt7CgzpbHbv-tjO5ILueMHelnPA-Q3ifOhIQOwAw-gtnHAH-wBZqIWDiWQnvTxv-x1tDQ
                ImageCollection images = ie.Images;
                string captchaImageUrl = "";
                for (int i = 0; i < images.Count; i++)
                {
                    if (images[i].Src.StartsWith("http://www.google.com/recaptcha/api/image?c="))
                    {
                        captchaImageUrl = images[i].Src;
                    }
                }

                Status("Solving captcha ... please be patient.");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(captchaImageUrl));
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                using (FileStream fs = System.IO.File.OpenWrite("captcha.jpg"))
                {
                    byte[] bytes = new byte[1024];
                    int count;
                    while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        fs.Write(bytes, 0, count);
                    }
                }
                response.Close();
                decaptcha.Init();
                string captchatext = decaptcha.DecodeCaptcha("captcha.jpg");

                if (captchatext != "Something's wrong with decaptcha service at the moment :(")
                {
                    ie.TextField(Find.ById("recaptcha_response_field")).TypeText(captchatext);
                    ie.Link(Find.ByClass("fluid_blue_dark_medium")).Click();
                    System.Threading.Thread.Sleep(1000);
                    // http://economy.erepublik.com/en/time-management/captcha/train
                    while (ie.Url.IndexOf("/captcha/") != -1)
                    {
                        decaptcha.IncorrectCaptcha();

                        for (int i = 0; i < images.Count; i++)
                        {
                            if (images[i].Src.StartsWith("http://www.google.com/recaptcha/api/image?c="))
                            {
                                captchaImageUrl = images[i].Src;
                            }
                        }

                        request = (HttpWebRequest)WebRequest.Create(new Uri(captchaImageUrl));
                        response = request.GetResponse();
                        using (Stream stream = response.GetResponseStream())
                        using (FileStream fs = System.IO.File.OpenWrite("captcha.jpg"))
                        {
                            byte[] bytes = new byte[1024];
                            int count;
                            while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                fs.Write(bytes, 0, count);
                            }
                        }
                        response.Close();
                        captchatext = decaptcha.DecodeCaptcha("captcha.jpg");
                        ie.TextField(Find.ById("recaptcha_response_field")).TypeText(captchatext);
                        ie.Link(Find.ByClass("fluid_blue_dark_medium")).Click();
                        System.Threading.Thread.Sleep(1000);
                    }
                    Status("Decoded captcha [" + captchatext + "]");
                }
                else
                {
                    Status(captchatext);
                    MessageBox.Show("Unfortunately you have to manually enter the captcha in the browser window due to problems with decaptcher service. Click OK ONCE YOU SUBMITTED THE CODE to proceed.", "Z-Bot Warning");
                }

                decaptcha.Close();
#else
                MessageBox.Show("Please enter the captcha in the browser window. Click OK ONCE YOU SUBMITTED THE CODE to proceed.", "Z-Bot Warning");
#endif
                    if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                return true;
            }

            return false;
        }

        // Buy
        private bool Buy()
        {
            Status("Buying products [" + settings.buyWhat + "] [" + settings.buyAmount.ToString() + "]");
            if (settings.buyWhat == "Food Q1")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/1/1/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Food Q2")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/1/2/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Food Q3")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/1/3/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Food Q4")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/1/4/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Food Q5")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/1/5/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Weapon Q1")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/15/1/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Weapon Q2")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/15/2/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Weapon Q3")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/15/3/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Weapon Q4")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/15/4/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }
            else if (settings.buyWhat == "Weapon Q5")
            {
                ie.GoTo("http://economy.erepublik.com/en/market/0/15/5/citizen/0/price_asc/1");
                SelectCompanyAndBuy();
                return true;
            }

            return false;
        }

        private bool SelectCompanyAndBuy()
        {
            TextFieldCollection amountFields = ie.TextFields.Filter(Find.ByClass("shadowed buyField"));
            TableCellCollection stockFields = ie.TableCells.Filter(Find.ByClass("m_stock"));
            LinkCollection buyButtons = ie.Links.Filter(Find.ByClass("f_light_blue_big buyOffer"));
            int selectedCompany = -1;
            for (int i = 0; i < stockFields.Count; i++)
            {
                string currentStock_string = stockFields[i].Text.ToString();
                int currentStock = Convert.ToInt32(currentStock_string);
                if (currentStock >= settings.buyAmount)
                {
                    selectedCompany = i;
                    break;
                }
            }

            if (selectedCompany != -1)
            {
                amountFields[selectedCompany].TypeText(settings.buyAmount.ToString());
                buyButtons[selectedCompany].Click();
                if (!CheckMoneyAndRoom()) return false;
                Status("Successfully bought products.");
                return true;
            }

            return false;
        }

        private bool CheckMoneyAndRoom()
        {
            // You do not have enough money in your account.
            string source = ie.Html;
            int enoughmoney = source.IndexOf("You do not have enough money in your account.");
            if (enoughmoney != -1)
            {
                Status("No enough money to buy products.");
                return false;
            }
            // You don't have enough room in your inventory.
            int enoughroom = source.IndexOf("You don't have enough room");
            if (enoughroom != -1)
            {
                Status("No enough room to buy products.");
                return false;
            }

            return true;
        }

        private bool GetAJob()
        {
            Status("Getting a job ...");

            if (ie.Url == "http://economy.erepublik.com/en/work")
                ie.Refresh();

            ie.GoTo("http://economy.erepublik.com/en/work");
            
            string source = ie.Html.ToLower();
            // Let's check do we actually have a job
            int dowehaveajob = 1;
            if (ie.Url == "http://economy.erepublik.com/en/land/overview/" + accountID)
                dowehaveajob = 0;

            if (dowehaveajob == 0)
            {
                if (settings.getJobOfferLink == "")
                {
                    string[] countryId_a = StringBetween("http://economy.erepublik.com/en/market/job/", "\"", source, false, false);
                    ie.GoTo("http://economy.erepublik.com/en/market/job/" + countryId_a[0]);
                    LinkCollection applyButtons = ie.Links.Filter(Find.ByTitle("Apply"));
                    // Apply first offer
                    applyButtons[0].Click();
                    // Let's check could we get our salary
                    source = ie.Html;
                    int hasSalary = source.IndexOf("does not have enough money to pay this salary");
                    int f = 1;
                    while(hasSalary != -1)
                    {
                        if (f < 5)
                        {
                            ie.GoTo("http://economy.erepublik.com/en/market/job/" + countryId_a[0]);
                            LinkCollection applyButtons2 = ie.Links.Filter(Find.ByClass("f_light_blue_big"));
                            // Apply first offer
                            applyButtons2[f].Click();
                            source = ie.Html;
                            hasSalary = source.IndexOf("does not have enough money to pay this salary");
                            f++;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    ie.GoTo(settings.getJobOfferLink);
                    ie.Link(Find.ByClass("f_light_blue_big")).Click();
                    Status("Got a job.");
                    return true;
                }
            } 
            else
            {
                Status("We already have a job.");
            }

            return false;
        }

        private bool Donate(string id, string currency, string amount)
        {
            Status("Donating to [" + id + "] ...");
            ie.GoTo("http://economy.erepublik.com/en/citizen/donate/" + id);
            ie.Link(Find.ById("donatemoneylink")).Click();
            TextField amountBoxHidden = ie.TextField(Find.ByValue(currency));
            Match match = Regex.Match(amountBoxHidden.Id, @"donate_currency_(\d*)", RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string currencyNumber = match.Groups[1].Value;

                TextField amountBox = ie.TextField(Find.ById("donate_sum_" + currencyNumber));
                amountBox.TypeText(amount);

                ie.Button(Find.ById("buttonDonateId_" + currencyNumber)).Click();
                System.Threading.Thread.Sleep(5000);
                if(CaptchaCheck()) Donate(id, currency, amount);
                Status("Donated.");
            }

            return true;
        }

        private string GetCurrencyID(string currency)
        {
            return "fu";
        }

        private float GetGoldAmount()
        {
            for(int i = 0; i < accountBank.Length; i++)
            {
                if (accountBank[i].name == "Gold")
                    return (float)accountBank[i].amount;
            }

            return 0;
        }

        // Work (booster = 1, 2, 3, 4)
        private bool Work(int booster)
        {
            Status("Going to work ...");

            if (booster > 4 && booster < 1) return false;

            double boosterCost = GetWorkBoosterCost(booster);

            // Check do we have enough gold
            float goldAmount = GetGoldAmount();
            while (boosterCost > goldAmount)
            {
                booster--;
                boosterCost = GetWorkBoosterCost(booster);
                if (boosterCost == 0.79) booster--;
            }

            if (ie.Url == "http://economy.erepublik.com/en/work")
                ie.Refresh();

            if (ie.Url != "http://economy.erepublik.com/en/work") 
                ie.GoTo("http://economy.erepublik.com/en/work");

            // Let's check have we worked today
            string source = ie.Html;
            int haveweworked = source.IndexOf("hasWorkedToday = 1;");
            if (haveweworked != -1)
            {
                Status("Already worked today.");
                return false;
            }
            // Let's check do we actually have a job
            int dowehaveajob = source.IndexOf("You do not have a job");
            if (dowehaveajob != -1)
            {
                Status("Don't have a job.");
                return false;
            }
            // Let's check is our wellness below 10
            int dowehavewellness = source.IndexOf("devastated");
            if (dowehaveajob != -1)
            {
                Status("Wellness too low. Can't work.");
                return false;
            }

            if (ie.Link(Find.ByClass("fluid_blue_dark_medium")).Exists)
            {
                if (ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/1_77x77.png")).Exists)
                {
                    if (booster == 1)
                    {
                        ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/1_77x77.png")).Click();
                    }
                    if (booster == 2)
                    {
                        ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/2_77x77.png")).Click();
                    }
                    if (booster == 3)
                    {
                        ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/3_77x77.png")).Click();
                    }
                    if (booster == 4)
                    {
                        ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/4_77x77.png")).Click();
                    }
                    ie.Link(Find.ByClass("fluid_blue_dark_medium")).Click();
                    if (CaptchaCheck()) { Work(booster); return false; }

                    if (gotCaptcha != 1)
                    {
                        // Let's check was there any raw
                        bool anyError = ie.Table(Find.ByClass("error_message ")).Exists;
                        if (anyError)
                        {
                            Status("Couldn't work (no salary or raw)");
                            if (settings.getJobIfNoRaw)
                            {
                                var approveConfirmDialog = new WatiN.Core.DialogHandlers.ConfirmDialogHandler();
                                ie.GoTo("http://economy.erepublik.com/en/my-places/company/" + accountID);
                                using (new WatiN.Core.DialogHandlers.UseDialogOnce(ie.DialogWatcher, approveConfirmDialog))
                                {
                                    Status("Resigning this job ...");
                                    ie.Link(Find.ByTitle("Resign")).ClickNoWait();
                                    approveConfirmDialog.WaitUntilExists();
                                    approveConfirmDialog.OKButton.Click();
                                    ie.WaitForComplete();
                                }
                                GetAJob();
                                Work(booster);
                                return false;
                            }
                            return false;
                        }

                        // Eat after work
                        if (settings.eatAfterWork) Eat(1);
                        Status("Ate after work.");

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        // Eat food
        private bool Eat(int howMuch)
        {
            Status("Eating " + howMuch + " food.");
            try
            {
                if (ie.Link(Find.ById("DailyConsumtionTrigger")).Exists && ie.Link(Find.ByClass("eat_food   ")).Exists)
                {
                    for (int i = 0; i < howMuch; i++)
                    {
                        ie.Link(Find.ById("DailyConsumtionTrigger")).Click();
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception e)
            {
                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Show);
                MessageBox.Show(e.ToString(), "Z-Bot Error");
                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                return false;
            }

            return true;
        }

        // Work (booster = 1, 2, 3, 4)
        private bool Train(int booster)
        {
            Status("Going to train ...");

            if (booster > 4 && booster < 1) return false;

            if(ie.Url != "http://economy.erepublik.com/en/train") 
                ie.GoTo("http://economy.erepublik.com/en/train");

            // Let's check have we trained today
            string source = ie.Html;
            int havewetrained = source.IndexOf("hasTrained = 1;");
            if (havewetrained != -1)
            {
                Status("Already trained today.");
                return false;
            }

            if (booster == 1)
            {
                ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/10_77x77.png")).Click();
            }
            if (booster == 2)
            {
                ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/11_77x77.png")).Click();
            }
            if (booster == 3)
            {
                ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/12_77x77.png")).Click();
            }
            if (booster == 4)
            {
                ie.Image(Find.BySrc("http://static.erepublik.com/uploads/boosters/13_77x77.png")).Click();
            }
            ie.Link(Find.ByClass("fluid_blue_dark_medium")).Click();
            if (CaptchaCheck()) { Train(booster); return false; }
            if(gotCaptcha != 1)
            {
                Status("Trained.");
                return true;
            }
            else
            {
                return false;
            }
        }

        // Fight (battleID = battle ID at the end of the link; times = how many times to fight)
        private bool Fight(string battleID, int times)
        {
            Status("Going to fight [" + battleID + "] - " + times + " times");
            ie.GoTo("http://www.erepublik.com/en/military/battlefield/" + battleID);

            for (int i = 0; i < times; i++)
            {
                ie.Link(Find.ById("fight_btn")).Click();
                ie.Link(Find.ById("hospital_btn")).Click();
                System.Threading.Thread.Sleep(2000);
            }

            return true;
        }

        // Check currencies
        private Currency[] CheckCurrencies()
        {
            ie.GoTo("http://economy.erepublik.com/en/accounts/" + accountID);

            DivCollection pushlefts = ie.Div("allaccounts").Divs.Filter(Find.ByClass("push_left"));
            DivCollection pushrights = ie.Div("allaccounts").Divs.Filter(Find.ByClass("push_right"));

            Currency[] toReturn = new Currency[pushlefts.Count];

            for (int i = 0; i < pushlefts.Count; i++)
            {
                string currencyName = "";
                string currencyAmount = "";

                currencyName = pushlefts[i].Text.Trim();

                currencyAmount = pushrights[i].Text.Trim();

                if (currencyName != "")
                {
                    toReturn[i].name = currencyName;
                    toReturn[i].amount = Convert.ToDouble(currencyAmount);
                }
            }            

            return toReturn;
        }

        // Vote (articleID = article ID [the number] near the end of the link)
        private bool Vote(string articleId)
        {
            Status("Voting article [" + articleId + "]");

            ie.GoTo("http://www.erepublik.com/en/article/" + articleId + "/1/20");

            // Check have we already voted
            string source = ie.Html;
            int havewevoted = source.IndexOf("vote_1");
            if (havewevoted == -1)
            {
                Status("Already voted this article.");
                return false;
            }

            ie.Link(Find.ByClass("vote_1")).Click();

            return true;
        }

        // Subscribes to the given article author's newspaper
        private bool Subscribe()
        {
            Status("Subscribing ...");

            if (ie.Url != "http://www.erepublik.com/en/article/" + settings.voteId.ToString() + "/1/20")
                ie.GoTo("http://www.erepublik.com/en/article/" + settings.voteId.ToString() + "/1/20");

            ie.Link(Find.ByClass("houdini subscribeToNewspaper goright")).Click();

            return true;
        }

        // Comments an article
        private bool Comment()
        {
            Status("Commenting ...");

            if (ie.Url != "http://www.erepublik.com/en/article/" + settings.voteId.ToString() + "/1/20")
                ie.GoTo("http://www.erepublik.com/en/article/" + settings.voteId.ToString() + "/1/20");

            if(ie.Div(Find.ByClass("submitpost-core")).Exists)
            {
                ie.TextField(Find.ById("article_comment")).TypeText(settings.commentText);
                ie.Element(Find.ByValue("Post a comment")).Click();

                return true;
            }

            return false;
        }

        // Checks are we logged in
        private bool LoggedIn()
        {
            // Check did we login
            if (ie.Div(Find.ById("miniprofile")).Exists)
                return true;

            return false;
        }      

        // Start button clicked
        private void startBtn_Click(object sender, EventArgs e)
        {
            try
            {

#if DISABLE_IMAGES
                RegistryKey ieKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main");
                ieKey.SetValue("Display Inline Images", "no");
#else
                RegistryKey ieKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main");
                ieKey.SetValue("Display Inline Images", "yes");
#endif
                //Z-Bot Launcher
                IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, "Z-Bot Launcher");
                if (hwnd != IntPtr.Zero)
                {
                    int f = 0;
                    foreach (string arg in Environment.GetCommandLineArgs())
                    {
                        f++;
                    }

                    if (f > 1)
                    {
                        if (LoadSettings())
                        {
                            Status("Starting ...");

                            WatiN.Core.Settings.WaitForCompleteTimeOut = 40;
                            WatiN.Core.Settings.WaitUntilExistsTimeOut = 40;
#if IE
                            ie = new IE("about:blank", true);
                            ie.ClearCookies();
                            if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                            if (settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Show);
#endif
#if FIREFOX
                            ie = new FireFox("http://www.erepublik.com");
#endif

                            if (showUpdates.Checked)
                            {
                                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Show);
                                ie.GoTo("http://mafioso.nihplod.com/updates.txt");
                                MessageBox.Show("Please click OK once you read the updates :D", "Z-Bot Warning");
                                if (!settings.showIE) ie.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                            }

                            started = 1;
                                                        
                            Thread t = new Thread(Bot);
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();

                            startBtn.Enabled = false;
                            startBtn.Text = "Started ...";
                        }
                    }
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Z-Bot Error");
            }
        }

        // Change user agent
        private void ChangeUserAgent()
        {
            string userAgent = UserAgents[Rand(0, UserAgents.Length)];
            Match match = Regex.Match(userAgent, @"(.*) \((.*)\) (.*)", RegexOptions.IgnoreCase);

            // Finally, we get the Group value and display it.
            string defaultValue = "Mozilla/5.0";
            string middleStuff = "X11; U; Linux i686; en-US; rv:1.9.0.1";
            string afterBrackets = "Gecko/2008072716 IceCat/3.0.1-g1";

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                defaultValue = match.Groups[1].Value;
                middleStuff = match.Groups[2].Value;
                afterBrackets = match.Groups[3].Value;
            }

            RegistryKey ieKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\5.0\User Agent");
            int rand = Rand(1, 3);
            ieKey.SetValue("", defaultValue);
            ieKey.SetValue("Compatible", "");
            ieKey.SetValue("Version", "");
            ieKey.SetValue("Platform", middleStuff + ") (" + afterBrackets);
            ieKey.CreateSubKey("Post Platform");
            ieKey.DeleteSubKeyTree("Post Platform");
        }

        // Load settings
        private bool LoadSettings()
        {
            TextWriter settingsFile = new StreamWriter("C:\\settings.txt");
            settings.eatBeforeWork = doEatBeforeWork.Checked;
            settingsFile.WriteLine("eatBeforeWork," + (settings.eatBeforeWork == true ? "1" : "0"));
            if (settings.eatBeforeWork && eatHowManyBeforeWork.Text == "") { MessageBox.Show("Please input an amount of food to eat before work!"); return false; }
            if (settings.eatBeforeWork) settings.eatBeforeWorkHowMuch = Convert.ToInt32(eatHowManyBeforeWork.Text); else settings.eatBeforeWorkHowMuch = 0;
            settingsFile.WriteLine("eatBeforeWorkHowMuch," + settings.eatBeforeWorkHowMuch.ToString());
            settings.work = doWork.Checked;
            settingsFile.WriteLine("work," + (settings.work == true ? "1" : "0"));
            if (settings.work && workBooster.Text == "") { MessageBox.Show("Please choose a work booster!"); return false; }
            if (settings.work) settings.workBooster = Convert.ToInt32(workBooster.Text); else settings.workBooster = 0;
            settingsFile.WriteLine("workBooster," + settings.workBooster.ToString());
            settings.eatAfterWork = doEatFoodAfterWork.Checked;
            settingsFile.WriteLine("eatAfterWork," + (settings.eatAfterWork == true ? "1" : "0"));
            settings.getJob = doGetJob.Checked;
            settingsFile.WriteLine("getJob," + (settings.getJob == true ? "1" : "0"));
            if (settings.getJob) settings.getJobOfferLink = jobOfferLink.Text;
            settingsFile.WriteLine("getJobOfferLink," + settings.getJobOfferLink);
            settings.getJobIfNoRaw = checkGetJobIfNoRaw.Checked;
            settingsFile.WriteLine("getJobIfNoRaw," + (settings.getJobIfNoRaw == true ? "1" : "0"));
            settings.train = doTrain.Checked;
            settingsFile.WriteLine("train," + (settings.train == true ? "1" : "0"));
            if (settings.train && trainBooster.Text == "") { MessageBox.Show("Please choose a train booster!"); return false; }
            if (settings.train) settings.trainBooster = Convert.ToInt32(trainBooster.Text); else settings.trainBooster = 0;
            settingsFile.WriteLine("trainBooster," + settings.trainBooster);
            settings.eatAfterTrain = doEatFoodAfterTrain.Checked;
            settingsFile.WriteLine("eatAfterTrain," + (settings.eatAfterTrain == true ? "1" : "0"));
            settings.fight = doFight.Checked;
            settingsFile.WriteLine("fight," + (settings.fight == true ? "1" : "0"));
            if (settings.fight && battleId.Text == "") { MessageBox.Show("Please put in a valid battle ID!"); return false; }
            if (settings.fight) settings.battleId = Convert.ToInt32(battleId.Text); else settings.battleId = 0;
            settingsFile.WriteLine("battleId," + settings.battleId.ToString());
            if (settings.fight && fightTimes.Text == "") { MessageBox.Show("Please put in a valid number of fights!"); return false; }
            if (settings.fight) settings.fightTimes = Convert.ToInt32(fightTimes.Text); else settings.fightTimes = 0;
            settingsFile.WriteLine("fightTimes," + settings.fightTimes.ToString());
            settings.vote = doVote.Checked;
            settingsFile.WriteLine("vote," + (settings.vote == true ? "1" : "0"));
            if (settings.vote && voteId.Text == "") { MessageBox.Show("Please put in a valid vote ID!"); return false; }
            if (settings.vote) settings.voteId = Convert.ToInt32(voteId.Text); else settings.voteId = 0;
            settingsFile.WriteLine("voteId," + settings.voteId);
            settings.sub = doSub.Checked;
            settingsFile.WriteLine("sub," + (settings.sub == true ? "1" : "0"));
            settings.buy = doBuy.Checked;
            settingsFile.WriteLine("buy," + (settings.buy == true ? "1" : "0"));
            if (settings.buy && buyWhat.Text == "") { MessageBox.Show("Please choose what to buy!"); return false; }
            if (settings.buy) settings.buyWhat = buyWhat.Text; else settings.buyWhat = "";
            settingsFile.WriteLine("buyWhat," + settings.buyWhat);
            if (settings.buy && buyAmount.Text == "") { MessageBox.Show("Please put in a valid buy amount!"); return false; }
            if (settings.buy) settings.buyAmount = Convert.ToInt32(buyAmount.Text); else settings.buyAmount = 0;
            settingsFile.WriteLine("buyAmount," + settings.buyAmount.ToString());
            if (checkModem.Checked && !checkConnection.Checked && !checkHotspot.Checked && !checkProxylist.Checked) { settings.renewIpType = 1; settings.modemType = chosenModem.Text; }
            if (checkConnection.Checked && !checkModem.Checked && !checkHotspot.Checked && !checkProxylist.Checked) settings.renewIpType = 2;
            if (checkHotspot.Checked && !checkModem.Checked && !checkConnection.Checked && !checkProxylist.Checked) { settings.renewIpType = 3; settings.hotSpotURL = hotSpotLink.Text; }
            if (checkProxylist.Checked && !checkModem.Checked && !checkConnection.Checked && !checkHotspot.Checked) { settings.renewIpType = 4; }
            if (checkModem.Checked && checkConnection.Checked) { MessageBox.Show("You can only choose one IP renewing type! Please correct the errors and try again."); return false; }
            if (!checkModem.Checked && !checkConnection.Checked && !checkHotspot.Checked && !checkProxylist.Checked) settings.renewIpType = 0;
            settingsFile.WriteLine("renewIpType," + settings.renewIpType.ToString());
            if(settings.renewIpType == 1)
                settingsFile.WriteLine("modemType," + settings.modemType);
            if(settings.renewIpType == 3)
                settingsFile.WriteLine("hotSpotURL," + settings.hotSpotURL);

            settings.donate = checkDonate.Checked;
            settingsFile.WriteLine("donate," + (settings.donate == true ? "1" : "0"));
            if(checkDonate.Checked)
            {
                if (donateID.Text == "") { MessageBox.Show("Please enter the donate ID!"); return false; }
                if (donateCurrency.Text == "") { MessageBox.Show("Please enter the donate currency!"); return false; }
                if (donateHowMuch.Text == "") { MessageBox.Show("Please enter the donate amount!"); return false; }
                settings.donateId = donateID.Text;
                settings.donateCurrency = donateCurrency.Text;
                settings.donateHowMuch = donateHowMuch.Text;
                settingsFile.WriteLine("donateId," + settings.donateId);
                settingsFile.WriteLine("donateCurrency," + settings.donateId);
                settingsFile.WriteLine("donateHowMuch," + settings.donateHowMuch);
            }
            //if (!checkModem.Checked && !checkConnection.Checked) { MessageBox.Show("Please choose one IP renewing option!"); return false; }
            settings.comment = checkComment.Checked;
            settingsFile.WriteLine("comment," + (settings.comment == true ? "1" : "0"));
            if (checkComment.Checked && commentText.Text != "") settings.commentText = commentText.Text;
            if (checkComment.Checked && commentText.Text == "") { MessageBox.Show("Please enter the comment text!"); return false; }
            settingsFile.WriteLine("commentText," + settings.commentText);
            settings.skipAccountsWithCaptcha = skipAccountsCaptcha.Checked;
            settingsFile.WriteLine("skipAccountsWithCaptcha," + (settings.skipAccountsWithCaptcha == true ? "1" : "0"));
            settings.modemUsername = modemUsername.Text;
            settings.modemPassword = modemPassword.Text;
            settings.modemAddress = modemAddress.Text;
            settingsFile.WriteLine("modemUsername," + settings.modemUsername);
            settingsFile.WriteLine("modemPassword," + settings.modemPassword);
            settingsFile.WriteLine("modemAddress," + settings.modemAddress);
            settings.connectionName = connectionName.Text;
            settings.connectionUsername = connectionUsername.Text;
            settings.connectionPassword = connectionPassword.Text;
            settingsFile.WriteLine("connectionName," + settings.connectionName);
            settingsFile.WriteLine("connectionUsername," + settings.connectionUsername);
            settingsFile.WriteLine("connectionPassword," + settings.connectionPassword);
            settings.showIE = checkShowIE.Checked;
            settingsFile.WriteLine("showIE," + (settings.showIE == true ? "1" : "0"));
            settingsFile.Close();

            return true;
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("by ... you know who ;)\r\n\r\nincase you don't know how to use it, read the readme pdf or find me on irc.", "Z-Bot Help");
        }

        // Status some stuff
        private void Status(string text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.statusBox.Text += text + "\r\n";
                this.statusBox.SelectionStart = this.statusBox.Text.Length;
                this.statusBox.ScrollToCaret();
            });
            logFile.WriteLine(text);
            logFile.Flush();
        }

        // Enables the start button again
        private void EnableStart()
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.startBtn.Text = "Start again";
                this.startBtn.Enabled = true;
            });
        }    

        private int Rand(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Loads accounts from the accounts.txt file located
        // in the same folder as the executable of Z-Bot
        private void LoadAccounts()
        {
            StreamReader SR;
            SR = System.IO.File.OpenText("accounts.txt");
            while (!SR.EndOfStream)
            {
                string S = SR.ReadLine();
                string[] accountData = S.Split(new Char[] { ',' });
                if(accountData.Length == 2)
                {
                    string[] columnData = { accountData[1], "" };
                    if (accountData[0].Substring(0, 2) != "//")
                    {
                        listAccounts.Items.Add(accountData[0]).SubItems.AddRange(columnData);
                        AccountUsernames.Add(accountData[0]);
                        AccountPasswords.Add(accountData[1]);
                    }
                }
                else
                {
                    MessageBox.Show("You seem to have put some wrong data in the accounts.txt file:\r\n\r\n" + S + "\r\n\r\nPlease check is it in the right format.", "Z-Bot Warning");
                }
            }
            SR.Close();
        }

        // Loads accounts from the accounts.txt file located
        // in the same folder as the executable of Z-Bot
        private void LoadSettingsFromFile()
        {
            StreamReader SR;
            if (System.IO.File.Exists("C:\\settings.txt"))
            {
                SR = System.IO.File.OpenText("C:\\settings.txt");
                while (!SR.EndOfStream)
                {
                    string S = SR.ReadLine();
                    string[] settingsData = S.Split(new Char[] { ',' });
                    if (settingsData.Length == 2)
                    {
                        if(settingsData[0] == "eatBeforeWork")
                            settings.eatBeforeWork = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "eatBeforeWorkHowMuch")
                            settings.eatBeforeWorkHowMuch = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "work")
                            settings.work = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "workBooster")
                            settings.workBooster = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "eatAfterWork")
                            settings.eatAfterWork = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "eatBeforeWorkHowMuch")
                            settings.eatBeforeWorkHowMuch = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "getJob")
                            settings.getJob = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "getJobOfferLink")
                            settings.getJobOfferLink = settingsData[1];
                        else if (settingsData[0] == "getJobIfNoRaw")
                            settings.getJobIfNoRaw = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "train")
                            settings.train = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "trainBooster")
                            settings.trainBooster = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "eatAfterTrain")
                            settings.eatAfterTrain = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "fight")
                            settings.fight = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "battleId")
                            settings.battleId = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "fightTimes")
                            settings.fightTimes = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "vote")
                            settings.vote = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "voteId")
                            settings.voteId = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "sub")
                            settings.sub = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "buy")
                            settings.buy = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "buyWhat")
                            settings.buyWhat = settingsData[1];
                        else if (settingsData[0] == "buyAmount")
                            settings.buyAmount = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "renewIpType")
                            settings.renewIpType = Convert.ToInt32(settingsData[1]);
                        else if (settingsData[0] == "modemType")
                            settings.modemType = settingsData[1];
                        else if (settingsData[0] == "donate")
                            settings.donate = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "donateId")
                            settings.donateId = settingsData[1];
                        else if (settingsData[0] == "donateCurrency")
                            settings.donateCurrency = settingsData[1];
                        else if (settingsData[0] == "donateHowMuch")
                            settings.donateHowMuch = settingsData[1];
                        else if (settingsData[0] == "comment")
                            settings.comment = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "skipAccountsWithCaptcha")
                            settings.skipAccountsWithCaptcha = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "showIE")
                            settings.showIE = settingsData[1] == "1" ? true : false;
                        else if (settingsData[0] == "modemUsername")
                            settings.modemUsername = settingsData[1];
                        else if (settingsData[0] == "modemPassword")
                            settings.modemPassword = settingsData[1];
                        else if (settingsData[0] == "modemAddress")
                            settings.modemAddress = settingsData[1];
                        else if (settingsData[0] == "connectionName")
                            settings.connectionName = settingsData[1];
                        else if (settingsData[0] == "connectionUsername")
                            settings.connectionUsername = settingsData[1];
                        else if (settingsData[0] == "connectionPassword")
                            settings.connectionPassword = settingsData[1];
                    }
                    else
                    {
                        MessageBox.Show("You seem to have messed with settings.txt. Delete it and make new settings.", "Z-Bot Warning");
                    } 
                }

                doEatBeforeWork.Checked = settings.eatBeforeWork;
                eatHowManyBeforeWork.Text = settings.eatBeforeWorkHowMuch.ToString();
                doWork.Checked = settings.work;
                workBooster.Text = settings.workBooster.ToString();
                doEatFoodAfterWork.Checked = settings.eatAfterWork;
                doGetJob.Checked = settings.getJob;
                jobOfferLink.Text = settings.getJobOfferLink;
                checkGetJobIfNoRaw.Checked = settings.getJobIfNoRaw;
                doTrain.Checked = settings.train;
                trainBooster.Text = settings.trainBooster.ToString();
                doEatFoodAfterTrain.Checked = settings.eatAfterTrain;
                doFight.Checked = settings.fight;
                battleId.Text = settings.battleId.ToString();
                fightTimes.Text = settings.fightTimes.ToString();
                doVote.Checked = settings.vote;
                voteId.Text = settings.voteId.ToString();
                doSub.Checked = settings.sub;
                doBuy.Checked = settings.buy;
                buyWhat.Text = settings.buyWhat;
                buyAmount.Text = settings.buyAmount.ToString();
                chosenModem.Text = settings.modemType;

                if (settings.renewIpType == 0)
                {
                    groupBox4.Enabled = false;
                    groupBox5.Enabled = false;
                    checkModem.Checked = false;
                    checkConnection.Checked = false;
                    checkHotspot.Checked = false;
                    checkProxylist.Checked = false;
                }
                if (settings.renewIpType == 1) checkModem.Checked = true;
                if (settings.renewIpType == 2) checkConnection.Checked = true;
                if (settings.renewIpType == 3) checkHotspot.Checked = true;
                if (settings.renewIpType == 4) checkProxylist.Checked = true;

                checkDonate.Checked = settings.donate;
                if (checkDonate.Checked)
                {
                    donateID.Text = settings.donateId;
                    donateCurrency.Text = settings.donateCurrency;
                    donateHowMuch.Text = settings.donateHowMuch;
                }
                checkComment.Checked = settings.comment;
                if (checkComment.Checked) commentText.Text = settings.commentText;

                skipAccountsCaptcha.Checked = settings.skipAccountsWithCaptcha;
                modemUsername.Text = settings.modemUsername;
                modemPassword.Text = settings.modemPassword;
                modemAddress.Text = settings.modemAddress;
                connectionName.Text = settings.connectionName;
                connectionUsername.Text = settings.connectionUsername;
                connectionPassword.Text = settings.connectionPassword;
                checkShowIE.Checked = settings.showIE;

                SR.Close();
            }
        }

        // Loads proxies from the proxy.txt file located
        // in the same folder as the executable of Z-Bot
        private void LoadProxies()
        {
            StreamReader SR;
            SR = System.IO.File.OpenText("proxy.txt");
            while (!SR.EndOfStream)
            {
                string line = SR.ReadLine();
                ProxyList.Add(line);
            }
            SR.Close();
        }

        // Renew IP using modem
        private void RenewIPModem()
        {
            if (settings.modemType == "Paradigm")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/goform/formStatus -d \"submit-url=%2Fhome.asp&submitppp0=Disconnect\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(3000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/goform/formStatus -d \"submit-url=%2Fhome.asp&submitppp0=Connect\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(4000);
                }
                else if(settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/goform/formStatus -d \"submit-url=%2Fhome.asp&submitppp0=Disconnect\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/goform/formStatus -d \"submit-url=%2Fhome.asp&submitppp0=Connect\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            if (settings.modemType == "Speedtouch 516i")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "-d \"0=13&1=Internet&5=2\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "-d \"0=12&1=Internet&5=2\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=13&1=Internet&5=2\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=12&1=Internet&5=2\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            if (settings.modemType == "Speedtouch 546v6")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "-d \"0=13&1=Shared_Internet&5=2\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "-d \"0=12&1=Shared_Internet&5=2\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=13&1=Shared_Internet&5=2\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=12&1=Shared_Internet&5=2\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            // Speedtouch 780WL
            if (settings.modemType == "Speedtouch 780WL")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + " -d \"0=13&1=Internet&5=1\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + " -d \"0=12&1=Internet&5=1\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=13&1=Internet&5=12\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "-d \"0=12&1=Internet&5=1\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            if (settings.modemType == "TP-Link TD-8817")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/Forms/status_deviceinfo_1 -d DvInfo_PVC=PVC0&PPPoEConn=Disconnect&PVC_or_Renew_or_Release=0");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/Forms/status_deviceinfo_1 -d DvInfo_PVC=PVC0&PPPoEConn=Connect&PVC_or_Renew_or_Release=0");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000); 
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/Forms/status_deviceinfo_1 -d DvInfo_PVC=PVC0&PPPoEConn=Disconnect&PVC_or_Renew_or_Release=0");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/Forms/status_deviceinfo_1 -d DvInfo_PVC=PVC0&PPPoEConn=Connect&PVC_or_Renew_or_Release=0");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000); 
                }
            }
            // Alice Gate VoIP
            if (settings.modemType == "Alice Gate VoIP")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/admin.cgi -d \"active_page=9117&page_title=Stato+Modem&mimic_button_field=submit_button_disattiva%3A+nat..&button_value=&strip_page_top=0\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/admin.cgi -d \"active_page=9117&page_title=Stato+Modem&mimic_button_field=submit_button_attiva%3A+nat..&button_value=nat&strip_page_top=0\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/admin.cgi -d \"active_page=9117&page_title=Stato+Modem&mimic_button_field=submit_button_disattiva%3A+nat..&button_value=&strip_page_top=0\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/admin.cgi -d \"active_page=9117&page_title=Stato+Modem&mimic_button_field=submit_button_attiva%3A+nat..&button_value=nat&strip_page_top=0\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            // D-Link DSL-584T
            if (settings.modemType == "D-Link DSL-584T")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/cgi-bin/webcm -d \"getpage=../html/status/deviceinfofile.htm&encaps0:settings/manual_conn=0&var:conid=encaps0&var:mycon=connection0&var:contype=&connection0:pppoe:command/stop=&var:judge=\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/cgi-bin/webcm -d \"getpage=../html/status/deviceinfofile.htm&encaps0:settings/manual_conn=1&var:conid=encaps0&var:mycon=connection0&var:contype=&connection0:pppoe:command/start=&var:judge=1\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/webcm -d \"getpage=../html/status/deviceinfofile.htm&encaps0:settings/manual_conn=0&var:conid=encaps0&var:mycon=connection0&var:contype=&connection0:pppoe:command/stop=&var:judge=\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/webcm -d \"getpage=../html/status/deviceinfofile.htm&encaps0:settings/manual_conn=1&var:conid=encaps0&var:mycon=connection0&var:contype=&connection0:pppoe:command/start=&var:judge=1\"");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            // Sagemcom Optima 1704
            if (settings.modemType == "Sagemcom Optima 1704")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/wancfg.cmd?action=pppinterconn&pppcmd=Disconnect");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/wancfg.cmd?action=pppinterconn&pppcmd=Connect&pppUserName=jbozic16@optinet&pppPassword=326p&dddd=kkkk");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/wancfg.cmd?action=pppinterconn&pppcmd=Disconnect");
                    // Sleep for a lil bit                                                           
                    System.Threading.Thread.Sleep(8000);
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/wancfg.cmd?action=pppinterconn&pppcmd=Connect&pppUserName=jbozic16@optinet&pppPassword=326p&dddd=kkkk");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            // Airties 5450
            if (settings.modemType == "Airties 5450")
            {
#if FIREFOX
                if (started == 0) ie = new FireFox("about:blank");
#endif
#if IE
                if (started == 0) ie = new IE("about:blank", true);
#endif
                ie.GoTo("http://" + settings.modemAddress + "/loginmain.html");
                if(settings.modemPassword != "") ie.TextField(Find.ById("uiPostPassword")).TypeText(settings.modemPassword);
                ie.Button(Find.ByValue("OK")).Click();
                System.Threading.Thread.Sleep(3000);
                ie.GoTo("http://" + settings.modemAddress + "/tools/wait_reboot.html?status_modem=restart_modem");
                System.Threading.Thread.Sleep(120000);
            }

            // Linksys AM200
            if (settings.modemType == "Linksys AM200")
            {
                if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    // First login
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/login.exe -d \"username=" + settings.modemUsername + "&password=" + settings.modemPassword + "&x=39&y=13&exec_cgis=login_CGI\" –c \"cookies.txt\"");
                    // Now disconnect
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/cgi.exe -d \"delay=0&_f_no_write_config=1&cmd_btn=Disconnect&exec_cgis=StaM&ret_url=%2Findex.stm%3Ftitle%3DStatus-Modem\" -b \"cookies.txt\"");
                    // Now connect
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/cgi.exe -d \"delay=0&_f_no_write_config=1&cmd_btn=Connect&exec_cgis=StaM&ret_url=%2Findex.stm%3Ftitle%3DStatus-Modem\" -b \"cookies.txt\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
                else if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    // First login
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/login.exe -d \"username=&password=&x=39&y=13&exec_cgis=login_CGI\" –c \"cookies.txt\"");
                    // Now disconnect
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/cgi.exe -d \"delay=0&_f_no_write_config=1&cmd_btn=Disconnect&exec_cgis=StaM&ret_url=%2Findex.stm%3Ftitle%3DStatus-Modem\" -b \"cookies.txt\"");
                    // Now connect
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/cgi-bin/cgi.exe -d \"delay=0&_f_no_write_config=1&cmd_btn=Connect&exec_cgis=StaM&ret_url=%2Findex.stm%3Ftitle%3DStatus-Modem\" -b \"cookies.txt\"");
                    // Sleep for a lil bit
                    System.Threading.Thread.Sleep(8000);
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(15000);
                }
            }
            // Huawei/Echolife HG510
            if(settings.modemType == "Huawei/Echolife HG510")
            {
                if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/rebootinfo.cgi");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
                else if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/rebootinfo.cgi");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
            }
            // Exper ECM-01
            if (settings.modemType == "Exper ECM-01")
            {
                if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/rebootinfo.cgi");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
                else if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/rebootinfo.cgi");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
            }
            // Thomson TG782
            if (settings.modemType == "Thomson TG782")
            {
#if FIREFOX
                if (started == 0) ie = new FireFox("about:blank");
#endif
#if IE
                if (started == 0) ie = new IE("about:blank", true);
#endif
                ie.DialogWatcher.Add(new WatiN.Core.DialogHandlers.LogonDialogHandler(settings.modemUsername, settings.modemPassword));
                ie.GoTo("http://" + settings.modemAddress);
                ie.Button(Find.ByValue("Disconnect")).Click();
                // Sleep for a lil bit
                System.Threading.Thread.Sleep(8000);
                ie.Refresh();
                ie.Button(Find.ByValue("Connect")).Click();
                // Allow the IP to refresh
                System.Threading.Thread.Sleep(15000);
            }
            // D-Link DKT-710
            if (settings.modemType == "D-Link DKT-710")
            {
                if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemAddress + "/reboot.html");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
                else if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/reboot.html");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
            }
            // Huawei Mobile Connect - 3G
            if (settings.modemType == "Huawei Mobile Connect - 3G")
            {
                if (settings.modemUsername == "" && settings.modemPassword == "")
                {
                    IntPtr wndHwnd = FindWindowByCaption(IntPtr.Zero, "Vodafone Mobile Connect Lite");
                    IntPtr disHwnd = FindWindowEx(wndHwnd, IntPtr.Zero, "", "Disconnect");
                    uint BM_CLICK = 0x00F5;
                    SendMessage(disHwnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                }
                else if (settings.modemUsername != "" && settings.modemPassword != "")
                {
                    // Restart
                    System.Diagnostics.Process.Start("curl.exe", "http://" + settings.modemUsername + ":" + settings.modemPassword + "@" + settings.modemAddress + "/reboot.html");
                    // Allow the IP to refresh
                    System.Threading.Thread.Sleep(20000);
                }
            }
        }

        // Renew IP using connection
        private void RenewIPConnection()
        {
            // First disconnect
            object ws = IWshRuntimeLibrary.WshWindowStyle.WshHide;
            object wt = IWshRuntimeLibrary.Tristate.TristateFalse;
            WshShell shell = new WshShell();
            shell.Run("rasdial \"" + settings.connectionName + "\" /DISCONNECT", ref ws, ref wt);
            System.Threading.Thread.Sleep(8000);

            // Now connect
            shell.Run("rasdial \"" + settings.connectionName + "\" " + settings.connectionUsername + " " + settings.connectionPassword, ref ws, ref wt);
            System.Threading.Thread.Sleep(8000);
        }

        public static string[] StringBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                        iEnd += strEnd.Length;

                    result[0] = strSource.Substring(0, iEnd);

                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            return result;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(started == 1) ie.Close();
        }

        private void testDisconnetNetwork_Click(object sender, EventArgs e)
        {
            object ws = IWshRuntimeLibrary.WshWindowStyle.WshHide;
            object wt = IWshRuntimeLibrary.Tristate.TristateFalse;
            WshShell shell = new WshShell();
            shell.Run("rasdial \"" + connectionName.Text + "\" /DISCONNECT", ref ws, ref wt);
        }

        private void testConnectNetwork_Click(object sender, EventArgs e)
        {
            object ws = IWshRuntimeLibrary.WshWindowStyle.WshHide;
            object wt = IWshRuntimeLibrary.Tristate.TristateFalse;
            WshShell shell = new WshShell();

            // Now connect
            shell.Run("rasdial \"" + connectionName.Text + "\" " + connectionUsername.Text + " " + connectionPassword.Text, ref ws, ref wt);
        }

        private void checkModem_CheckedChanged(object sender, EventArgs e)
        {
            if(checkModem.Checked)
                groupBox4.Enabled = true;
            else
                groupBox4.Enabled = false;
        }

        private void checkConnection_CheckedChanged(object sender, EventArgs e)
        {
            if (checkConnection.Checked)
                groupBox5.Enabled = true;
            else
                groupBox5.Enabled = false;
        }

        public string[] UserAgents = {
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.13 Chrome/0.2.149.27 Safari/525.13",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 Chrome/1.0.154.53 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.3) Gecko/20041002 Firefox/0.10.1",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1) Gecko/20090624 Firefox/3.5",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.13) Gecko/20060410 Firefox/1.0.8",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2a1pre) Gecko/20090403 Minefield/3.6a1pre",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1a2pre) Gecko/2008073000 Shredder/3.0a2pre ThunderBrowse/3.2.1.8 ",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.1.2) Gecko/20090804 Shiretoko/3.5.2",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8) Gecko/20051111 Firefox/1.5 BAVM/1.0.0",
            "Mozilla/5.0 (X11; U; GNU/kFreeBSD i686; en-US; rv:1.8.1.16) Gecko/20080702 Iceape/1.1.11",
            "Mozilla/5.0 (X11; U; GNU/kFreeBSD i686; en-US; rv:1.9.0.1) Gecko/2008071502 Iceweasel/3.0.1",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.1) Gecko/2008072716 IceCat/3.0.1-g1",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.0.1) Gecko/2008071420 Iceweasel/3.0.1",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.8) Gecko/20071008 Iceape/1.1.5",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.1) Gecko/20061205 Iceweasel/2.0.0.1)",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.0.4)Gecko/20060620 Iceweasel/1.5.0.4-g1",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; InfoPath.1)",
            "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1;)",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Business Everywhere 7.1.2; GTB6; .NET CLR 1.0.3705; .NET CLR 1.1.4322; Media Center PC 4.0)",
            "Mozilla/4.0 (compatible; MSIE 5.5; Windows 98)",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1;)",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; Tablet PC 2.0) Sleipnir/2.8.3",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.12) Gecko/20080219 Firefox/2.0.0.12 Navigator/9.0.0.6",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.8pre) Gecko/20071019 Firefox/2.0.0.8 Navigator/9.0.0.1",
            "Mozilla/5.0 (X11; U; Linux i686) Gecko/20071127 Firefox/2.0.0.11",
            "Opera/9.60 (J2ME/MIDP; Opera Mini/4.2.13337/458; U; en) Presto/2.2.0",
            "Opera/9.60 (J2ME/MIDP; Opera Mini/4.1.11320/608; U; en) Presto/2.2.0",
            "Opera/10.00 (X11; Linux i686 ; U; en) Presto/2.2.0",
            "Opera/9.62 (Windows NT 5.1; U; en) Presto/2.1.1",
            "Opera/9.60 (X11; Linux i686; U; en) Presto/2.1.1",
            "Opera/9.52 (Windows NT 5.1; U; en)",
            "Opera/9.25 (Windows NT 6.0; U; en)",
            "Opera/9.20 (Macintosh; Intel Mac OS X; U; en)",
            "Opera/9.02 (Windows NT 5.0; U; en)",
            "Opera/9.00 (Windows NT 4.0; U; en)",
            "Opera/9.00 (X11; Linux i686; U; en)",
            "Opera/8.00 (Windows NT 5.1; U; en)",
            "Opera/8.00 (Windows NT 5.1; U; en)"
            };

        public double GetWorkBoosterCost(int booster)
        {
            double boosterCost = 0.00;
            switch (booster)
            {
                case 2:
                    boosterCost = 0.19;
                    break;
                case 3:
                    boosterCost = 0.79;
                    break;
                case 4:
                    boosterCost = 0.99;
                    break;
            }

            return boosterCost;
        }

        public string GetSource(string url)
        {
            string direction;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            direction = stream.ReadToEnd();
            stream.Close();
            response.Close();

            return direction;
        }

        public string GetMyIP()
        {
            string direction;
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            WebResponse response = request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            direction = stream.ReadToEnd();
            stream.Close();
            response.Close();

            // Current IP Address: 109.175.82.228
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }

        public void ResetIE()
        {
#if IE
            ie.ClearCookies();
            ie.ClearCache();
            ChangeUserAgent();
            ie.ForceClose();
            ie = new IE("about:blank", true);
#endif
#if FIREFOX
            ie.Close();
            ie = new FireFox("about:blank");
#endif
            // Thomson TG782
            if (settings.modemType == "Thomson TG782")
            {
                ie.DialogWatcher.Add(new WatiN.Core.DialogHandlers.LogonDialogHandler(settings.modemUsername, settings.modemPassword));
            }
#if IE
            ie.ClearCookies();
            ie.ClearCache();
#endif
        }

        private void testModem_Click(object sender, EventArgs e)
        {
            LoadSettings();
            RenewIPModem();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(started == 1)
            {
                ie.Close();                
            }
        }
    }
}