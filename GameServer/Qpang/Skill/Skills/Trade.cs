﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Qserver.GameServer.Qpang
{
    public class Zilla : Skill
    {
        public override uint GetId()
        {
            return (uint)Items.SKILL_ZILLA;
        }
    }
}