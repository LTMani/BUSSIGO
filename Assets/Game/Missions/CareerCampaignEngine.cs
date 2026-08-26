using System;
using System.Collections.Generic;

namespace Bussigo.Game.Missions
{
    public class CampaignChapter
    {
        public int ChapterNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequiredCorridorId { get; set; }
        public float RewardCoins { get; set; }
        public int RewardXp { get; set; }
        public bool IsCompleted { get; set; } = false;
    }

    public class CareerCampaignEngine
    {
        public List<CampaignChapter> Chapters { get; } = new List<CampaignChapter>();
        public int CurrentActiveChapterIndex { get; private set; } = 0;

        public CareerCampaignEngine()
        {
            InitializeChapters();
        }

        private void InitializeChapters()
        {
            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 1,
                Title = "The Feeder Route: Guntur to Vijayawada",
                Description = "Complete your first passenger shuttle between Vijayawada Benz Circle and Guntur NTR Bus Stand.",
                RequiredCorridorId = "COR-VJA-GNT-02",
                RewardCoins = 25000,
                RewardXp = 500
            });

            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 2,
                Title = "NH65 Highway Maiden Run",
                Description = "Drive the flagship express route from Vijayawada PNBS to Suryapet Food Plaza.",
                RequiredCorridorId = "COR-VJA-HYD-01",
                RewardCoins = 65000,
                RewardXp = 1200
            });

            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 3,
                Title = "Telangana Capital Flagship Express",
                Description = "Complete the full Vijayawada to Hyderabad MGBS corridor with over 90% passenger comfort score.",
                RequiredCorridorId = "COR-VJA-HYD-01",
                RewardCoins = 150000,
                RewardXp = 3000
            });
        }
    }
}
