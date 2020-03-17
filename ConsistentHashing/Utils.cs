using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace ConsistentHashing
{
    public static class Utils
    {
        public static ulong MurmurHash<T>(this T obj)
        {
            byte[] bytes = obj.GetBytes();

            ulong seed = 1318007700;

            ulong m = 0xc6a4a7935bd1e995;
            int r = 47;
            ulong len = (ulong)bytes.Length;

            ulong h = seed ^ (len * m);

            int idx = 0;

            while(len >= 8)
            {
                ulong k = (ulong)(((ulong)bytes[idx++]) |
                                  ((ulong)bytes[idx++]) << 8 |
                                  ((ulong)bytes[idx++]) << 16 |
                                  ((ulong)bytes[idx++]) << 24 |
                                  ((ulong)bytes[idx++]) << 32 |
                                  ((ulong)bytes[idx++]) << 40 |
                                  ((ulong)bytes[idx++]) << 48 |
                                  ((ulong)bytes[idx++]) << 56);
                k *= m;
                k ^= k >> r;
                k *= m;

                h ^= k;
                h *= m;
                len -= 8;
            }

            switch(len & 7)
            {
                case 7:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8 |
                                 ((ulong)bytes[idx++]) << 16 |
                                 ((ulong)bytes[idx++]) << 24 |
                                 ((ulong)bytes[idx++]) << 32 |
                                 ((ulong)bytes[idx++]) << 48 |
                                 ((ulong)bytes[idx++]) << 56);
                    h *= m;
                    break;
                case 6:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8 |
                                 ((ulong)bytes[idx++]) << 16 |
                                 ((ulong)bytes[idx++]) << 24 |
                                 ((ulong)bytes[idx++]) << 32 |
                                 ((ulong)bytes[idx++]) << 48);
                    h *= m;
                    break;
                case 5:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8 |
                                 ((ulong)bytes[idx++]) << 16 |
                                 ((ulong)bytes[idx++]) << 24 |
                                 ((ulong)bytes[idx++]) << 32);
                    h *= m;
                    break;
                case 4:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8 |
                                 ((ulong)bytes[idx++]) << 16 |
                                 ((ulong)bytes[idx++]) << 24);
                    h *= m;
                    break;
                case 3:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8 |
                                 ((ulong)bytes[idx++]) << 16);
                    h *= m;
                    break;
                case 2:
                    h ^= (ulong)(((ulong)bytes[idx++]) |
                                 ((ulong)bytes[idx++]) << 8);
                    h *= m;
                    break;
                case 1:
                    h ^= ((ulong)bytes[idx++]);
                    h *= m;
                    break;
            }

            h ^= h >> r;
            h *= m;
            h ^= h >> r;


            Console.WriteLine("Bytes -> ");
            foreach (byte b in bytes)
                Console.Write(b);
            Console.WriteLine(" && Key -> " + h);

            return h;
        }

        public static byte[] GetBytes<T>(this T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);

                return ms.ToArray();
            }
        }
    }
}
