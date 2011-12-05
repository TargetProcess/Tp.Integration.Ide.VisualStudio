using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Tp.Integration.Ide.VisualStudio.Services;

namespace Tp.Integration.Ide.VisualStudio.Utils {
	[TestFixture]
	public class SerializerTest {
		[Test]
		public void Test() {
			var sw = new StringWriter();
			var timeTracking1 = new TimeTracking {
				Records = new List<TimeRecord> {
					new TimeRecord(1, 6.HoursAgo(), 5.HoursAgo()),
					new TimeRecord(3, 5.HoursAgo(), 3.HoursAgo()),
					new TimeRecord(4, 3.HoursAgo(), 1.HoursAgo()),
				}
			};
			Serializer.Serialize(timeTracking1, sw);
			var timeTracking2 = Serializer.Deserialize<TimeTracking>(new StringReader(sw.ToString()));
			Assert.AreEqual(timeTracking1.ToString(), timeTracking2.ToString());
		}
	}

	public static class DateTimeExtensions {
		public static DateTime DaysAgo(this int days) {
			return DateTime.Now.Subtract(days.Days());
		}

		public static DateTime DaysAfter(this int days) {
			return DateTime.Now.Add(days.Days());
		}

		public static DateTime HoursAgo(this int Hours) {
			return DateTime.Now.Subtract(Hours.Hours());
		}

		public static DateTime HoursAfter(this int Hours) {
			return DateTime.Now.Add(Hours.Hours());
		}

		public static DateTime MinutesAgo(this int Minutes) {
			return DateTime.Now.Subtract(Minutes.Minutes());
		}

		public static DateTime MinutesAfter(this int Minutes) {
			return DateTime.Now.Add(Minutes.Minutes());
		}

		public static DateTime SecondsAgo(this int Seconds) {
			return DateTime.Now.Subtract(Seconds.Seconds());
		}

		public static DateTime SecondsAfter(this int Seconds) {
			return DateTime.Now.Add(Seconds.Seconds());
		}

		public static TimeSpan Days(this int days) {
			return new TimeSpan(days, 0, 0, 0);
		}

		public static TimeSpan Hours(this int hours) {
			return new TimeSpan(0, hours, 0, 0);
		}

		public static TimeSpan Minutes(this int minutes) {
			return new TimeSpan(0, 0, minutes, 0);
		}

		public static TimeSpan Seconds(this int seconds) {
			return new TimeSpan(0, 0, 0, seconds);
		}
	}
}