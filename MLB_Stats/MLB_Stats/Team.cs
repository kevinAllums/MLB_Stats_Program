using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MLB_Stats
{
    class Team
    {
        private string yearID;
        private string name;
        private string teamID;

        private DataTable teamBasicInfo;
        private DataTable teamBattingStats;
        private DataTable teamPitchingStats;
        private DataTable teamBattingStatsPost;
        private DataTable teamPitchingStatsPost;

        private StatsDatabaseAccess statsDatabaseAccess = new StatsDatabaseAccess();

        public Team(string yearID, string name)
        {
            this.yearID = yearID;
            this.name = name;
            this.teamID = statsDatabaseAccess.getTeamID(this.yearID, this.name);
            this.teamBasicInfo = statsDatabaseAccess.getTeamBasicInfo(this.yearID, this.name);
            this.teamBattingStats = statsDatabaseAccess.getTeamBattingStats(this.yearID, this.teamID);
            this.teamPitchingStats = statsDatabaseAccess.getTeamPitchingStats(this.yearID, this.teamID);
            this.teamBattingStatsPost = statsDatabaseAccess.getTeamBattingStatsPost(this.yearID, this.teamID);
            this.teamPitchingStatsPost = statsDatabaseAccess.getTeamPitchingStatsPost(this.yearID, this.teamID);
        }

        public string YearID
        {
            get
            {
                return yearID;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public DataTable TeamBasicInfo
        {
            get
            {
                return teamBasicInfo;
            }
        }

        public DataTable TeamBattingStats
        {
            get
            {
                return teamBattingStats;
            }
        }

        public DataTable TeamPitchingStats
        {
            get
            {
                return teamPitchingStats;
            }
        }

        public DataTable TeamBattingStatsPost
        {
            get
            {
                return teamBattingStatsPost;
            }
        }

        public DataTable TeamPitchingStatsPost
        {
            get
            {
                return teamPitchingStatsPost;
            }
        }
    }
}
