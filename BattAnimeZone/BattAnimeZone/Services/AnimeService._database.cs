namespace BattAnimeZone.Services
{
	public partial class AnimeService
	{
		private bool databaseFilled = false;
		public bool isDatabaseFilled()
		{
			return this.databaseFilled;
		}

		public void changeFilledFlag()
		{
			this.databaseFilled = true;
		}
	}
}
