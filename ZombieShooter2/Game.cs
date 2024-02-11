using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static ZombieShooter2.Program;

namespace ZombieShooter2
{
	static class Game
	{
		internal static List<Enemy> enemies;
		internal static List<Bullet> bullets;
		internal static Player player;
		internal static GameState gameState;
		internal static int ammo, coins;

		static List<Enemy> deadEnemies;
		static List<Bullet> deadBullets;
		static Timer gameTimer, shootTimer, enemyAttackTimer;
		static Random random;
		static int waveCounter, playerForce, enemiesForce;

		internal static void Initialize()
		{
			gameState = GameState.NotRunning;

			gameTimer = new Timer() { Interval = 16 };
			shootTimer = new Timer() { Interval = 2000 };
			enemyAttackTimer = new Timer() { Interval = 800 };
			gameTimer.Tick += GameTimer_Tick;
			enemyAttackTimer.Tick += EnemyAttackTimer_Tick;
			shootTimer.Tick += (s, e) => player.CanShoot = true;

			enemies = new List<Enemy>();
			bullets = new List<Bullet>();
			deadEnemies = new List<Enemy>();
			deadBullets = new List<Bullet>();

			player = new Player(new PictureBox() 
			{ Location = new Point(100, 100),Image = Media.playerImage,Size = new Size(30, 50), Tag = "gamePage" });

			random = new Random();

			waveCounter = 0;
			playerForce = 0;
			enemiesForce = 0;
			ammo = 20;
			coins = 0;
		}

		internal static void RunNew()
		{
			Initialize();

			gameState = GameState.Running;

			gameTimer.Start();
			shootTimer.Start();
			enemyAttackTimer.Start();

			mainForm.gamePage.AddControl(player.Hitbox);
			mainForm.healtBar.Value = player.healt;
		}

		internal static void Pause()
		{
			gameState = GameState.Stopped;

			gameTimer.Stop();
			shootTimer.Stop();
			enemyAttackTimer.Stop();
		}

		internal static void Run()
		{
			gameState = GameState.Running;

			gameTimer.Start();
			shootTimer.Start();
			enemyAttackTimer.Start();
		}

		internal static void Quit()
		{
			gameState = GameState.NotRunning;

			gameTimer.Stop();
			shootTimer.Stop();
			enemyAttackTimer.Stop();

			mainForm.gamePage.RemoveControl(player.Hitbox);

			foreach (Enemy enemy in enemies)
			{
				mainForm.gamePage.RemoveControl(enemy.Hitbox);
				enemy.Hitbox.Dispose();
			}

			foreach (Bullet bullet in bullets)
			{
				mainForm.gamePage.RemoveControl(bullet.Hitbox);
				bullet.Hitbox.Dispose();
			}
		}

		internal static void GameOver()
		{
			Quit();
			mainForm.gameOverWavesLabel.Text = $"Waves survived: {waveCounter}";
			mainForm.gameOverPage.LoadToFormInstedCurrent(mainForm, mainForm.gamePage);
		}

		static void GameTimer_Tick(object sender, EventArgs e)
		{
			mainForm.Text = mainForm.gamePage.Text + $" | player: {player.Hitbox.Location} shoot: {player.CanShoot} |" +
				$" enemies: count: {enemies.Count} | bullets: count {bullets.Count}";

			if (player.GoLeft) player.Move(-15, 0);
			if (player.GoRight) player.Move(15, 0);

			if (player.Jumping)
			{
				player.Move(0, -playerForce);
				playerForce -= 1;
			}

			if (player.GetCollider("platform") != null)
			{
				player.Jumping = false;
				playerForce = 0;
				player.FixCollision("platform");
			}
			else
			{
				playerForce -= 1;
				player.Move(0, -playerForce);
			}

			player.FixFormCollision();

			if (enemies.Count == 0) SpawnNewWave();

			foreach (Bullet bullet in bullets)
			{
				bullet.Move(bullet.Speed.X, bullet.Speed.Y);
				if (bullet.GetCollider("enemy") != null &&
					deadBullets.Find(bullet => bullet.Hitbox.Equals((PictureBox)bullet.GetCollider("enemy"))) == null)
				{
					deadBullets.Add(bullet);
					deadEnemies.Add(enemies.Find(enemy => enemy.Hitbox.Equals((PictureBox)bullet.GetCollider("enemy"))));
				}

				if (bullet.GetCollider("platform") != null || bullet.IsCollidingWithMainForm())
				{
					deadBullets.Add(bullet);
				}
			}

			foreach (Enemy enemy in enemies)
			{
				enemy.MoveXToPlayer(player, 5);

				if (enemy.GetCollider("platform") != null)
				{
					enemiesForce = 0;
					enemy.FixCollision("platform");
				}
				else
				{
					enemiesForce -= 2;
					enemy.Move(0, -enemiesForce);
				}
			}

			foreach (Bullet deadBullet in deadBullets)
			{
				bullets.Remove(deadBullet);
				mainForm.gamePage.RemoveControl(deadBullet.Hitbox);
				deadBullet.Hitbox.Dispose();
			}

			foreach (Enemy deadEnemy in deadEnemies)
			{
				enemies.Remove(deadEnemy);
				mainForm.gamePage.RemoveControl(deadEnemy.Hitbox);
				deadEnemy.Hitbox.Dispose();
				coins += 2;
				mainForm.coinsLabel.Text = coins.ToString();
			}

			deadBullets.Clear();
			deadEnemies.Clear();
		}

		static void EnemyAttackTimer_Tick(object sender, EventArgs e)
		{
			if (gameState == GameState.Running)
			{
				foreach (Enemy enemy in enemies)
				{
					enemy.AttackPlayer(player);
					break;
				}
			}
		}

		internal static void Input_KeyDown(object sender, KeyEventArgs e)
		{
			if (gameState == GameState.Running)
			{
				if (e.KeyCode is Keys.Left or Keys.A) player.GoLeft = true;
				if (e.KeyCode is Keys.Right or Keys.D) player.GoRight = true;
				if ((e.KeyCode is Keys.Space or Keys.Up or Keys.W) && !player.Jumping && !player.CollidingOnTop)
				{
					player.Jumping = true;
					playerForce = 15;
				}
			}
		}

		internal static void Input_KeyUp(object sender, KeyEventArgs e)
		{
			if (gameState == GameState.Running)
			{
				if (e.KeyCode is Keys.Left or Keys.A) player.GoLeft = false;
				if (e.KeyCode is Keys.Right or Keys.D) player.GoRight = false;
			}
		}

		internal static void BuyAmmo(object sender, EventArgs e)
		{
			ammo += 20;
			coins -= 10;
			mainForm.ammoLabel.Text = ammo.ToString();
			mainForm.coinsLabel.Text = coins.ToString();

			if (coins >= 10)
			{
				mainForm.shopAmmoBuyButton.Enabled = true;
				mainForm.shopAmmoBuyButton.BackColor = Color.Gold;
			}
			else
			{
				mainForm.shopAmmoBuyButton.Enabled = false;
				mainForm.shopAmmoBuyButton.BackColor = Color.DarkGray;
			}
		}

		internal static void BuyHeal(object sender, EventArgs e)
		{
			coins -= 60;
			if (player.healt >= 50) player.healt = 100;
			else if (player.healt < 50) player.healt += 50;
			mainForm.healtBar.Value = player.healt;
			mainForm.coinsLabel.Text = coins.ToString();

			if (coins >= 60)
			{
				mainForm.shopHealBuyButton.Enabled = true;
				mainForm.shopHealBuyButton.BackColor = Color.Gold;
			}
			else
			{
				mainForm.shopHealBuyButton.Enabled = false;
				mainForm.shopHealBuyButton.BackColor = Color.DarkGray;
			}
		}

		static void SpawnNewWave()
		{
			waveCounter++;
			mainForm.wavesLabel.Text = "wave: " + waveCounter.ToString();

			for (int i = 0; i <= waveCounter * 2; i++)
			{
				PictureBox[] platforms = new PictureBox[]
				{
					mainForm.gamePlatform_1, mainForm.gamePlatform_2, mainForm.gamePlatform_3,
					mainForm.gamePlatform_4, mainForm.gamePlatform_Floor
				};

				PictureBox enemyPlatform = platforms[random.Next(5)];
				PictureBox enemyHitbox = new PictureBox() { Name = "enemy", Size = player.Hitbox.Size, Image = Media.enemyImage };

				enemyHitbox.Location = new Point(
					random.Next(enemyPlatform.Left, enemyPlatform.Right), enemyPlatform.Top - enemyHitbox.Height);

				Enemy enemy = new Enemy(enemyHitbox);
				enemies.Add(enemy);
				mainForm.gamePage.AddControl(enemy.Hitbox);
			}
		}
	}

	enum GameState
	{
		Running, Stopped, NotRunning
	}
}