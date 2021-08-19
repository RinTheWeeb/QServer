﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qserver.GameServer.Qpang
{
    public class RoomSkillManager
    {
        private RoomSession _room;
        private Dictionary<uint, Skill> _skills;

        public RoomSkillManager()
        {
            
        }

        public void Initialize(RoomSession room)
        {
            this._room = room;
            this._skills = Game.Instance.SkillManager.GetSkillsForMode((byte)room.Room.Mode);
        }


        public Skill GenerateRandomSkill()
        {
            // idk
            Random rnd = new Random();
            return this._skills.ToList()[rnd.Next(0, this._skills.Count)].Value;
        }

        public bool IsSkillValid(uint itemId)
        {
            return this._skills.ContainsKey(itemId);
        }
    }
}
