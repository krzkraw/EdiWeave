﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdiWeave.Core.Model.Edi;
using EdiWeave.Core.Model.Edi.ErrorContexts;
using EdiWeave.Core.Model.Edi.X12;
using EdiWeave.Framework;
using EdiWeave.Framework.Readers;
using EdiWeave.Rules.HIPAA_005010X222A1_837;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdiWeave.UnitTests.Hipaa
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Test4010()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00401.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            var expected = CommonHelper.LoadString(sample);
            List<EdiItem> ediItems;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
            }
            var actual = X12Helper.Generate(ediItems, null, Environment.NewLine);

            // ASSERT
            Assert.IsNotNull(ediItems);
            Assert.IsNotNull(ediItems.OfType<ISA>().SingleOrDefault());
            Assert.IsNotNull(ediItems.OfType<GS>().SingleOrDefault());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test5010()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00501.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            var expected = CommonHelper.LoadString(sample);
            List<EdiItem> ediItems;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
            }
            var actual = X12Helper.Generate(ediItems, null, Environment.NewLine);

            // ASSERT
            Assert.IsNotNull(ediItems);
            Assert.IsNotNull(ediItems.OfType<ISA>().SingleOrDefault());
            Assert.IsNotNull(ediItems.OfType<GS>().SingleOrDefault());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test5010Isa()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00501_ISA.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            var expected = CommonHelper.LoadString(sample);
            List<EdiItem> ediItems;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
            }
            var actual = X12Helper.Generate(ediItems, null, Environment.NewLine);

            // ASSERT
            Assert.IsNotNull(ediItems);
            Assert.IsNotNull(ediItems.OfType<ISA>().SingleOrDefault());
            Assert.IsNotNull(ediItems.OfType<GS>().SingleOrDefault());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test5010Hl()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00501_HL.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            var expected = CommonHelper.LoadString(sample);
            List<EdiItem> ediItems;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
            }
            var actual = X12Helper.Generate(ediItems, null, Environment.NewLine);

            // ASSERT
            Assert.IsNotNull(ediItems);
            Assert.IsNotNull(ediItems.OfType<ISA>().SingleOrDefault());
            Assert.IsNotNull(ediItems.OfType<GS>().SingleOrDefault());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test5010Lfnm1Lisa()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00501_LF.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            var expected = CommonHelper.LoadString(sample);
            List<EdiItem> ediItems;
            Separators separators;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
                separators = ediReader.Separators;
            }
            var actual = X12Helper.Generate(ediItems, separators, "");

            // ASSERT
            Assert.IsNotNull(ediItems);
            Assert.IsNotNull(ediItems.OfType<ISA>().SingleOrDefault());
            Assert.IsNotNull(ediItems.OfType<GS>().SingleOrDefault());
            Assert.AreEqual(expected, actual);
        }

        private Assembly HipaaFactory(MessageContext mc)
        {
            if (mc.Version == "005010X222A1")
                return Assembly.Load(new AssemblyName("EdiWeave.Rules.Hipaa005010"));
            if (mc.Version == "004010X098A1")
                return Assembly.Load(new AssemblyName("EdiWeave.Rules.Hipaa004010"));

            throw new Exception("Not supported!");
        }

        [TestMethod]
        public void Test5010WithCustomValidation()
        {
            // ARRANGE
            const string sample = "EdiWeave.UnitTests.Hipaa.Edi.Hipaa_837P_00501_Custom.txt";
            var ediStream = CommonHelper.LoadStream(sample);
            List<EdiItem> ediItems;

            // ACT
            using (var ediReader = new X12Reader(ediStream, HipaaFactory))
            {
                ediItems = ediReader.ReadToEnd().ToList();
            }
            var msg = ediItems.OfType<TS837>().Single();
            msg.IsValid(out var ec);

            // ASSERT
            Assert.IsNotNull(ec);
            Assert.IsNotNull(ec.Errors.SingleOrDefault(e => e.Name == "CustomValidationSegment"));
        }
    }
}
