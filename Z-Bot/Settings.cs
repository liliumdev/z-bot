using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z_Bot
{
    public struct Settings
    {
        public bool eatBeforeWork;
        public bool showIE;
        public int eatBeforeWorkHowMuch;
        public bool work;
        public int workBooster;
        public bool eatAfterWork;
        public bool getJob;
        public string getJobOfferLink;
        public bool getJobIfNoRaw;
        public bool train;
        public int trainBooster;
        public bool eatAfterTrain;
        public bool fight;
        public int battleId;
        public int fightTimes;
        public bool vote;
        public int voteId;
        public bool sub;
        public bool buy;
        public string buyWhat;
        public int buyAmount;
        public int renewIpType;  // 1 == modem; 2 == connection; 3 == hotspot; 4 == proxy
        public string modemType;
        public string modemUsername;
        public string modemPassword;
        public string modemAddress;
        public string connectionName;
        public string connectionUsername;
        public string connectionPassword;
        public string hotSpotURL;
        public bool skipAccountsWithCaptcha;
        public bool comment;
        public string commentText;
        public bool donate;
        public string donateId;
        public string donateCurrency;
        public string donateHowMuch;
    }

    public struct Currency
    {
        public double amount;
        public string name;
    }
}
