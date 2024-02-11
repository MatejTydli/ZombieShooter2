using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Windows.Forms;
using static System.Windows.Forms.SystemInformation;

namespace ZombieShooter2
{
	class Form1 : Form
	{
		public FormPage menuPage, optionsPage, gamePage, shopPage, gameOverPage;
		public Label
		menuTitleLabel, menuStartLabel, menuOptionsLabel, menuExitLabel,
		optionsTitleLabel, optionsBackLabel, optionsSoundLabel, optionsExitLabel,
		shopTitleLabel, shopBackLabel, gameOverTitleLabel, gameOverBackLabel, gameOverWavesLabel,
		ammoLabel, coinsLabel, wavesLabel;
		public PictureBox
		gamePausePictureBox, gameShopPictureBox, ammoPictureBox, coinsPictureBox, shopAmmoPictureBox, shopHealPictureBox,
		gamePlatform_Floor, gamePlatform_1, gamePlatform_2, gamePlatform_3, gamePlatform_4;
		public Button shopAmmoBuyButton, shopHealBuyButton;
		public ProgressBar healtBar;

		readonly Font font = new Font("Microsoft YaHei UI", 35.00f, FontStyle.Bold);
		readonly Font fontSmall = new Font("Microsoft YaHei UI", 30.00f, FontStyle.Regular);

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);
		}

		public Form1()
		{
			InitializeComponent();
			InitializeMenuPage();
			InitializeOptionsPage();
			InitializeGamePage();
			InitializeShopPage();
			InitializeGameOverPage();

			this.ShowIcon = false;
			this.WindowState = FormWindowState.Normal;
   			this.Size = new Size(1382, 744);
			this.Name = "Main Form";
			this.KeyDown += Game.Input_KeyDown;
			this.KeyUp += Game.Input_KeyUp;
			this.MouseClick += (s, e) => { if (Game.gameState == GameState.Running) Game.player.Shoot(e.Location); };
			this.MouseMove += (s, e) => { if (Game.gameState == GameState.Running) Game.player.SetFacing(e.Location); };

			menuPage.LoadToForm(this);
		}

		void InitializeMenuPage()
		{
			menuTitleLabel = new Label() { Text = "Main Menu" };
			menuStartLabel = new Label() { Text = "Start" };
			menuOptionsLabel = new Label() { Text = "Options" };
			menuExitLabel = new Label() { Text = "Exit" };

			setupLabelsProperties(new Label[] { menuTitleLabel, menuStartLabel, menuOptionsLabel, menuExitLabel });
			setupLabelsMouseEvents(new Label[] { menuStartLabel, menuOptionsLabel, menuExitLabel });

			menuStartLabel.Click += MenuStartLabel_Click;
			menuOptionsLabel.Click += MenuOptionsLabel_Click;
			menuExitLabel.Click += (s, e) => Application.Exit();

			menuPage = new FormPage(
				new List<Control> { menuTitleLabel, menuStartLabel, menuOptionsLabel, menuExitLabel },
				Color.Cyan, "ZombieShooter2 - Main Menu", false);
		}

		void InitializeOptionsPage()
		{
			optionsTitleLabel = new Label() { Text = "Options" };
			optionsBackLabel = new Label() { Text = "Back to Game" };
			optionsSoundLabel = new Label() { Text = "Sounds: ON" };
			optionsExitLabel = new Label() { Text = "Exit to Main Menu" };

			setupLabelsProperties(new Label[] { optionsTitleLabel, optionsSoundLabel, optionsBackLabel, optionsExitLabel });
			setupLabelsMouseEvents(new Label[] { optionsBackLabel, optionsSoundLabel, optionsExitLabel });

			optionsBackLabel.Click += OptionsBackLabel_Click;
			optionsSoundLabel.Click += OptionsSoundLabel_Click;
			optionsExitLabel.Click += OptionsExitLabel_Click;

			optionsPage = new FormPage(
				new List<Control> { optionsTitleLabel, optionsBackLabel, optionsSoundLabel, optionsExitLabel },
				Color.DarkCyan, "ZombieShooter2 - Options", false);
		}

		void InitializeGamePage()
		{
			gamePausePictureBox = new PictureBox()
			{
				Location = new Point(0, 5),
				Image = Media.pauseImage,
				Size = Media.pauseImage.Size
			};
			gameShopPictureBox = new PictureBox()
			{
				Location = new Point(WorkingArea.Width - (Media.pauseImage.Width + 50), 5),
				Image = Media.shopImage,
				Size = Media.shopImage.Size
			};

			gamePausePictureBox.Click += GamePausePictureBox_Click;
			gameShopPictureBox.Click += GameShopPictureBox_Click;

			var gameBarSpace = 25;
			var gameBar = new PictureBox()
			{
				BackColor = Color.DarkGray,
				Size = new Size(WorkingArea.Width / 3, 50),
				Location = new Point(WorkingArea.Width / 3, 0)
			};

			healtBar = new ProgressBar() { Value = 100 };
			ammoPictureBox = new PictureBox() { Image = Media.ammoImage, Size = Media.ammoImage.Size };
			ammoLabel = new Label() { Text = Game.ammo.ToString(), Height = 50 };
			coinsPictureBox = new PictureBox() { Image = Media.coinsImage, Size = Media.coinsImage.Size };
			coinsLabel = new Label() { Text = "0", Height = 50 };

			gameBar.Controls.AddRange(new Control[] { healtBar, ammoPictureBox, ammoLabel, coinsPictureBox, coinsLabel });
			foreach (Control control in gameBar.Controls) control.BringToFront();

			healtBar.Location = new Point(gameBarSpace, (gameBar.Height - healtBar.Height) / 2);
			ammoPictureBox.Location = new Point(healtBar.Right + gameBarSpace, (gameBar.Height - ammoPictureBox.Height) / 2);
			ammoLabel.Font = fontSmall;
			ammoLabel.Location = new Point(ammoPictureBox.Right + gameBarSpace, 0);
			coinsPictureBox.Location = new Point(ammoPictureBox.Right +
				(int)Math.Round(ammoLabel.CreateGraphics().MeasureString(ammoLabel.Text, fontSmall).Width) + gameBarSpace * 2,
				(gameBar.Height - coinsPictureBox.Height) / 2);
			coinsLabel.Font = fontSmall;
			coinsLabel.Location = new Point(coinsPictureBox.Right + gameBarSpace, 0);

			wavesLabel = new Label() { Text = "waves: 1", Height = 50 };
			wavesLabel.Font = new Font("Microsoft YaHei UI", 18.00f, FontStyle.Regular);
			wavesLabel.Location = new Point((gameBar.Left - gamePausePictureBox.Right) / 2,
				(gameBar.Height - new Font("Microsoft YaHei UI", 18.00f, FontStyle.Regular).Height) / 2);

			gamePlatform_Floor = new PictureBox()
			{
				Size = new Size(WorkingArea.Width, 200),
				Location = new Point(0, WorkingArea.Height - 80),
				BackColor = Color.Gray,
				Name = "platform"
			};
			gamePlatform_1 = new PictureBox()
			{
				Size = new Size(200, 52),
				Location = new Point(WorkingArea.Width - 280, 520),
				BackColor = Color.Gray,
				Name = "platform"
			};
			gamePlatform_2 = new PictureBox()
			{
				Size = new Size(450, 52),
				Location = new Point(600, 400),
				BackColor = Color.Gray,
				Name = "platform"
			};
			gamePlatform_3 = new PictureBox()
			{
				Size = new Size(250, 52),
				Location = new Point(150, 500),
				BackColor = Color.Gray,
				Name = "platform"
			};
			gamePlatform_4 = new PictureBox()
			{
				Size = new Size(200, 52),
				Location = new Point(200, 300),
				BackColor = Color.Gray,
				Name = "platform"
			};

			gamePage = new FormPage(
				new List<Control> { gameBar, gamePausePictureBox, gameShopPictureBox, wavesLabel,
				gamePlatform_Floor, gamePlatform_1, gamePlatform_2, gamePlatform_3, gamePlatform_4 },
				Color.LightGray, "ZoombieShooter2 - Game", false);
		}

		void InitializeShopPage()
		{
			shopTitleLabel = new Label() { Text = "Shop" };
			shopBackLabel = new Label() { Text = "Back to Game" };

			setupLabelsProperties(new Control[] { shopTitleLabel, shopBackLabel });
			setupLabelsMouseEvents(new Label[] { shopBackLabel });

			shopBackLabel.MouseClick += ShopBackLabel_Click;
			shopBackLabel.Top = WorkingArea.Height - shopBackLabel.Top;

			shopAmmoPictureBox = new PictureBox()
			{
				Image = Media.ammoImage,
				Size = new Size(80, 80),
				SizeMode = PictureBoxSizeMode.StretchImage
			};
			shopHealPictureBox = new PictureBox()
			{
				Image = Media.healImage,
				Size = new Size(80, 80),
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			shopAmmoPictureBox.Location = new Point(WorkingArea.Width / 2 - shopAmmoPictureBox.Width / 2, shopTitleLabel.Bottom + 80);
			shopHealPictureBox.Location = new Point(shopAmmoPictureBox.Left, shopAmmoPictureBox.Bottom + 80);

			shopAmmoBuyButton = new Button()
			{
				Text = "Buy 20 ammo for 10 coins",
				BackColor = Color.Gold,
				AutoSize = true,
			};
			shopHealBuyButton = new Button()
			{
				Text = "Buy 50 HP for 60 coins",
				BackColor = Color.Gold,
				AutoSize = true,
			};

			shopAmmoBuyButton.Paint += (s,e) => shopAmmoBuyButton.Location = new Point(
				shopAmmoPictureBox.Left + shopAmmoPictureBox.Width / 2 - shopAmmoBuyButton.Width / 2,
				shopAmmoPictureBox.Bottom);

			shopHealBuyButton.Paint += (s,e) => shopHealBuyButton.Location = new Point(
				shopHealPictureBox.Left + shopAmmoPictureBox.Width / 2 - shopHealBuyButton.Width / 2,
				shopHealPictureBox.Bottom);

			shopAmmoBuyButton.Click += Game.BuyAmmo;
			shopHealBuyButton.Click += Game.BuyHeal;

			shopPage = new FormPage(
				new List<Control> { shopTitleLabel, shopBackLabel, shopAmmoPictureBox,
				shopHealPictureBox, shopAmmoBuyButton, shopHealBuyButton },
				Color.LightGray, "ZoombieShooter2 - Shop", false);
		}

		void InitializeGameOverPage()
		{
			gameOverTitleLabel = new Label() { Text = "Game Over!" };
			gameOverWavesLabel = new Label() { Text = "Waves survived: 0" };
			gameOverBackLabel = new Label() { Text = "Back to Main Menu" };

			setupLabelsProperties(new Control[] { gameOverTitleLabel, gameOverWavesLabel, gameOverBackLabel });
			setupLabelsMouseEvents(new Control[] { gameOverBackLabel });

			gameOverBackLabel.Click += (s, e) => menuPage.LoadToFormInstedCurrent(this, gameOverPage);

			gameOverPage = new FormPage(
				new List<Control> { gameOverTitleLabel, gameOverWavesLabel, gameOverBackLabel },
				Color.Red, "ZoombieShooter2 - GameOver", false);
		}

		void setupLabelsProperties(Control[] labels)
		{
			for (int i = 0; i < labels.Length; i++)
			{
				labels[i].ForeColor = Color.Black;
				labels[i].Font = font;
				labels[i].AutoSize = true;
				setControlLocation(labels[i], labels[i].CreateGraphics(), 25, i + 1);
			}
		}

		void setupLabelsMouseEvents(Control[] labels)
		{
			for (int i = 0; i < labels.Length; i++)
			{
				labels[i].MouseEnter += Label_MouseEnter;
				labels[i].MouseLeave += Label_MouseLeave;
			}
		}

		void setControlLocation(Control control, Graphics graphics, int space, int heightOrder)
		{
			control.Location = new Point(
				WorkingArea.Width / 2 - (int)Math.Round(graphics.MeasureString(control.Text, font).Width / 2),
				(int)Math.Round(graphics.MeasureString(control.Text, font).Height * heightOrder + space * heightOrder));
		}

		void Label_MouseEnter(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			label.BackColor = Color.Blue;
			label.ForeColor = Color.White;
		}

		void Label_MouseLeave(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			label.BackColor = Color.Transparent;
			label.ForeColor = Color.Black;
		}

		void MenuOptionsLabel_Click(object sender, EventArgs e)
		{
			optionsBackLabel.Visible = false;
			optionsBackLabel.Enabled = false;
			optionsPage.LoadToFormInstedCurrent(this, menuPage);
		}

		void MenuStartLabel_Click(object sender, EventArgs e)
		{
			gamePage.LoadToFormInstedCurrent(this, menuPage);
			Game.RunNew();
		}

		void OptionsExitLabel_Click(object sender, EventArgs e)
		{
			if (Game.gameState == GameState.NotRunning)
			{
				menuPage.LoadToFormInstedCurrent(this, optionsPage);
				optionsBackLabel.Visible = true;
				optionsBackLabel.Enabled = true;
			}
			else
			{
				menuPage.LoadToFormInstedCurrent(this, optionsPage);
				Game.Quit();
			}
		}

		void OptionsSoundLabel_Click(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			label.Text = label.Text == "Sounds: ON" ? "Sounds: OFF" : "Sounds: ON";
		}

		void OptionsBackLabel_Click(object sender, EventArgs e)
		{
			gamePage.LoadToFormInstedCurrent(this, optionsPage);
			Game.Run();
		}

		void GamePausePictureBox_Click(object sender, EventArgs e)
		{
			optionsPage.LoadToFormInstedCurrent(this, gamePage);
			Game.Pause();
		}

		void GameShopPictureBox_Click(object sender, EventArgs e)
		{
			if (Game.coins >= 10)
			{
				shopAmmoBuyButton.Enabled = true;
				shopAmmoBuyButton.BackColor = Color.Gold;
			}
			else
			{
				shopAmmoBuyButton.Enabled = false;
				shopAmmoBuyButton.BackColor = Color.DarkGray;
			}

			if (Game.coins >= 60)
			{
				shopHealBuyButton.Enabled = true;
				shopHealBuyButton.BackColor = Color.Gold;
			}
			else
			{
				shopHealBuyButton.Enabled = false;
				shopHealBuyButton.BackColor = Color.DarkGray;
			}

			shopPage.LoadToFormInstedCurrent(this, gamePage);
			Game.Pause();
		}

		void ShopBackLabel_Click(object sender, MouseEventArgs e)
		{
			gamePage.LoadToFormInstedCurrent(this, shopPage);
			Game.Run();
		}
	}
}
