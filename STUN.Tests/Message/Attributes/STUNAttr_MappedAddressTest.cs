using NUnit.Framework;
using STUN.Message.Enums;
using System.Collections.Generic;
using System.Net;

namespace STUN.Message.Attributes {
	[TestFixture]
	public class STUNAttr_MappedAddressTest {
		[Test]
		public void FullTestIPv4() {
			byte[] expected = new byte[] {
				0x01, 0x01, 0x00, 0x0C, 0x21, 0x12, 0xA4, 0x42,
				0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
				0x08, 0x09, 0x0A, 0x0B, 0x00, 0x01, 0x00, 0x08,
				0x00, 0x01, 0x12, 0x34, 0x01, 0x02, 0x03, 0x04
			};

			IPAddress usedAddress = IPAddress.Parse("1.2.3.4");
			ushort usedPort = 4660;

			var msg = new STUNMessageBuilder(new byte[128],
				STUNClass.Success, STUNMethod.Binding,
				new Transaction(new byte[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
			msg.WriteAttribute(new STUNAttr_MappedAddress(usedAddress, usedPort));
			var stunReq = msg.Build();

			CollectionAssert.AreEqual(expected, stunReq.ToArray());

			var attrs = new List<STUNAttr>();
			var parser = new STUNMessageParser(stunReq, false, attrs);
			Assert.IsTrue(parser.isValid);

			STUNAttr_MappedAddress parsedAddr = new STUNAttr_MappedAddress();
			parsedAddr.ReadFromBuffer(attrs[0]);
			Assert.AreEqual(usedPort, parsedAddr.port, "Parser can't read the port correctly");
			Assert.IsTrue(parsedAddr.isIPv4());
			Assert.AreEqual(usedAddress, parsedAddr.ip.ToIPAddress(), "Parser can't read the address correctly");
		}

		[Test]
		public void FullTestIPv6() {
			// WRONG: regenerate
			byte[] expected = new byte[] {
				0x01, 0x01, 0x00, 0x18, 0x21, 0x12, 0xA4, 0x42,
				0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
				0x08, 0x09, 0x0A, 0x0B, 0x00, 0x01, 0x00, 0x14,
				0x00, 0x02, 0x12, 0x34, 0x20, 0x01, 0x0D, 0xB8,
				0x85, 0xA3, 0x00, 0x00, 0x00, 0x00, 0x8A, 0x2E,
				0x03, 0x70, 0x73, 0x34
			};

			IPAddress usedAddress = IPAddress.Parse("[2001:0db8:85a3:0000:0000:8a2e:0370:7334]");
			ushort usedPort = 4660;

			var msg = new STUNMessageBuilder(new byte[128],
				STUNClass.Success, STUNMethod.Binding,
				new Transaction(new byte[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
			msg.WriteAttribute(new STUNAttr_MappedAddress(usedAddress, usedPort));
			var stunReq = msg.Build();

			CollectionAssert.AreEqual(expected, stunReq.ToArray());

			var attrs = new List<STUNAttr>();
			var parser = new STUNMessageParser(stunReq, false, attrs);
			Assert.IsTrue(parser.isValid);

			STUNAttr_MappedAddress parsedAddr = new STUNAttr_MappedAddress();
			parsedAddr.ReadFromBuffer(attrs[0]);
			Assert.AreEqual(usedPort, parsedAddr.port, "Parser can't read the port correctly");
			Assert.IsFalse(parsedAddr.isIPv4());
			Assert.AreEqual(usedAddress, parsedAddr.ip.ToIPAddress(), "Parser can't read the address correctly");
		}
	}
}
