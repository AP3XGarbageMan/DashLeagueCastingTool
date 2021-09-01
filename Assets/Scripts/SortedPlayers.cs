using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckOnYou : MonoBehaviour
{

}


[Serializable]
public class PlayersInGame
{
    public string Name;
    public string TeamName;
    public string ShortName;

    public List<string> hasKilled = new List<string>();
    public List<string> killedBy = new List<string>();

    public int Team;
    public int Kills;
    public int PreviousKills;
    public int Deaths;
    public int PreviousDeaths;
    public int Score;
    public int headShots;
    public int CurrentKillStreak;
    public int HighestKillStreak;

    public bool isStreaking;
    public bool isDead;

    public PlayersInGame(string _name, int _team, int _score, int _kills, int _deaths) //, string _hk, string _kb)
    {
        Name = _name;
        TeamName = "Team";
        ShortName = "name";
        Team = _team;
        Kills = _kills;
        Deaths = _deaths;
        Score = _score;

        headShots = 0;
        PreviousDeaths = 0;
        PreviousKills = 0;
        CurrentKillStreak = 0;
        HighestKillStreak = 0;

        hasKilled.Add("name");
        killedBy.Add("name");

        isStreaking = false;
        isDead = false;
    }
}
