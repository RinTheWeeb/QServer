﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Qserver.GameServer.Qpang
{
    public class VIP : GameMode
    {
        public override bool IsMissionMode()
        {
            return true;
        }

        public override bool IsTeamMode()
        {
            return true;
        }

        // TODO
    }
}
