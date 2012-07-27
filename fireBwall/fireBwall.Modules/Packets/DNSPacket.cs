using System;
using System.Collections.Generic;
using System.Text;

namespace fireBwall.Packets
{
    /// <summary>
    /// DNS packet obj
    /// </summary>
    public unsafe class DNSPacket : UDPPacket
    {
        uint start = 0;
        uint length = 0;

        public DNSPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isDNS())
                throw new Exception("Not a DNS packet!");
            start = base.LayerStart() + base.LayerLength();
            length = Length() - start;
        }

        public DNSPacket(UDPPacket eth)
            : base(eth.data)
        {
            if (!isDNS())
                throw new Exception("Not a DNS packet!");
            start = base.LayerStart() + base.LayerLength();
            length = Length() - start;
        }

        public ushort TransactionID
        {
            get
            {
                return (ushort)((data->m_IBuffer[start] << 8) + data->m_IBuffer[start + 1]);
            }
            set
            {
                data->m_IBuffer[start] = (byte)(value >> 8);
                data->m_IBuffer[start + 1] = (byte)(value & 0xff);
            }
        }

        public ushort DNSFlags
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 2] << 8) + data->m_IBuffer[start + 3]);
            }
            set
            {
                data->m_IBuffer[start + 2] = (byte)(value >> 8);
                data->m_IBuffer[start + 3] = (byte)(value & 0xff);
            }
        }

        public bool Response
        {
            get
            {
                return (data->m_IBuffer[start + 2] & 0x80) == 0x80;
            }
            set
            {
                if (value)
                {
                    data->m_IBuffer[start + 2] = (byte)(data->m_IBuffer[start + 2] | 0x80);
                }
                else
                {
                    if ((data->m_IBuffer[start + 2] & 0x80) == 0x80)
                        data->m_IBuffer[start + 2] -= 0x80;
                }
            }
        }

        public ushort QuestionCount
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 4] << 8) + data->m_IBuffer[start + 5]);
            }
            set
            {
                data->m_IBuffer[start + 4] = (byte)(value >> 8);
                data->m_IBuffer[start + 5] = (byte)(value & 0xff);
            }
        }

        public ushort AnswerRRs
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 6] << 8) + data->m_IBuffer[start + 7]);
            }
            set
            {
                data->m_IBuffer[start + 6] = (byte)(value >> 8);
                data->m_IBuffer[start + 7] = (byte)(value & 0xff);
            }
        }

        public ushort AuthorityRRs
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 8] << 8) + data->m_IBuffer[start + 9]);
            }
            set
            {
                data->m_IBuffer[start + 8] = (byte)(value >> 8);
                data->m_IBuffer[start + 9] = (byte)(value & 0xff);
            }
        }

        public ushort AdditionalRRs
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 10] << 8) + data->m_IBuffer[start + 11]);
            }
            set
            {
                data->m_IBuffer[start + 10] = (byte)(value >> 8);
                data->m_IBuffer[start + 11] = (byte)(value & 0xff);
            }
        }

        public DNSQuestion[] Queries
        {
            get
            {
                DNSQuestion[] qs = new DNSQuestion[QuestionCount];
                int index = (int)start + 12;
                for (int x = 0; x < QuestionCount; x++)
                {
                    qs[x] = new DNSQuestion();
                    qs[x].Name = new List<byte>();
                    qs[x].Name.Add(data->m_IBuffer[index]);
                    index++;                    
                    while (data->m_IBuffer[index] != 0x00)
                    {
                        qs[x].Name.Add(data->m_IBuffer[index]);
                        index++;
                    }
                    qs[x].Name.Add(data->m_IBuffer[index]);
                    index++;
                    qs[x].Type = (ushort)(data->m_IBuffer[index] << 8);
                    index++;
                    qs[x].Type += data->m_IBuffer[index];
                    index++;
                    qs[x].Class = (ushort)(data->m_IBuffer[index] << 8);
                    index++;
                    qs[x].Class += data->m_IBuffer[index];
                    index++;
                }
                return qs;
            }
            set
            {
                DNSAnswer[] ans = null;
                if (AnswerRRs != 0)
                {
                    ans = Answers;
                }
                QuestionCount = (ushort)value.Length;
                int index = (int)start + 12;
                foreach (DNSQuestion q in value)
                {
                    for (int x = 0; x < q.Name.Count; x++)
                    {
                        data->m_IBuffer[index] = q.Name[x];
                        index++;
                    }                    

                    data->m_IBuffer[index] = (byte)(q.Type >> 8);
                    index++;
                    data->m_IBuffer[index] = (byte)(q.Type & 0xff);
                    index++;

                    data->m_IBuffer[index] = (byte)(q.Class >> 8);
                    index++;
                    data->m_IBuffer[index] = (byte)(q.Class & 0xff);
                    index++;
                }
                if (AnswerRRs != 0)
                {
                    Answers = ans;
                }
                else
                    data->m_Length = (uint)index;
            }
        }

        public class DNSQuestion
        {
            public List<byte> Name = new List<byte>();
            public ushort Type = 0;
            public ushort Class = 0;
            public byte ByteLength
            {
                get
                {
                    return (byte)(Name.Count + 2 + 2);
                }
            }

            public override string ToString()
            {
                string ret = "";
                int index = 0;
                int lengthToNextDot = Name[index];
                index++;
                while (Name[index] != 0x00)
                {
                    if (lengthToNextDot != 0)
                    {
                        ret += (char)Name[index];
                    }
                    else
                    {
                        ret += ".";
                        lengthToNextDot = Name[index] + 1;
                    }
                    index++;
                    lengthToNextDot--;
                }

                return ret;
            }
        }

        public DNSAnswer[] Answers
        {
            get
            {
                if (AnswerRRs == 0)
                    return null;
                int index = (int)start + 12;
                DNSQuestion[] qs = Queries;
                foreach (DNSQuestion q in qs)
                    index += q.ByteLength;
                DNSAnswer[] das = new DNSAnswer[AnswerRRs];
                for (int x = 0; x < AnswerRRs; x++)
                {
                    das[x] = new DNSAnswer();
                    das[x].Name = new List<byte>();
                    if ((data->m_IBuffer[index] & 0xc0) == 0xc0)
                    {
                        index++;
                        int tindex = (int)(start + data->m_IBuffer[index]);
                        das[x].Name.Add(data->m_IBuffer[tindex]);
                        tindex++;
                        while (data->m_IBuffer[tindex] != 0x00)
                        {
                            das[x].Name.Add(data->m_IBuffer[tindex]);
                            tindex++;
                        }
                        das[x].Name.Add(data->m_IBuffer[tindex]);
                    }
                    else
                    {
                        das[x].Name.Add(data->m_IBuffer[index]);
                        index++;
                        das[x].Name = new List<byte>();
                        while (data->m_IBuffer[index] != 0x00 && data->m_IBuffer[index] != 0xc0)
                        {
                            das[x].Name.Add(data->m_IBuffer[index]);
                            index++;
                        }
                        das[x].Name.Add(data->m_IBuffer[index]);
                    }
                    index++;
                    
                    das[x].Type = (ushort)(data->m_IBuffer[index] << 8);
                    index++;
                    das[x].Type += data->m_IBuffer[index];
                    index++;
                    
                    das[x].Class = (ushort)(data->m_IBuffer[index] << 8);
                    index++;
                    das[x].Class += data->m_IBuffer[index];
                    index++;

                    das[x].TTL = (UInt32)(data->m_IBuffer[index] << 24);
                    index++;
                    das[x].TTL = (UInt32)(data->m_IBuffer[index] << 16);
                    index++;
                    das[x].TTL = (UInt32)(data->m_IBuffer[index] << 8);
                    index++;
                    das[x].TTL += data->m_IBuffer[index];
                    index++;

                    das[x].RDLength = (ushort)(data->m_IBuffer[index] << 8);
                    index++;
                    das[x].RDLength += data->m_IBuffer[index];
                    index++;

                    das[x].RData = new byte[das[x].RDLength];
                    for (int i = 0; i < das[x].RDLength; i++)
                    {
                        das[x].RData[i] = data->m_IBuffer[index];
                        index++;
                    }
                }
                return das;
            }
            set 
            {
                int index = (int)start + 12;
                DNSQuestion[] qs = Queries;
                foreach (DNSQuestion q in qs)
                    index += q.ByteLength;
                if (value.Length == 0)
                {
                    data->m_Length = (uint)index;
                }
                else
                {
                    AnswerRRs = (ushort)value.Length;
                    foreach (DNSAnswer ans in value)
                    {
                        foreach (byte b in ans.Name)
                        {
                            data->m_IBuffer[index] = b;
                            index++;
                        }

                        data->m_IBuffer[index] = (byte)(ans.Type >> 8);
                        index++;
                        data->m_IBuffer[index] = (byte)(ans.Type & 0xff);
                        index++;

                        data->m_IBuffer[index] = (byte)(ans.Class >> 8);                        
                        index++;
                        data->m_IBuffer[index] = (byte)(ans.Class & 0xff);
                        index++;

                        data->m_IBuffer[index] = (byte)((ans.TTL >> 24) & 0xff);
                        index++;
                        data->m_IBuffer[index] = (byte)((ans.TTL >> 16) & 0xff);
                        index++;
                        data->m_IBuffer[index] = (byte)((ans.TTL >> 8) & 0xff);
                        index++;
                        data->m_IBuffer[index] = (byte)(ans.TTL & 0xff);
                        index++;

                        data->m_IBuffer[index] = (byte)((ans.RDLength >> 8) & 0xff);
                        index++;
                        data->m_IBuffer[index] = (byte)((ans.RDLength) & 0xff);
                        index++;

                        foreach (byte b in ans.RData)
                        {
                            data->m_IBuffer[index] = b;
                            index++;
                        }
                        data->m_Length = (uint)index;
                        length = (uint)(index - start);
                    }
                }
            }
        }

        public class DNSAnswer
        {
            public List<byte> Name = new List<byte>();
            public ushort Type = 0;
            public ushort Class = 0;
            public UInt32 TTL = 0;
            public ushort RDLength = 0;
            public byte[] RData;

            public override string ToString()
            {
                string ret = "";
                int index = 0;
                int lengthToNextDot = Name[index];
                index++;
                while (Name[index] != 0x00)
                {
                    if (lengthToNextDot != 0)
                    {
                        ret += (char)Name[index];
                    }
                    else
                    {
                        ret += ".";
                        lengthToNextDot = Name[index] + 1;
                    }
                    index++;
                    lengthToNextDot--;
                }

                return ret;
            }
        }

        public override uint LayerStart()
        {
            return start;
        }

        public override uint LayerLength()
        {
            return length;
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.DNS)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.DNS;
        }

        public override Packet MakeNextLayerPacket()
        {
            return this;
        }
    }
}
