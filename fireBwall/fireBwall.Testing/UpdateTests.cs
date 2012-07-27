using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using fireBwall.Updates;

namespace fireBwall.Testing
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void GetAPIVersion()
        {
            UpdateChecker uc = new UpdateChecker();
            uc.UpdateFirebwallMetaVersion();
            Assert.IsTrue(UpdateChecker.availableFirebwall != null);
        }
    }
}
