﻿using System;
using System.Collections.Generic;
using System.Text;
using Qserver.Util;
using Qserver.GameServer.Qpang;
using Qserver.GameServer.Network;
using Qserver.GameServer.Network.Managers;
using Qserver.GameServer.Network.Packets;
using System.Threading;

namespace Qserver.GameServer.Network.Handlers
{
    public class SquareHandler
    {
        public static void HandleChatRequest(PacketReader packet, ConnServer manager)
        {
            packet.ReadBytes(34);
            ushort len = packet.ReadUInt16();
            string msg = packet.ReadWString(len & 254); // prevent over 255

            Player player = manager.player;
            if (player == null)
                return;

            SquarePlayer squarePlayer = player.SquarePlayer;
            if (squarePlayer == null)
                return;

            // TODO

        }

        public static void HandleConnectRequest(PacketReader packet, ConnServer manager)
        {
        }

        public static void HandleEmoteEevent(PacketReader packet, ConnServer manager)
        {
        }

        public static void HandleSquareLogin(PacketReader packet, ConnServer manager)
        {
            uint playerId = packet.ReadUInt32();
            uint squareId = packet.ReadUInt32();

            Player player = manager.player;

            if (player == null)
                return;

            bool isInSquare = player.SquarePlayer != null;
            if (isInSquare)
            {
                // TODO
            }
            else
            {
                // TODO
            }

        }

        public static void HandleLeftInventory(PacketReader packet, ConnServer manager)
        {

        }

        public static void HandleReloadSquareEvent(PacketReader packet, ConnServer manager)
        {

        }

        public static void HandleRequestPlayers(PacketReader packet, ConnServer manager)
        {

        }

        public static void HandleUpdatePosition(PacketReader packet, ConnServer manager)
        {

        }

        public static void HandleUpdateStateEvent(PacketReader packet, ConnServer manager)
        {

        }

       
    }
}