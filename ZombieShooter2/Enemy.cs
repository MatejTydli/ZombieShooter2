using System.Windows.Forms;
using System.Drawing;

namespace ZombieShooter2
{
	class Enemy : GameObject
	{
		public bool IsInTopCollision;

		public Enemy(PictureBox hitbox) : base(hitbox)
		{
			this.IsInTopCollision = false;
		}

		public void MoveXToPlayer(Player player, int speed)
		{
			int playerXCenter = player.Hitbox.Right - player.Hitbox.Width / 2;

			if (this.Hitbox.Bounds.IntersectsWith(
				new Rectangle(playerXCenter - 5, player.Hitbox.Location.Y, 10, player.Hitbox.Height))) { }
			else if (playerXCenter < this.Hitbox.Left) this.Move(-speed, 0);
			else if (playerXCenter > this.Hitbox.Left) this.Move(speed, 0);
		}

		public bool AttackPlayer(Player player)
		{
			if (this.Hitbox.Bounds.IntersectsWith(player.Hitbox.Bounds))
			{
				if (player.healt <= 5)
				{
					player.healt = 0;
					Game.GameOver();
					return true;
				}

				player.healt -= 5;
				Program.mainForm.healtBar.Value = player.healt;
			}

			return false;
		}
	}
}