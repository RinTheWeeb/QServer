﻿using Qserver.GameServer.Helpers;
using System;
using System.Linq;
using System.IO;
using System.Text;
using Qserver.Util;
using System.Net;
using System.Net.Sockets;

namespace Qserver.GameServer.Network.Packets
{
   
    public struct PacketHeader
    {
        public UInt16 Length;
        public byte Sequence;
        public byte Unk; // crc?
    }

    public struct PayloadHeader
    {
        public UInt16 Length;
        public UInt16 Opcode;
    }

    public class PacketReader
    {
        private PacketHeader PacketHeader;
        private PayloadHeader PayloadHeader;

        public Opcode Opcode
        {
            get { return (Opcode)PayloadHeader.Opcode; }
        }
        public ushort Size
        {
            get { return PacketHeader.Length; }
        }
        public ushort PayloadSize
        {
            get { return PayloadHeader.Length; }
        }
        public byte Encryption
        {
            get { return PacketHeader.Sequence; }
        }

        /// Packet::Header (0)
        /// Packet::PayloadHeader (4)
        /// Packet::Payload (8)

        private BinaryReader _payload;

        public PacketReader(NetworkStream mem, string identifier, byte[] key)
        {
            _payload = new BinaryReader(mem); // temp
            PacketHeader = new PacketHeader()
            {
                Length = this.ReadUInt16(), // 0 - 2
                Sequence = this.ReadUInt8(), // 2 - 3
                Unk = this.ReadUInt8() // 3 - 4
            };
            if(Encryption > 0)
            {
                BlowFish b = BlowFish.Instance;
                if (Encryption >= 0x05)
                {
                    b = new BlowFish(key); // xx xx xx xx 29 A1 D3 56
                    b.CompatMode = true;
                }

                byte[] encryptedPayload = this.ReadBytes(PacketHeader.Length-4); // data.Skip(4).Take(PacketHeader.Length).ToArray();
                byte[] decryptedPayload = b.Decrypt_ECB(encryptedPayload);
                _payload = new BinaryReader(new MemoryStream(decryptedPayload));
            }
            else
                _payload = new BinaryReader(new MemoryStream(this.ReadBytes(PacketHeader.Length - 4)));

            // print bytes
            byte[] content = new byte[(int)_payload.BaseStream.Length];
            long index = _payload.BaseStream.Position;
            _payload.BaseStream.Read(content, 0, (int)_payload.BaseStream.Length);
            _payload.BaseStream.Position = index;

            string bytes = $"[{PacketHeader.Length.ToString("X2")}] {PacketHeader.Sequence.ToString("X2")} {PacketHeader.Unk.ToString("X2")} ";
            for(int i = 0; i < content.Length; i++)
            {
                bytes += content[i].ToString("X2");
                if (i != content.Length - 1)
                    bytes += " ";
            }
            Log.Message(LogType.DUMP, bytes);

            PayloadHeader = new PayloadHeader()
            {
                Length = this.ReadUInt16(), // 4 - 6
                Opcode = this.ReadUInt16() // 6 - 8
            };
            byte unk1 = this.ReadUInt8();
            byte unk2 = this.ReadUInt8();
            ushort unk3 = this.ReadUInt16();
        }

        public sbyte ReadInt8()
        {
            return _payload.ReadSByte();
        }

        public new short ReadInt16()
        {
            return _payload.ReadInt16();
        }

        public new int ReadInt32()
        {
            return _payload.ReadInt32();
        }

        public new long ReadInt64()
        {
            return _payload.ReadInt64();
        }

        public byte ReadUInt8()
        {
            return _payload.ReadByte();
        }

        public new ushort ReadUInt16()
        {
            return _payload.ReadUInt16();
        }

        public new uint ReadUInt32()
        {
            return _payload.ReadUInt32();
        }

        public new ulong ReadUInt64()
        {
            return _payload.ReadUInt64();
        }

        public float ReadFloat()
        {
            return _payload.ReadSingle();
        }

        public new double ReadDouble()
        {
            return _payload.ReadDouble();
        }

        public Vector ReadVector()
        {
            return new Vector(_payload.ReadUInt16(), _payload.ReadUInt16());
        }

        public string ReadString(byte terminator = 0)
        {
            StringBuilder tmpString = new StringBuilder();
            char tmpChar = _payload.ReadChar();
            char tmpEndChar = Convert.ToChar(Encoding.UTF8.GetString(new byte[] { terminator }));

            while (tmpChar != tmpEndChar)
            {
                tmpString.Append(tmpChar);
                tmpChar = _payload.ReadChar();
            }

            return tmpString.ToString();
        }

        public new string ReadString()
        {
            return ReadString(0);
        }

        public new string ReadWString(int len)
        {
            byte[] str = ReadBytes(len*2+2);
            string result = "";
            for(int i = 0; i < len * 2 + 2; i+=2)
                if(str[i] != 0x00)
                    result += (char)str[i];
            return result;
        }

        public new byte[] ReadBytes(int count)
        {
            return _payload.ReadBytes(count);
        }

        public string ReadStringFromBytes(int count)
        {
            byte[] stringArray = _payload.ReadBytes(count);
            Array.Reverse(stringArray);

            return Encoding.ASCII.GetString(stringArray);
        }

        public string ReadIPAddress()
        {
            byte[] ip = new byte[4];

            for (int i = 0; i < 4; ++i)
            {
                ip[i] = ReadUInt8();
            }

            return ip[0] + "." + ip[1] + "." + ip[2] + "." + ip[3];
        }

        public void SkipBytes(int count)
        {
            _payload.BaseStream.Position += count;
        }
    }
}
