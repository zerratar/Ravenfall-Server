using GameServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace RavenNet.Tests
{

    [TestClass]
    public class PacketTests
    {
        [TestMethod]
        public void CheckForPacketOpCodeCollisions()
        {
            var opcodes = new HashSet<short>();
            var assembly = Assembly.GetAssembly(typeof(MyPlayerAdd));
            var packetTypes = assembly.GetTypes().Where(x => x.FullName.Contains("Shinobytes.Ravenfall.RavenNet.Packets"));
            foreach (var packetType in packetTypes)
            {
                var opCodeField = packetType.GetField("OpCode");
                if (opCodeField == null) continue;
                var opCodeValue = (short)opCodeField.GetValue(null);
                if (!opcodes.Add(opCodeValue))
                {
                    Assert.Fail($"Duplicate opcode {opCodeValue} found in {packetType.FullName}");
                }
            }
        }
    }

    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void TestUserPlayerList()
        {
            var payload = new byte[] { 1, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 1, 6, 97, 115, 100, 97, 115, 100, 1, 5, 101, 103, 104, 97, 104, 1, 5, 52, 52, 52, 52, 52, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 21, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 1, 255, 255, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 17, 0, 0, 0, 7, 0, 0, 0, 1, 0, 255, 255, 1, 7, 35, 100, 54, 98, 56, 97, 101, 1, 255, 255, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 22, 0, 0, 0, 15, 0, 0, 0, 2, 0, 0, 0, 255, 255, 255, 255, 1, 255, 255, 2, 0, 0, 0, 3, 0, 0, 0, 1, 0, 1, 0, 1, 0, };

            var targetType = typeof(Shinobytes.Ravenfall.RavenNet.Packets.Client.UserPlayerList);
            var serializer = new BinarySerializer();
            var data = serializer.Deserialize(payload, targetType);

        }

        [TestMethod]
        public void TestBadMyPlayerAdd()
        {
            var payload = new byte[] { 1, 1, 1, 7, 35, 65, 56, 57, 49, 50, 65, 6, 0, 0, 0, 1, 7, 35, 48, 48, 48, 48, 48, 48, 255, 255, 255, 255, 1, 0, 0, 0, 15, 0, 0, 0, 1, 7, 35, 56, 101, 52, 52, 97, 100, 2, 0, 0, 0, 1, 10, 0, 0, 0, 1, 7, 35, 100, 54, 98, 56, 97, 101, 1, 7, 35, 100, 54, 98, 56, 97, 101, 1, 7, 35, 56, 101, 52, 52, 97, 100, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 7, 0, 0, 0, 1, 0, 0, 0, 1, 3, 122, 101, 114, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
            var targetType = typeof(MyPlayerAddTmp);
            var serializer = new BinarySerializer();
            var data = serializer.Deserialize(payload, targetType);

        }
        public class MyPlayerAddTmp
        {
            public const short OpCode = 16;
            public int PlayerId { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public decimal Experience { get; set; }
            public int Health { get; set; }
            public Vector3 Position { get; set; }
            public Professions Professions { get; set; }
            public Attributes Attributes { get; set; }
            public Appearance Appearance { get; set; }
            public int[] InventoryItemId { get; set; }
            public long[] InventoryItemAmount { get; set; }
            public long Coins { get; set; }
        }

        [TestMethod]
        public void TestComplexArraySerialization()
        {
            var serializer = new BinarySerializer();
            var lookup = new NetworkPacketTypeRegistry();
            lookup.Register<MyPlayerAdd>(MyPlayerAdd.OpCode);

            var packetSerializer = new NetworkPacketSerializer(null, lookup, serializer);

            var packet = new NetworkPacket();
            packet.Id = MyPlayerAdd.OpCode;

            packet.Data = new MyPlayerAdd
            {
                Health = 10,
                Coins = 0,
                Experience = 0,
                Level = 1,
                Name = "Zerratar",
                PlayerId = 10,
                Position = new Shinobytes.Ravenfall.RavenNet.Models.Vector3(1.1255f, 7.5f, 6f),
                Professions = new Professions(),
                Appearance = GenerateRandomAppearance(),
                Attributes = new Attributes(),
                InventoryItemAmount = new long[] { 1, 1, 1 },
                InventoryItemId = new int[] { 1, 2, 3 }
            };

            var data = packetSerializer.Serialize(packet);
            var result = packetSerializer.Deserialize(data);


            if (!result.TryGetValue<MyPlayerAdd>(out var resultData))
            {
                Assert.Fail("Resulting data was not of the expected type MyPlayerAdd");
                return;
            }

            //Assert.AreEqual(appearance.Length, resultData.Appearance.Length);
            //Assert.AreEqual(id.Length, resultData.Id.Length);
            //Assert.AreEqual(combatLevel.Length, resultData.Level.Length);
            //Assert.AreEqual(name.Length, resultData.Name.Length);

        }
        //[TestMethod]
        //public void TestComplexArraySerialization()
        //{
        //    var serializer = new BinarySerializer();
        //    var lookup = new NetworkPacketTypeRegistry();
        //    lookup.Register<UserPlayerList>(UserPlayerList.OpCode);

        //    var packetSerializer = new NetworkPacketSerializer(null, lookup, serializer);

        //    var packet = new NetworkPacket();
        //    packet.Id = UserPlayerList.OpCode;

        //    var name = new string[] { "Zerratar", "Zerratar2" };
        //    var id = new int[] { 1, 2 };
        //    var combatLevel = new int[] { 3, 3 };
        //    var appearance = new Appearance[] {
        //        GenerateRandomAppearance(),
        //        GenerateRandomAppearance()
        //    };

        //    packet.Data = new UserPlayerList
        //    {
        //        Appearance = appearance,
        //        Id = id,
        //        Name = name,
        //        Level = combatLevel
        //    };

        //    var data = packetSerializer.Serialize(packet);
        //    var result = packetSerializer.Deserialize(data);


        //    if (!result.TryGetValue<UserPlayerList>(out var resultData))
        //    {
        //        Assert.Fail("Resulting data was not of the expected type UserPlayerList");
        //        return;
        //    }

        //    Assert.AreEqual(appearance.Length, resultData.Appearance.Length);
        //    Assert.AreEqual(id.Length, resultData.Id.Length);
        //    Assert.AreEqual(combatLevel.Length, resultData.Level.Length);
        //    Assert.AreEqual(name.Length, resultData.Name.Length);

        //}


        private Appearance GenerateRandomAppearance()
        {
            var gender = Utility.Random<Gender>();
            var skinColor = Utility.Random("#d6b8ae");
            var hairColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            var beardColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            return new Appearance
            {
                Gender = gender,
                SkinColor = skinColor,
                HairColor = hairColor,
                BeardColor = beardColor,
                StubbleColor = skinColor,
                WarPaintColor = hairColor,
                EyeColor = Utility.Random("#000000", "#c0392b", "#2c3e50"),
                Eyebrows = Utility.Random(0, gender == Gender.Male ? 10 : 7),
                Hair = Utility.Random(0, 38),
                FacialHair = gender == Gender.Male ? Utility.Random(0, 18) : -1,
                Head = Utility.Random(0, 23),
                HelmetVisible = true
            };
        }


        [TestMethod]
        public void TestBadSerialization()
        {
            //var payload = new byte[] { 1, 3, 0, 0, 0, 1, 10, 112, 108, 97, 121, 101, 114, 51, 55, 54, 49, 0, 35, 208, 146, 63, 87, 22, 133, 63, 48, 213, 164, 63, };

            //var targetType = typeof(PlayerAdd);
            //var serializer = new BinarySerializer();
            //var data = serializer.Deserialize(payload, targetType);



            //var payload = new byte[] { 1, 10, 0, 0, 0, 1, 10, 112, 108, 97, 121, 101, 114, 55, 50, 52, 54, 0, 123, 136, 20, 64, 87, 22, 133, 63, 75, 87, 173, 64, 64, 126, 191, 64, 162, 61, 15, 66, 1, 77, 206, 88, 64, 233, 121, 246, 164, 50, 12, 154, 64, 199, 93, 188, 64, 222, 153, 10, 66, };

            //var targetType = typeof(PlayerAdd);
            //var serializer = new BinarySerializer();
            //var data = serializer.Deserialize(payload, targetType);

        }

        [TestMethod]
        public void SerializeDeserializeFullPacket()
        {
            var serializer = new BinarySerializer();
            var lookup = new NetworkPacketTypeRegistry();
            lookup.Register<Test>(1);

            var packetSerializer = new NetworkPacketSerializer(null, lookup, serializer);

            var packet = new NetworkPacket();
            packet.Id = 1;
            packet.Data = new Test();
            var data = packetSerializer.Serialize(packet);
            var result = packetSerializer.Deserialize(data);
            Assert.AreEqual(typeof(Test).FullName, result.Data.GetType().FullName);
            Assert.AreEqual("Hello, world!", ((Test)result.Data).Value);
        }

    }

    public class Test
    {
        public string Value { get; set; } = "Hello, world!";
    }


    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        public float SqrtMagnitude => X * X + Y * Y + Z * Z;

        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X - b.X,
            Y = a.Y - b.Y,
            Z = a.Z - b.Z
        };

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X + b.X,
            Y = a.Y + b.Y,
            Z = a.Z + b.Z
        };

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Magnitude;
        }

        public override string ToString()
        {
            return $"{{x: {X}, y: {Y}, z: {Z}}}";
        }
    }
    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
    }

}
