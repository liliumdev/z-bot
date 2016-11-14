namespace Z_Bot
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listAccounts = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.jobOfferLink = new System.Windows.Forms.TextBox();
            this.checkGetJobIfNoRaw = new System.Windows.Forms.CheckBox();
            this.eatHowManyBeforeWork = new System.Windows.Forms.TextBox();
            this.doEatBeforeWork = new System.Windows.Forms.CheckBox();
            this.workBooster = new System.Windows.Forms.TextBox();
            this.trainBooster = new System.Windows.Forms.TextBox();
            this.doEatFoodAfterTrain = new System.Windows.Forms.CheckBox();
            this.doGetJob = new System.Windows.Forms.CheckBox();
            this.doEatFoodAfterWork = new System.Windows.Forms.CheckBox();
            this.buyWhat = new System.Windows.Forms.ComboBox();
            this.buyAmount = new System.Windows.Forms.TextBox();
            this.doBuy = new System.Windows.Forms.CheckBox();
            this.fightTimes = new System.Windows.Forms.TextBox();
            this.doSub = new System.Windows.Forms.CheckBox();
            this.voteId = new System.Windows.Forms.TextBox();
            this.doVote = new System.Windows.Forms.CheckBox();
            this.battleId = new System.Windows.Forms.TextBox();
            this.doFight = new System.Windows.Forms.CheckBox();
            this.doTrain = new System.Windows.Forms.CheckBox();
            this.doWork = new System.Windows.Forms.CheckBox();
            this.modemAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.testConnectNetwork = new System.Windows.Forms.Button();
            this.testDisconnetNetwork = new System.Windows.Forms.Button();
            this.connectionName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.connectionPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.modemPassword = new System.Windows.Forms.TextBox();
            this.modemUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chosenModem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.helpBtn = new System.Windows.Forms.Button();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabactions = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.donateID = new System.Windows.Forms.TextBox();
            this.donateHowMuch = new System.Windows.Forms.TextBox();
            this.checkDonate = new System.Windows.Forms.CheckBox();
            this.donateCurrency = new System.Windows.Forms.TextBox();
            this.skipAccountsCaptcha = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.commentText = new System.Windows.Forms.TextBox();
            this.checkComment = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkShowIE = new System.Windows.Forms.CheckBox();
            this.showUpdates = new System.Windows.Forms.CheckBox();
            this.checkProxylist = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.testModem = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.hotSpotLink = new System.Windows.Forms.TextBox();
            this.checkHotspot = new System.Windows.Forms.CheckBox();
            this.checkConnection = new System.Windows.Forms.CheckBox();
            this.checkModem = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabactions.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // listAccounts
            // 
            this.listAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listAccounts.Location = new System.Drawing.Point(6, 19);
            this.listAccounts.Name = "listAccounts";
            this.listAccounts.Size = new System.Drawing.Size(532, 106);
            this.listAccounts.TabIndex = 0;
            this.listAccounts.UseCompatibleStateImageBehavior = false;
            this.listAccounts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Username";
            this.columnHeader1.Width = 264;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Password";
            this.columnHeader2.Width = 264;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listAccounts);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounts";
            // 
            // jobOfferLink
            // 
            this.jobOfferLink.Location = new System.Drawing.Point(26, 105);
            this.jobOfferLink.MaxLength = 500;
            this.jobOfferLink.Name = "jobOfferLink";
            this.jobOfferLink.Size = new System.Drawing.Size(155, 20);
            this.jobOfferLink.TabIndex = 21;
            // 
            // checkGetJobIfNoRaw
            // 
            this.checkGetJobIfNoRaw.AutoSize = true;
            this.checkGetJobIfNoRaw.Location = new System.Drawing.Point(8, 131);
            this.checkGetJobIfNoRaw.Name = "checkGetJobIfNoRaw";
            this.checkGetJobIfNoRaw.Size = new System.Drawing.Size(174, 17);
            this.checkGetJobIfNoRaw.TabIndex = 20;
            this.checkGetJobIfNoRaw.Text = "Get another job if no raw/salary";
            this.checkGetJobIfNoRaw.UseVisualStyleBackColor = true;
            // 
            // eatHowManyBeforeWork
            // 
            this.eatHowManyBeforeWork.Location = new System.Drawing.Point(139, 17);
            this.eatHowManyBeforeWork.MaxLength = 2;
            this.eatHowManyBeforeWork.Name = "eatHowManyBeforeWork";
            this.eatHowManyBeforeWork.Size = new System.Drawing.Size(50, 20);
            this.eatHowManyBeforeWork.TabIndex = 18;
            // 
            // doEatBeforeWork
            // 
            this.doEatBeforeWork.AutoSize = true;
            this.doEatBeforeWork.Location = new System.Drawing.Point(8, 19);
            this.doEatBeforeWork.Name = "doEatBeforeWork";
            this.doEatBeforeWork.Size = new System.Drawing.Size(125, 17);
            this.doEatBeforeWork.TabIndex = 17;
            this.doEatBeforeWork.Text = "Eat food before work";
            this.doEatBeforeWork.UseVisualStyleBackColor = true;
            // 
            // workBooster
            // 
            this.workBooster.Location = new System.Drawing.Point(64, 37);
            this.workBooster.MaxLength = 1;
            this.workBooster.Name = "workBooster";
            this.workBooster.Size = new System.Drawing.Size(50, 20);
            this.workBooster.TabIndex = 16;
            // 
            // trainBooster
            // 
            this.trainBooster.Location = new System.Drawing.Point(62, 17);
            this.trainBooster.MaxLength = 1;
            this.trainBooster.Name = "trainBooster";
            this.trainBooster.Size = new System.Drawing.Size(50, 20);
            this.trainBooster.TabIndex = 15;
            // 
            // doEatFoodAfterTrain
            // 
            this.doEatFoodAfterTrain.AutoSize = true;
            this.doEatFoodAfterTrain.Location = new System.Drawing.Point(8, 39);
            this.doEatFoodAfterTrain.Name = "doEatFoodAfterTrain";
            this.doEatFoodAfterTrain.Size = new System.Drawing.Size(113, 17);
            this.doEatFoodAfterTrain.TabIndex = 14;
            this.doEatFoodAfterTrain.Text = "Eat food after train";
            this.doEatFoodAfterTrain.UseVisualStyleBackColor = true;
            // 
            // doGetJob
            // 
            this.doGetJob.AutoSize = true;
            this.doGetJob.Location = new System.Drawing.Point(8, 85);
            this.doGetJob.Name = "doGetJob";
            this.doGetJob.Size = new System.Drawing.Size(137, 17);
            this.doGetJob.TabIndex = 13;
            this.doGetJob.Text = "Get a job if unemployed";
            this.doGetJob.UseVisualStyleBackColor = true;
            // 
            // doEatFoodAfterWork
            // 
            this.doEatFoodAfterWork.AutoSize = true;
            this.doEatFoodAfterWork.Location = new System.Drawing.Point(8, 62);
            this.doEatFoodAfterWork.Name = "doEatFoodAfterWork";
            this.doEatFoodAfterWork.Size = new System.Drawing.Size(116, 17);
            this.doEatFoodAfterWork.TabIndex = 12;
            this.doEatFoodAfterWork.Text = "Eat food after work";
            this.doEatFoodAfterWork.UseVisualStyleBackColor = true;
            // 
            // buyWhat
            // 
            this.buyWhat.DropDownHeight = 200;
            this.buyWhat.DropDownWidth = 200;
            this.buyWhat.FormattingEnabled = true;
            this.buyWhat.IntegralHeight = false;
            this.buyWhat.Items.AddRange(new object[] {
            "Food Q1",
            "Food Q2",
            "Food Q3",
            "Food Q4",
            "Food Q5",
            "Weapon Q1",
            "Weapon Q2",
            "Weapon Q3",
            "Weapon Q4",
            "Weapon Q5"});
            this.buyWhat.Location = new System.Drawing.Point(61, 16);
            this.buyWhat.Name = "buyWhat";
            this.buyWhat.Size = new System.Drawing.Size(119, 21);
            this.buyWhat.TabIndex = 11;
            // 
            // buyAmount
            // 
            this.buyAmount.Location = new System.Drawing.Point(190, 18);
            this.buyAmount.MaxLength = 300;
            this.buyAmount.Name = "buyAmount";
            this.buyAmount.Size = new System.Drawing.Size(50, 20);
            this.buyAmount.TabIndex = 10;
            // 
            // doBuy
            // 
            this.doBuy.AutoSize = true;
            this.doBuy.Location = new System.Drawing.Point(7, 18);
            this.doBuy.Name = "doBuy";
            this.doBuy.Size = new System.Drawing.Size(44, 17);
            this.doBuy.TabIndex = 8;
            this.doBuy.Text = "Buy";
            this.doBuy.UseVisualStyleBackColor = true;
            // 
            // fightTimes
            // 
            this.fightTimes.Location = new System.Drawing.Point(130, 17);
            this.fightTimes.MaxLength = 300;
            this.fightTimes.Name = "fightTimes";
            this.fightTimes.Size = new System.Drawing.Size(50, 20);
            this.fightTimes.TabIndex = 7;
            // 
            // doSub
            // 
            this.doSub.AutoSize = true;
            this.doSub.Location = new System.Drawing.Point(7, 42);
            this.doSub.Name = "doSub";
            this.doSub.Size = new System.Drawing.Size(45, 17);
            this.doSub.TabIndex = 6;
            this.doSub.Text = "Sub";
            this.doSub.UseVisualStyleBackColor = true;
            // 
            // voteId
            // 
            this.voteId.Location = new System.Drawing.Point(61, 17);
            this.voteId.MaxLength = 300;
            this.voteId.Name = "voteId";
            this.voteId.Size = new System.Drawing.Size(64, 20);
            this.voteId.TabIndex = 5;
            // 
            // doVote
            // 
            this.doVote.AutoSize = true;
            this.doVote.Location = new System.Drawing.Point(7, 19);
            this.doVote.Name = "doVote";
            this.doVote.Size = new System.Drawing.Size(48, 17);
            this.doVote.TabIndex = 4;
            this.doVote.Text = "Vote";
            this.doVote.UseVisualStyleBackColor = true;
            // 
            // battleId
            // 
            this.battleId.Location = new System.Drawing.Point(60, 17);
            this.battleId.MaxLength = 300;
            this.battleId.Name = "battleId";
            this.battleId.Size = new System.Drawing.Size(64, 20);
            this.battleId.TabIndex = 3;
            // 
            // doFight
            // 
            this.doFight.AutoSize = true;
            this.doFight.Location = new System.Drawing.Point(7, 19);
            this.doFight.Name = "doFight";
            this.doFight.Size = new System.Drawing.Size(49, 17);
            this.doFight.TabIndex = 2;
            this.doFight.Text = "Fight";
            this.doFight.UseVisualStyleBackColor = true;
            // 
            // doTrain
            // 
            this.doTrain.AutoSize = true;
            this.doTrain.Checked = true;
            this.doTrain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doTrain.Location = new System.Drawing.Point(8, 19);
            this.doTrain.Name = "doTrain";
            this.doTrain.Size = new System.Drawing.Size(50, 17);
            this.doTrain.TabIndex = 1;
            this.doTrain.Text = "Train";
            this.doTrain.UseVisualStyleBackColor = true;
            // 
            // doWork
            // 
            this.doWork.AutoSize = true;
            this.doWork.Checked = true;
            this.doWork.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doWork.Location = new System.Drawing.Point(8, 39);
            this.doWork.Name = "doWork";
            this.doWork.Size = new System.Drawing.Size(52, 17);
            this.doWork.TabIndex = 0;
            this.doWork.Text = "Work";
            this.doWork.UseVisualStyleBackColor = true;
            // 
            // modemAddress
            // 
            this.modemAddress.Location = new System.Drawing.Point(110, 130);
            this.modemAddress.Name = "modemAddress";
            this.modemAddress.Size = new System.Drawing.Size(136, 20);
            this.modemAddress.TabIndex = 8;
            this.modemAddress.Text = "192.168.1.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Modem address:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.testConnectNetwork);
            this.groupBox5.Controls.Add(this.testDisconnetNetwork);
            this.groupBox5.Controls.Add(this.connectionName);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.connectionPassword);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.connectionUsername);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(273, 38);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(240, 181);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Connection details";
            // 
            // testConnectNetwork
            // 
            this.testConnectNetwork.Location = new System.Drawing.Point(14, 136);
            this.testConnectNetwork.Name = "testConnectNetwork";
            this.testConnectNetwork.Size = new System.Drawing.Size(220, 32);
            this.testConnectNetwork.TabIndex = 16;
            this.testConnectNetwork.Text = "Test connect";
            this.testConnectNetwork.UseVisualStyleBackColor = true;
            this.testConnectNetwork.Click += new System.EventHandler(this.testConnectNetwork_Click);
            // 
            // testDisconnetNetwork
            // 
            this.testDisconnetNetwork.Location = new System.Drawing.Point(13, 98);
            this.testDisconnetNetwork.Name = "testDisconnetNetwork";
            this.testDisconnetNetwork.Size = new System.Drawing.Size(220, 32);
            this.testDisconnetNetwork.TabIndex = 15;
            this.testDisconnetNetwork.Text = "Test disconnect";
            this.testDisconnetNetwork.UseVisualStyleBackColor = true;
            this.testDisconnetNetwork.Click += new System.EventHandler(this.testDisconnetNetwork_Click);
            // 
            // connectionName
            // 
            this.connectionName.Location = new System.Drawing.Point(65, 18);
            this.connectionName.Name = "connectionName";
            this.connectionName.Size = new System.Drawing.Size(169, 20);
            this.connectionName.TabIndex = 14;
            this.connectionName.Text = "DSLConnection";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Name:";
            // 
            // connectionPassword
            // 
            this.connectionPassword.Location = new System.Drawing.Point(65, 71);
            this.connectionPassword.Name = "connectionPassword";
            this.connectionPassword.Size = new System.Drawing.Size(169, 20);
            this.connectionPassword.TabIndex = 12;
            this.connectionPassword.Text = "admin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Password";
            // 
            // connectionUsername
            // 
            this.connectionUsername.Location = new System.Drawing.Point(65, 45);
            this.connectionUsername.Name = "connectionUsername";
            this.connectionUsername.Size = new System.Drawing.Size(169, 20);
            this.connectionUsername.TabIndex = 10;
            this.connectionUsername.Text = "admin";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Username:";
            // 
            // modemPassword
            // 
            this.modemPassword.Location = new System.Drawing.Point(109, 104);
            this.modemPassword.Name = "modemPassword";
            this.modemPassword.Size = new System.Drawing.Size(137, 20);
            this.modemPassword.TabIndex = 5;
            this.modemPassword.Text = "admin";
            // 
            // modemUsername
            // 
            this.modemUsername.Location = new System.Drawing.Point(110, 78);
            this.modemUsername.Name = "modemUsername";
            this.modemUsername.Size = new System.Drawing.Size(136, 20);
            this.modemUsername.TabIndex = 4;
            this.modemUsername.Text = "admin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Modem password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Modem username:";
            // 
            // chosenModem
            // 
            this.chosenModem.FormattingEnabled = true;
            this.chosenModem.ItemHeight = 13;
            this.chosenModem.Items.AddRange(new object[] {
            "Paradigm",
            "Speedtouch 516i",
            "Speedtouch 546v6",
            "Speedtouch 780WL",
            "TP-Link TD-8817",
            "Alice Gate VoIP",
            "Linksys AM200",
            "Thomson TG782",
            "Huawei/Echolife HG510",
            "D-Link DKT-710",
            "D-Link DSL-584T",
            "Exper ECM-01",
            "Airties 5450",
            "Sagemcom Optima 1704",
            "Huawei Mobile Connect - 3G"});
            this.chosenModem.Location = new System.Drawing.Point(8, 48);
            this.chosenModem.Name = "chosenModem";
            this.chosenModem.Size = new System.Drawing.Size(238, 21);
            this.chosenModem.TabIndex = 1;
            this.chosenModem.Text = "Paradigm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please choose your modem below :";
            // 
            // startBtn
            // 
            this.startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startBtn.Location = new System.Drawing.Point(12, 451);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(417, 47);
            this.startBtn.TabIndex = 4;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // helpBtn
            // 
            this.helpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBtn.Location = new System.Drawing.Point(435, 451);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(120, 47);
            this.helpBtn.TabIndex = 5;
            this.helpBtn.Text = "Help";
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(12, 504);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.statusBox.Size = new System.Drawing.Size(543, 64);
            this.statusBox.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabactions);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 149);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(544, 296);
            this.tabControl1.TabIndex = 7;
            // 
            // tabactions
            // 
            this.tabactions.Controls.Add(this.groupBox8);
            this.tabactions.Controls.Add(this.groupBox7);
            this.tabactions.Controls.Add(this.groupBox6);
            this.tabactions.Controls.Add(this.groupBox3);
            this.tabactions.Controls.Add(this.groupBox2);
            this.tabactions.Location = new System.Drawing.Point(4, 22);
            this.tabactions.Name = "tabactions";
            this.tabactions.Padding = new System.Windows.Forms.Padding(3);
            this.tabactions.Size = new System.Drawing.Size(536, 270);
            this.tabactions.TabIndex = 0;
            this.tabactions.Text = "Actions";
            this.tabactions.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.donateID);
            this.groupBox8.Controls.Add(this.donateHowMuch);
            this.groupBox8.Controls.Add(this.checkDonate);
            this.groupBox8.Controls.Add(this.donateCurrency);
            this.groupBox8.Controls.Add(this.skipAccountsCaptcha);
            this.groupBox8.Controls.Add(this.doBuy);
            this.groupBox8.Controls.Add(this.buyWhat);
            this.groupBox8.Controls.Add(this.buyAmount);
            this.groupBox8.Location = new System.Drawing.Point(237, 163);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(274, 101);
            this.groupBox8.TabIndex = 26;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Other";
            // 
            // donateID
            // 
            this.donateID.Location = new System.Drawing.Point(74, 43);
            this.donateID.MaxLength = 300;
            this.donateID.Name = "donateID";
            this.donateID.Size = new System.Drawing.Size(50, 20);
            this.donateID.TabIndex = 24;
            // 
            // donateHowMuch
            // 
            this.donateHowMuch.Location = new System.Drawing.Point(186, 43);
            this.donateHowMuch.MaxLength = 300;
            this.donateHowMuch.Name = "donateHowMuch";
            this.donateHowMuch.Size = new System.Drawing.Size(67, 20);
            this.donateHowMuch.TabIndex = 23;
            // 
            // checkDonate
            // 
            this.checkDonate.AutoSize = true;
            this.checkDonate.Location = new System.Drawing.Point(7, 43);
            this.checkDonate.Name = "checkDonate";
            this.checkDonate.Size = new System.Drawing.Size(61, 17);
            this.checkDonate.TabIndex = 21;
            this.checkDonate.Text = "Donate";
            this.checkDonate.UseVisualStyleBackColor = true;
            // 
            // donateCurrency
            // 
            this.donateCurrency.Location = new System.Drawing.Point(130, 43);
            this.donateCurrency.MaxLength = 300;
            this.donateCurrency.Name = "donateCurrency";
            this.donateCurrency.Size = new System.Drawing.Size(50, 20);
            this.donateCurrency.TabIndex = 22;
            this.donateCurrency.Text = "62";
            // 
            // skipAccountsCaptcha
            // 
            this.skipAccountsCaptcha.AutoSize = true;
            this.skipAccountsCaptcha.Location = new System.Drawing.Point(7, 69);
            this.skipAccountsCaptcha.Name = "skipAccountsCaptcha";
            this.skipAccountsCaptcha.Size = new System.Drawing.Size(202, 17);
            this.skipAccountsCaptcha.TabIndex = 20;
            this.skipAccountsCaptcha.Text = "Skip accounts which require captcha";
            this.skipAccountsCaptcha.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.commentText);
            this.groupBox7.Controls.Add(this.checkComment);
            this.groupBox7.Controls.Add(this.doVote);
            this.groupBox7.Controls.Add(this.voteId);
            this.groupBox7.Controls.Add(this.doSub);
            this.groupBox7.Location = new System.Drawing.Point(237, 63);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(276, 96);
            this.groupBox7.TabIndex = 25;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Article";
            // 
            // commentText
            // 
            this.commentText.Location = new System.Drawing.Point(83, 62);
            this.commentText.MaxLength = 300;
            this.commentText.Name = "commentText";
            this.commentText.Size = new System.Drawing.Size(176, 20);
            this.commentText.TabIndex = 8;
            // 
            // checkComment
            // 
            this.checkComment.AutoSize = true;
            this.checkComment.Location = new System.Drawing.Point(7, 65);
            this.checkComment.Name = "checkComment";
            this.checkComment.Size = new System.Drawing.Size(70, 17);
            this.checkComment.TabIndex = 7;
            this.checkComment.Text = "Comment";
            this.checkComment.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.doFight);
            this.groupBox6.Controls.Add(this.battleId);
            this.groupBox6.Controls.Add(this.fightTimes);
            this.groupBox6.Location = new System.Drawing.Point(237, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(277, 52);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Battles";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.doTrain);
            this.groupBox3.Controls.Add(this.doEatFoodAfterTrain);
            this.groupBox3.Controls.Add(this.trainBooster);
            this.groupBox3.Location = new System.Drawing.Point(9, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 64);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Train";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.jobOfferLink);
            this.groupBox2.Controls.Add(this.doEatBeforeWork);
            this.groupBox2.Controls.Add(this.checkGetJobIfNoRaw);
            this.groupBox2.Controls.Add(this.doWork);
            this.groupBox2.Controls.Add(this.eatHowManyBeforeWork);
            this.groupBox2.Controls.Add(this.workBooster);
            this.groupBox2.Controls.Add(this.doGetJob);
            this.groupBox2.Controls.Add(this.doEatFoodAfterWork);
            this.groupBox2.Location = new System.Drawing.Point(9, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 154);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Work";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkShowIE);
            this.tabPage2.Controls.Add(this.showUpdates);
            this.tabPage2.Controls.Add(this.checkProxylist);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.hotSpotLink);
            this.tabPage2.Controls.Add(this.checkHotspot);
            this.tabPage2.Controls.Add(this.checkConnection);
            this.tabPage2.Controls.Add(this.checkModem);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(536, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Miscellaneous";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkShowIE
            // 
            this.checkShowIE.AutoSize = true;
            this.checkShowIE.Checked = true;
            this.checkShowIE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowIE.Location = new System.Drawing.Point(376, 231);
            this.checkShowIE.Name = "checkShowIE";
            this.checkShowIE.Size = new System.Drawing.Size(105, 17);
            this.checkShowIE.TabIndex = 13;
            this.checkShowIE.Text = "Show IE window";
            this.checkShowIE.UseVisualStyleBackColor = true;
            // 
            // showUpdates
            // 
            this.showUpdates.AutoSize = true;
            this.showUpdates.Checked = true;
            this.showUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showUpdates.Location = new System.Drawing.Point(273, 231);
            this.showUpdates.Name = "showUpdates";
            this.showUpdates.Size = new System.Drawing.Size(97, 17);
            this.showUpdates.TabIndex = 12;
            this.showUpdates.Text = "Show updates ";
            this.showUpdates.UseVisualStyleBackColor = true;
            // 
            // checkProxylist
            // 
            this.checkProxylist.AutoSize = true;
            this.checkProxylist.Location = new System.Drawing.Point(276, 11);
            this.checkProxylist.Name = "checkProxylist";
            this.checkProxylist.Size = new System.Drawing.Size(65, 17);
            this.checkProxylist.TabIndex = 11;
            this.checkProxylist.Text = "proxy.txt";
            this.checkProxylist.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.testModem);
            this.groupBox4.Controls.Add(this.modemAddress);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.chosenModem);
            this.groupBox4.Controls.Add(this.modemPassword);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.modemUsername);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(8, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(252, 212);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Modem details";
            // 
            // testModem
            // 
            this.testModem.Location = new System.Drawing.Point(19, 165);
            this.testModem.Name = "testModem";
            this.testModem.Size = new System.Drawing.Size(220, 32);
            this.testModem.TabIndex = 17;
            this.testModem.Text = "Test";
            this.testModem.UseVisualStyleBackColor = true;
            this.testModem.Click += new System.EventHandler(this.testModem_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "IP renewing method :";
            // 
            // hotSpotLink
            // 
            this.hotSpotLink.Location = new System.Drawing.Point(449, 10);
            this.hotSpotLink.Name = "hotSpotLink";
            this.hotSpotLink.Size = new System.Drawing.Size(64, 20);
            this.hotSpotLink.TabIndex = 5;
            // 
            // checkHotspot
            // 
            this.checkHotspot.AutoSize = true;
            this.checkHotspot.Location = new System.Drawing.Point(348, 11);
            this.checkHotspot.Name = "checkHotspot";
            this.checkHotspot.Size = new System.Drawing.Size(95, 17);
            this.checkHotspot.TabIndex = 4;
            this.checkHotspot.Text = "Hotspot Shield";
            this.checkHotspot.UseVisualStyleBackColor = true;
            // 
            // checkConnection
            // 
            this.checkConnection.AutoSize = true;
            this.checkConnection.Location = new System.Drawing.Point(190, 12);
            this.checkConnection.Name = "checkConnection";
            this.checkConnection.Size = new System.Drawing.Size(80, 17);
            this.checkConnection.TabIndex = 2;
            this.checkConnection.Text = "Connection";
            this.checkConnection.UseVisualStyleBackColor = true;
            this.checkConnection.CheckedChanged += new System.EventHandler(this.checkConnection_CheckedChanged);
            // 
            // checkModem
            // 
            this.checkModem.AutoSize = true;
            this.checkModem.Checked = true;
            this.checkModem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkModem.Location = new System.Drawing.Point(123, 12);
            this.checkModem.Name = "checkModem";
            this.checkModem.Size = new System.Drawing.Size(61, 17);
            this.checkModem.TabIndex = 1;
            this.checkModem.Text = "Modem";
            this.checkModem.UseVisualStyleBackColor = true;
            this.checkModem.CheckedChanged += new System.EventHandler(this.checkModem_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 580);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(584, 580);
            this.Name = "Form1";
            this.Text = "Z-Bot";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabactions.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listAccounts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox doSub;
        private System.Windows.Forms.TextBox voteId;
        private System.Windows.Forms.CheckBox doVote;
        private System.Windows.Forms.TextBox battleId;
        private System.Windows.Forms.CheckBox doFight;
        private System.Windows.Forms.CheckBox doTrain;
        private System.Windows.Forms.CheckBox doWork;
        private System.Windows.Forms.TextBox modemPassword;
        private System.Windows.Forms.TextBox modemUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox chosenModem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.TextBox fightTimes;
        private System.Windows.Forms.ComboBox buyWhat;
        private System.Windows.Forms.TextBox buyAmount;
        private System.Windows.Forms.CheckBox doBuy;
        private System.Windows.Forms.CheckBox doEatFoodAfterWork;
        private System.Windows.Forms.TextBox modemAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox connectionPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox connectionUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox doGetJob;
        private System.Windows.Forms.TextBox connectionName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox doEatFoodAfterTrain;
        private System.Windows.Forms.TextBox workBooster;
        private System.Windows.Forms.TextBox trainBooster;
        private System.Windows.Forms.TextBox eatHowManyBeforeWork;
        private System.Windows.Forms.CheckBox doEatBeforeWork;
        private System.Windows.Forms.TextBox jobOfferLink;
        private System.Windows.Forms.CheckBox checkGetJobIfNoRaw;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabactions;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox commentText;
        private System.Windows.Forms.CheckBox checkComment;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox skipAccountsCaptcha;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox hotSpotLink;
        private System.Windows.Forms.CheckBox checkHotspot;
        private System.Windows.Forms.CheckBox checkConnection;
        private System.Windows.Forms.CheckBox checkModem;
        private System.Windows.Forms.CheckBox checkProxylist;
        private System.Windows.Forms.Button testConnectNetwork;
        private System.Windows.Forms.Button testDisconnetNetwork;
        private System.Windows.Forms.CheckBox showUpdates;
        private System.Windows.Forms.Button testModem;
        private System.Windows.Forms.CheckBox checkShowIE;
        private System.Windows.Forms.TextBox donateID;
        private System.Windows.Forms.TextBox donateHowMuch;
        private System.Windows.Forms.CheckBox checkDonate;
        private System.Windows.Forms.TextBox donateCurrency;
    }
}

