using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MLB_Stats
{
    class Player
    {
        private string playerID;
        private DataTable basicInfo;
        private DataTable battingStats;
        private DataTable battingTotals;
        private DataTable pitchingStats;
        private DataTable pitchingTotals;
        private DataTable fieldingStats;
        private DataTable fieldingTotals;
        private DataTable battingStatsPost;
        private DataTable battingTotalsPost;
        private DataTable pitchingStatsPost;
        private DataTable pitchingTotalsPost;
        private DataTable awards;
        private string hallOfFame;

        private StatsDatabaseAccess statsDatabaseAccess = new StatsDatabaseAccess();

        public Player(string playerID)
        {
            this.playerID = playerID;
            this.basicInfo = statsDatabaseAccess.GetBasicInfo(this.playerID);

            this.battingStats = statsDatabaseAccess.GetBattingStats(this.playerID);
            this.battingTotals = statsDatabaseAccess.GetBattingTotals(this.playerID);

            this.pitchingStats = statsDatabaseAccess.GetPitchingStats(this.playerID);
            this.pitchingTotals = statsDatabaseAccess.GetPitchingTotals(this.playerID);

            this.fieldingStats = statsDatabaseAccess.GetFieldingStats(this.playerID);
            this.fieldingTotals = statsDatabaseAccess.GetFieldingTotals(this.playerID);

            this.battingStatsPost = statsDatabaseAccess.GetBattingStatsPost(this.playerID);
            this.battingTotalsPost = statsDatabaseAccess.GetBattingTotalsPost(this.playerID);

            this.pitchingStatsPost = statsDatabaseAccess.GetPitchingStatsPost(this.playerID);
            this.pitchingTotalsPost = statsDatabaseAccess.GetPitchingTotalsPost(this.playerID);

            this.awards = statsDatabaseAccess.GetAwards(this.playerID);
            this.hallOfFame = statsDatabaseAccess.GetHallOfFame(this.playerID);
        }

        public string PlayerID
        {
            get
            {
                return playerID;
            }
        }

        public DataTable BattingStats
        {
            get
            {
                return battingStats;
            }
        }

        public DataTable BattingTotals
        {
            get
            {
                return battingTotals;
            }
        }

        public DataTable PitchingStats
        {
            get
            {
                return pitchingStats;
            }
        }

        public DataTable PitchingTotals
        {
            get
            {
                return pitchingTotals;
            }
        }

        public DataTable FieldingStats
        {
            get
            {
                return fieldingStats;
            }
        }

        public DataTable FieldingTotals
        {
            get
            {
                return fieldingTotals;
            }
        }

        public DataTable BattingStatsPost
        {
            get
            {
                return battingStatsPost;
            }
        }

        public DataTable BattingTotalsPost
        {
            get
            {
                return battingTotalsPost;
            }
        }

        public DataTable PitchingStatsPost
        {
            get
            {
                return pitchingStatsPost;
            }
        }

        public DataTable PitchingTotalsPost
        {
            get
            {
                return pitchingTotalsPost;
            }
        }

        public DataTable Awards
        {
            get
            {
                return awards;
            }
        }

        public string HallOfFame
        {
            get
            {
                return hallOfFame;
            }
        }

        public DataTable BasicInfo
        {
            get
            {
                return basicInfo;
            }
        }
    }
}
