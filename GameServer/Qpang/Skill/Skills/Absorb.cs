﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Qserver.GameServer.Qpang.Skills
{
    public class Absorb : Skill
    {
        // Blocks 100 damage for selected team member 

        public override uint GetId()
        {
            return (uint)Items.SKILL_ABSORB;
        }
    }
}
