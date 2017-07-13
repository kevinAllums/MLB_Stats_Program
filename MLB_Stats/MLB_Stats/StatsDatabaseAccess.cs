using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace MLB_Stats
{
    class StatsDatabaseAccess
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private SQLiteCommand command;
        private DataTable DT;

        public StatsDatabaseAccess()
        {
            const string filename = @"C:\Users\kevin\Desktop\lahman2016\lahman2016withAdditions.sqlite";
            conn = new SQLiteConnection(string.Format("DataSource={0};Version=3;", filename));
        }

        public DataTable GetPlayers(string nameFirst, string nameLast)
        {
            if (nameLast == "" || nameFirst == "")
            {
                string partialName;
                partialName = nameLast.Length > nameFirst.Length ? nameLast : nameFirst;
                partialName += "%";
                string nameLastMatches = string.Format("SELECT " +
                    "master.playerID, " +
                    "master.nameFirst, " +
                    "master.nameLast, " +
                    "master.position, " +
                    "master.debut, " +
                    "master.finalGame " +
                    "FROM master " +
                    "WHERE nameLast LIKE '{0}' " +
                    "AND position!='M';", partialName);
                string nameFirstMatches = string.Format("SELECT " +
                    "master.playerID, " +
                    "master.nameFirst, " +
                    "master.nameLast, " +
                    "master.position, " +
                    "master.debut, " +
                    "master.finalGame " +
                    "FROM master " +
                    "WHERE nameFirst LIKE '{0}' " +
                    "AND position!='M';", partialName);

                DataTable results = new DataTable();

                results = UseSQLiteAdapter(nameLastMatches).Copy();
                results.Merge(UseSQLiteAdapter(nameFirstMatches));

                return results;
            }

            nameFirst += "%";
            nameLast += "%";

            string getPlayersWithBoth = string.Format("SELECT " +
                "master.playerID, " +
                "master.nameFirst, " +
                "master.nameLast, " +
                "master.position, " +
                "master.debut, " +
                "master.finalGame " +
                "FROM master " +
                "WHERE (nameFirst LIKE '{0}' " +
                "AND nameLast LIKE '{1}' " +
                "AND position!='M')", nameFirst, nameLast);

            return UseSQLiteAdapter(getPlayersWithBoth);
        }

        public DataTable GetBattingStats(string playerID)
        {
            string battingQuery = string.Format("SELECT " +
                "b.yearID AS Year, " +
                "t.name AS 'Team', " +
                "b.lgID AS League, " +
                "b.G, " +
                "b.AB, " +
                "b.R, " +
                "b.H, " +
                "b.'2B', " +
                "b.'3B', " +
                "b.HR, " +
                "b.RBI, " +
                "b.SB, " +
                "b.CS, " +
                "b.BB, " +
                "b.SO, " +
                "b.IBB, " +
                "b.HBP, " +
                "b.SH, " +
                "b.SF, " +
                "b.GIDP " +
                "FROM Batting b " +
                "LEFT JOIN Teams t ON t.teamID = b.teamID AND t.yearID = b.yearID " +
                "WHERE b.playerID IN (SELECT playerid FROM master WHERE (master.playerID = '{0}'));", playerID);

            return UseSQLiteAdapter(battingQuery);
        }

        public DataTable GetBattingTotals(string playerID)
        {
            string battingTotalsQuery = string.Format("SELECT " +
                "TOTAL(b.G) AS G, " +
                "TOTAL(b.AB) AS AB, " +
                "TOTAL(b.R) AS R, " +
                "TOTAL(b.H) AS H, " +
                "TOTAL(b.'2B') AS '2B', " +
                "TOTAL(b.'3B') AS '3B', " +
                "TOTAL(b.HR) AS HR, " +
                "TOTAL(b.RBI) AS RBI, " +
                "TOTAL(b.SB) AS SB, " +
                "TOTAL(b.CS) AS CS, " +
                "TOTAL(b.BB) AS BB, " +
                "TOTAL(b.SO) AS SO, " +
                "TOTAL(b.IBB) AS IBB, " +
                "TOTAL(b.HBP) AS HBP, " +
                "TOTAL(b.SH) AS SH, " +
                "TOTAL(b.SF) AS SF, " +
                "TOTAL(b.GIDP) AS GIDP " +
                "FROM Batting b " +
                "WHERE b.playerID='{0}';", playerID);

            return UseSQLiteAdapter(battingTotalsQuery);
        }

        public DataTable GetPitchingStats(string playerID)
        {
            string pitchingQuery = string.Format("SELECT " +
                "p.yearID AS Year, " +
                "t.name AS 'Team', " +
                "p.lgID AS League, " +
                "p.W, " +
                "p.L, " +
                "p.G, " +
                "p.GS, " +
                "p.CG, " +
                "p.SHO, " +
                "p.SV, " +
                "p.IPouts, " +
                "p.H, " +
                "p.ER, " +
                "p.HR, " +
                "p.BB, " +
                "p.SO, " +
                "p.BAOpp, " +
                "p.ERA, " +
                "p.IBB, " +
                "p.WP, " +
                "p.HBP, " +
                "p.BK, " +
                "p.BFP, " +
                "p.GF, " +
                "p.R, " +
                "p.SH, " +
                "p.SF, " +
                "p.GIDP " +
                "FROM pitching p " +
                "LEFT JOIN Teams t ON t.teamID = p.teamID AND t.yearID = p.yearID " +
                "WHERE p.playerID IN (SELECT playerid FROM master WHERE (master.playerID = '{0}'));", playerID);

            return UseSQLiteAdapter(pitchingQuery);
        }

        public DataTable GetPitchingTotals(string playerID)
        {
            string pitchingTotalsQuery = string.Format("SELECT " +
                "TOTAL(p.W) AS W, " +
                "TOTAL(p.L) AS L, " +
                "TOTAL(p.G) AS G, " +
                "TOTAL(p.GS) AS GS, " +
                "TOTAL(p.CG) AS CG, " +
                "TOTAL(p.SHO) AS SHO, " +
                "TOTAL(p.SV) AS SV, " +
                "TOTAL(p.IPouts) AS IPouts, " +
                "TOTAL(p.H) AS H, " +
                "TOTAL(p.ER) AS ER, " +
                "TOTAL(p.HR) AS HR, " +
                "TOTAL(p.BB) AS BB, " +
                "TOTAL(p.SO) AS SO, " +
                "TOTAL(p.BAOpp) AS BAOpp, " +
                "TOTAL(p.ERA) AS ERA, " +
                "TOTAL(p.IBB) AS IBB, " +
                "TOTAL(p.WP) AS WP, " +
                "TOTAL(p.HBP) AS HBP, " +
                "TOTAL(p.BK) AS BK, " +
                "TOTAL(p.BFP) AS BFP, " +
                "TOTAL(p.GF) AS GF, " +
                "TOTAL(p.R) AS R, " +
                "TOTAL(p.SH) AS SH, " +
                "TOTAL(p.SF) AS SF, " +
                "TOTAL(p.GIDP) AS GIDP " +
                "FROM pitching p " +
                "WHERE p.playerID='{0}';", playerID);

            return UseSQLiteAdapter(pitchingTotalsQuery);
        }

        public DataTable GetFieldingStats(string playerID)
        {
            string fieldingQuery = string.Format("SELECT " +
                "f.yearID AS Year, " +
                "t.name AS 'Team', " +
                "f.lgID AS League, " +
                "f.POS, " +
                "f.G, " +
                "f.GS, " +
                "f.InnOuts, " +
                "f.PO, " +
                "f.A, " +
                "f.E, " +
                "f.DP, " +
                "f.PB, " +
                "f.WP, " +
                "f.SB, " +
                "f.CS, " +
                "f.ZR " +
                "FROM fielding f " +
                "LEFT JOIN Teams t ON t.teamID = f.teamID AND t.yearID = f.yearID " +
                "WHERE f.playerID IN (SELECT playerid FROM master WHERE (master.playerID = '{0}'));", playerID);

            return UseSQLiteAdapter(fieldingQuery);
        }

        public DataTable GetFieldingTotals(string playerID)
        {
            string fieldingTotalsQuery = string.Format("SELECT " +
                "TOTAL(f.G) AS G, " +
                "TOTAL(f.GS) AS GS, " +
                "TOTAL(f.InnOuts) AS InnOuts, " +
                "TOTAL(f.PO) AS PO, " +
                "TOTAL(f.A) AS A, " +
                "TOTAL(f.E) AS E, " +
                "TOTAL(f.DP) AS DP, " +
                "TOTAL(f.PB) AS PB, " +
                "TOTAL(f.WP) AS WP, " +
                "TOTAL(f.SB) AS SB, " +
                "TOTAL(f.CS) AS CS, " +
                "TOTAL(f.ZR) AS ZR " +
                "FROM fielding f " +
                "WHERE f.playerID='{0}';", playerID);

            return UseSQLiteAdapter(fieldingTotalsQuery);
        }

        public DataTable GetBattingStatsPost(string playerID)
        {
            string battingPostQuery = string.Format("SELECT " +
                "b.yearID AS Year, " +
                "b.round AS Round, " +
                "t.name AS 'Team', " +
                "b.lgID AS League, " +
                "b.G, " +
                "b.AB, " +
                "b.R, " +
                "b.H, " +
                "b.'2B', " +
                "b.'3B', " +
                "b.HR, " +
                "b.RBI, " +
                "b.SB, " +
                "b.CS, " +
                "b.BB, " +
                "b.SO, " +
                "b.IBB, " +
                "b.HBP, " +
                "b.SH, " +
                "b.SF, " +
                "b.GIDP " +
                "FROM battingpost b " +
                "LEFT JOIN Teams t ON t.teamID = b.teamID AND t.yearID = b.yearID " +
                "WHERE b.playerID IN (SELECT playerid FROM master WHERE (master.playerID = '{0}'));", playerID);

            return UseSQLiteAdapter(battingPostQuery);
        }

        public DataTable GetBattingTotalsPost(string playerID)
        {
            string battingTotalsPostQuery = string.Format("SELECT " +
                "TOTAL(b.G) AS G, " +
                "TOTAL(b.AB) AS AB, " +
                "TOTAL(b.R) AS R, " +
                "TOTAL(b.H) AS H, " +
                "TOTAL(b.'2B') AS '2B', " +
                "TOTAL(b.'3B') AS '3B', " +
                "TOTAL(b.HR) AS HR, " +
                "TOTAL(b.RBI) AS RBI, " +
                "TOTAL(b.SB) AS SB, " +
                "TOTAL(b.CS) AS CS, " +
                "TOTAL(b.BB) AS BB, " +
                "TOTAL(b.SO) AS SO, " +
                "TOTAL(b.IBB) AS IBB, " +
                "TOTAL(b.HBP) AS HBP, " +
                "TOTAL(b.SH) AS SH, " +
                "TOTAL(b.SF) AS SF, " +
                "TOTAL(b.GIDP) AS GIDP " +
                "FROM battingpost b " +
                "WHERE b.playerID='{0}';", playerID);

            return UseSQLiteAdapter(battingTotalsPostQuery);
        }

        public DataTable GetPitchingStatsPost(string playerID)
        {
            string pitchingPostQuery = string.Format("SELECT " +
                "p.yearID AS Year, " +
                "p.round AS Round, " +
                "t.name AS 'Team', " +
                "p.lgID AS League, " +
                "p.W, " +
                "p.L, " +
                "p.G, " +
                "p.GS, " +
                "p.CG, " +
                "p.SHO, " +
                "p.SV, " +
                "p.IPouts, " +
                "p.H, " +
                "p.ER, " +
                "p.HR, " +
                "p.BB, " +
                "p.SO, " +
                "p.BAOpp, " +
                "p.ERA, " +
                "p.IBB, " +
                "p.WP, " +
                "p.HBP, " +
                "p.BK, " +
                "p.BFP, " +
                "p.GF, " +
                "p.R, " +
                "p.SH, " +
                "p.SF, " +
                "p.GIDP " +
                "FROM pitchingpost p " +
                "LEFT JOIN Teams t ON t.teamID = p.teamID AND t.yearID = p.yearID " +
                "WHERE p.playerID IN (SELECT playerid FROM master WHERE (master.playerID = '{0}'));", playerID);

            return UseSQLiteAdapter(pitchingPostQuery);
        }

        public DataTable GetPitchingTotalsPost(string playerID)
        {
            string pitchingTotalsPostQuery = string.Format("SELECT " +
                "TOTAL(p.W) AS W, " +
                "TOTAL(p.L) AS L, " +
                "TOTAL(p.G) AS G, " +
                "TOTAL(p.GS) AS GS, " +
                "TOTAL(p.CG) AS CG, " +
                "TOTAL(p.SHO) AS SHO, " +
                "TOTAL(p.SV) AS SV, " +
                "TOTAL(p.IPouts) AS IPouts, " +
                "TOTAL(p.H) AS H, " +
                "TOTAL(p.ER) AS ER, " +
                "TOTAL(p.HR) AS HR, " +
                "TOTAL(p.BB) AS BB, " +
                "TOTAL(p.SO) AS SO, " +
                "TOTAL(p.BAOpp) AS BAOpp, " +
                "TOTAL(p.ERA) AS ERA, " +
                "TOTAL(p.IBB) AS IBB, " +
                "TOTAL(p.WP) AS WP, " +
                "TOTAL(p.HBP) AS HBP, " +
                "TOTAL(p.BK) AS BK, " +
                "TOTAL(p.BFP) AS BFP, " +
                "TOTAL(p.GF) AS GF, " +
                "TOTAL(p.R) AS R, " +
                "TOTAL(p.SH) AS SH, " +
                "TOTAL(p.SF) AS SF, " +
                "TOTAL(p.GIDP) AS GIDP " +
                "FROM pitchingpost p " +
                "WHERE p.playerID='{0}';", playerID);

            return UseSQLiteAdapter(pitchingTotalsPostQuery);
        }

        public DataTable GetAwards(string playerID)
        {
            string awardsQuery = string.Format("SELECT " +
                "a.yearID AS Year, " +
                "a.lgID AS League, " +
                "a.awardID AS Award," +
                "a.notes AS Position " +
                "FROM awardsplayers a " +
                "WHERE a.playerID = '{0}';", playerID);

            return UseSQLiteAdapter(awardsQuery);
        }

        public string GetHallOfFame(string playerID)
        {
            string hallOfFameQuery = string.Format("SELECT h.yearid AS Year " +
                "FROM halloffame h " +
                "WHERE h.playerID = '{0}' AND h.inducted = 'Y';", playerID);

            return UseSQLiteExecuteScalar(hallOfFameQuery);
        }

        public DataTable GetBasicInfo(string playerID)
        {
            string basicInfoQuery = string.Format("SELECT m.nameFirst, " +
                "m.nameLast, " +
                "m.nameGiven, " +
                "m.birthYear, " +
                "m.birthMonth, " +
                "m.birthDay, " +
                "m.birthCountry, " +
                "m.birthState, " +
                "m.birthCity, " +
                "m.deathYear, " +
                "m.deathMonth, " +
                "m.deathDay, " +
                "m.weight, " +
                "m.height, " +
                "m.position, " +
                "m.bats, " +
                "m.throws, " +
                "m.debut, " +
                "m.finalGame, " +
                "m.number, " +
                "m.photo " +
                "FROM master m " +
                "WHERE m.playerID = '{0}';", playerID);

            return UseSQLiteAdapter(basicInfoQuery);
        }
        // TEAM STUFF

        // need to rework this and team search
        public DataTable GetYearsTeams()
        {
            string yearsAndTeams = "SELECT yearID, name FROM teams ORDER BY yearID, name;";

            return UseSQLiteAdapter(yearsAndTeams);
        }

        public DataTable GetTeamBasicInfo(string yearID, string name)
        {
            string teamBasicInfoQuery = string.Format("SELECT " +
                "t.yearID, " +
                "t.lgID," +
                "t.teamID," +
                "t.franchID, " +
                "t.divID, " +
                "t.Rank, " +
                "t.W, " +
                "t.L, " +
                "t.DivWin, " +
                "t.WCWin, " +
                "t.LgWin, " +
                "t.WSWin, " +
                "t.name, " +
                "t.park " +
                "FROM teams t " +
                "WHERE yearID={0} AND name=\"{1}\";", yearID, name);

            return UseSQLiteAdapter(teamBasicInfoQuery);
        }

        public string GetTeamID(string yearID, string name)
        {
            string teamIDQuery = string.Format("SELECT teamID " +
                "FROM teams " +
                "WHERE yearID={0} AND name=\"{1}\";", yearID, name);

            return UseSQLiteExecuteScalar(teamIDQuery);
        }

        public DataTable GetTeamBattingStats(string yearID, string teamID)
        {
            string teamBattingQuery = string.Format("SELECT " +
                "m.nameFirst || ' ' || m.NameLast AS Player, " +
                "b.G, " +
                "b.AB, " +
                "b.R, " +
                "b.H, " +
                "b.'2B', " +
                "b.'3B', " +
                "b.HR, " +
                "b.RBI, " +
                "b.SB, " +
                "b.CS, " +
                "b.BB, " +
                "b.SO, " +
                "b.IBB, " +
                "b.HBP, " +
                "b.SH, " +
                "b.SF, " +
                "b.GIDP " +
                "FROM batting b " +
                "INNER JOIN master m " +
                "WHERE b.yearID = {0} " +
                "AND b.teamID = '{1}' " +
                "AND b.playerID = m.playerID " +
                "ORDER BY nameLast;", yearID, teamID);

            return UseSQLiteAdapter(teamBattingQuery);
        }

        public DataTable GetTeamPitchingStats(string yearID, string teamID)
        {
            string teamPitchingQuery = string.Format("SELECT " +
                "m.nameFirst || ' ' || m.NameLast AS Player, " +
                "p.W, " +
                "p.L, " +
                "p.G, " +
                "p.GS, " +
                "p.CG, " +
                "p.SHO, " +
                "p.SV, " +
                "p.IPouts, " +
                "p.H, " +
                "p.ER, " +
                "p.HR, " +
                "p.BB, " +
                "p.SO, " +
                "p.BAOpp, " +
                "p.ERA, " +
                "p.IBB, " +
                "p.WP, " +
                "p.HBP, " +
                "p.BK, " +
                "p.BFP, " +
                "p.GF, " +
                "p.R, " +
                "p.SH, " +
                "p.SF, " +
                "p.GIDP " +
                "FROM pitching p " +
                "INNER JOIN master m " +
                "WHERE p.yearID = {0} " +
                "AND p.teamID = '{1}' " +
                "AND p.playerID = m.playerID " +
                "ORDER BY nameLast;", yearID, teamID);

            return UseSQLiteAdapter(teamPitchingQuery);
        }

        public DataTable GetTeamBattingStatsPost(string yearID, string teamID)
        {
            string teamBattingPostQuery = string.Format("SELECT " +
                "b.round, " +
                "m.nameFirst || ' ' || m.NameLast AS Player, " +
                "b.G, " +
                "b.AB, " +
                "b.R, " +
                "b.H, " +
                "b.'2B', " +
                "b.'3B', " +
                "b.HR, " +
                "b.RBI, " +
                "b.SB, " +
                "b.CS, " +
                "b.BB, " +
                "b.SO, " +
                "b.IBB, " +
                "b.HBP, " +
                "b.SH, " +
                "b.SF, " +
                "b.GIDP " +
                "FROM battingpost b " +
                "INNER JOIN master m " +
                "WHERE b.yearID = {0} " +
                "AND b.teamID = '{1}' " +
                "AND b.playerID = m.playerID " +
                "ORDER BY b.round;", yearID, teamID);

            return UseSQLiteAdapter(teamBattingPostQuery);
        }

        public DataTable GetTeamPitchingStatsPost(string yearID, string teamID)
        {
            string teamPitchingPostQuery = string.Format("SELECT " +
                "p.round, " +
                "m.nameFirst || ' ' || m.NameLast AS Player, " +
                "p.W, " +
                "p.L, " +
                "p.G, " +
                "p.GS, " +
                "p.CG, " +
                "p.SHO, " +
                "p.SV, " +
                "p.IPouts, " +
                "p.H, " +
                "p.ER, " +
                "p.HR, " +
                "p.BB, " +
                "p.SO, " +
                "p.BAOpp, " +
                "p.ERA, " +
                "p.IBB, " +
                "p.WP, " +
                "p.HBP, " +
                "p.BK, " +
                "p.BFP, " +
                "p.GF, " +
                "p.R, " +
                "p.SH, " +
                "p.SF, " +
                "p.GIDP " +
                "FROM pitchingpost p " +
                "INNER JOIN master m " +
                "WHERE p.yearID = {0} " +
                "AND p.teamID = '{1}' " +
                "AND p.playerID = m.playerID " +
                "ORDER BY p.round;", yearID, teamID);

            return UseSQLiteAdapter(teamPitchingPostQuery);
        }

        // Database access
        public string UseSQLiteExecuteScalar(string query)
        {
            string result = "";
            try
            {
                conn.Open();
                DT = new DataTable();
                command = new SQLiteCommand(query, conn);
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    result = command.ExecuteScalar().ToString();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }

        public DataTable UseSQLiteAdapter(string query)
        {
            try
            {
                conn.Open();
                DT = new DataTable();
                adapter = new SQLiteDataAdapter(query, conn);
                adapter.Fill(DT);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return DT;
        }
    }
}
