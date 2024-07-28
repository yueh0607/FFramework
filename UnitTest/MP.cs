using System.Text;
using FFramework;

namespace TestAA
{
    [FPackable]
    public partial class MyPack
    {
        public byte byteVal;
        public sbyte sbyteVal;
        public ushort ushortVal;
        public short shortVal;
        public uint uintVal;
        public int intVal;
        public ulong ulongVal;
        public long longVal;
        public bool boolVal;
        public float floatVal;
        public double doubleVal;
        public decimal decimalVal;
        public char charVal;
        public DateTime dateTimeVal;
    }

    [TestClass]
    public class MyPackTests
    {
        [TestMethod]
        public void TestSerializationAndDeserialization()
        {
            ThreadEnvirment envirment = new ThreadEnvirment(30);
            
           
            // Arrange
            DynamicSequence seq = new DynamicSequence();
            MyPack pack = new MyPack()
            {
                byteVal = 255,
                sbyteVal = -128,
                ushortVal = 65535,
                shortVal = -32768,
                uintVal = 4294967295,
                intVal = -2147483648,
                ulongVal = 18446744073709551615,
                longVal = -9223372036854775808,
                boolVal = true,
                floatVal = 3.14f,
                doubleVal = 2.71828,
                decimalVal = 79228162514264337593543950335m,
                charVal = 'A',
                dateTimeVal = new DateTime(2024, 7, 28)
            };

            // Act
            pack.Serialize(ref seq);
            byte[] source = seq.GetMergeSequence();
            StringBuilder builder = new StringBuilder();
            foreach (var item in source)
            {
                builder.Append(item);
            }
            string serializedData = builder.ToString();

            // Reset values
            pack.byteVal = 0;
            pack.sbyteVal = 0;
            pack.ushortVal = 0;
            pack.shortVal = 0;
            pack.uintVal = 0;
            pack.intVal = 0;
            pack.ulongVal = 0;
            pack.longVal = 0;
            pack.boolVal = false;
            pack.floatVal = 0f;
            pack.doubleVal = 0;
            pack.decimalVal = 0;
            pack.charVal = '\0';
            pack.dateTimeVal = DateTime.MinValue;

            pack.Deserialize(ref seq);

            // Assert
            Assert.AreEqual(255, pack.byteVal);
            Assert.AreEqual(-128, pack.sbyteVal);
            Assert.AreEqual(65535, pack.ushortVal);
            Assert.AreEqual(-32768, pack.shortVal);
            Assert.AreEqual(4294967295, pack.uintVal);
            Assert.AreEqual(-2147483648, pack.intVal);
            Assert.AreEqual(18446744073709551615, pack.ulongVal);
            Assert.AreEqual(-9223372036854775808, pack.longVal);
            Assert.AreEqual(true, pack.boolVal);
            Assert.AreEqual(3.14f, pack.floatVal);
            Assert.AreEqual(2.71828, pack.doubleVal);
            Assert.AreEqual(79228162514264337593543950335m, pack.decimalVal);
            Assert.AreEqual('A', pack.charVal);
            Assert.AreEqual(new DateTime(2024, 7, 28), pack.dateTimeVal);

            Console.WriteLine(serializedData);
            Console.WriteLine($"byteVal={pack.byteVal}, sbyteVal={pack.sbyteVal}, ushortVal={pack.ushortVal}, shortVal={pack.shortVal}, uintVal={pack.uintVal}, intVal={pack.intVal}, ulongVal={pack.ulongVal}, longVal={pack.longVal}, boolVal={pack.boolVal}, floatVal={pack.floatVal}, doubleVal={pack.doubleVal}, decimalVal={pack.decimalVal}, charVal={pack.charVal}, dateTimeVal={pack.dateTimeVal}");
        }
    }
}
