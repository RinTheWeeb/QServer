﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qserver.GameServer.Qpang
{
    public class PublicEnemy : GameMode
    {
        public override bool IsMissionMode()
        {
            return true;
        }

        public override bool IsTeamMode()
        {
            return false;
        }

        public override void OnStart(RoomSession roomSession)
        {
            //var players = roomSession.GetPlayers();
            //if(players.Count > 0)
            //    players.

            base.OnStart(roomSession);
        }

        public override void Tick(RoomSession roomSession)
        {
            if(roomSession.PublicEnemy == null)
            {
                // Master always starts as public enemy
                var players = roomSession.GetPlayers();
                if (players.Count > 0)
                    foreach (var p in players)
                        if (p.Player.PlayerId == roomSession.Room.MasterId)
                            roomSession.PublicEnemy = p;
            }
                

            base.Tick(roomSession);
        }

        public override void OnPlayerSync(RoomSessionPlayer session)
        {
            var roomSession = session.RoomSession;
            if (roomSession != null && roomSession.PublicEnemy != null)
            {
                session.Post(new GCGameState(session.Player.PlayerId, (uint)CGGameState.State.PREY_COUNT_START, roomSession.PublicEnemy.Health));
                session.Post(new GCGameState(roomSession.PublicEnemy.Player.PlayerId, (uint)CGGameState.State.PREY_TRANFORM, roomSession.PublicEnemy.Health));
                //session.Post(new GCGameState(roomSession.PublicEnemy.Player.PlayerId, (uint)CGGameState.State.PREY_TRANFORM_READY, roomSession.PublicEnemy.Health));
                session.Post(new GCGameState(roomSession.PublicEnemy.Player.PlayerId, (uint)CGGameState.State.PREY_SELECT, roomSession.PublicEnemy.Health));
            }
        }

        public override void OnPlayerKill(RoomSessionPlayer killer, RoomSessionPlayer target, Weapon weapon, byte hitLocation)
        {
            // TODO: check if target was Prey and start new counter
            var roomSession = target.RoomSession;
            if (roomSession == null)
                return;

            if (roomSession.PublicEnemy != target)
                killer.Score++;
            else if (roomSession.PublicEnemy == target)
            {
                killer.Kills++;
                roomSession.PublicEnemy = killer;
            }

            killer.Kills--; // pre-undo base kill gain

            // TODO2: keep track of kills
            base.OnPlayerKill(killer, target, weapon, hitLocation);
        }
    }
}
