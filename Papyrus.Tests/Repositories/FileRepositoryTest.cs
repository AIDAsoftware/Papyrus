using System;
using System.IO;
using NUnit.Framework;

namespace Papyrus.Tests.Repositories {
    public class FileRepositoryTest {
        public readonly string WorkingDirectoryPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Documents"));

        [SetUp]
        public void CreateDirectory() {
            Directory.CreateDirectory(WorkingDirectoryPath);
        }

        [TearDown]
        public void DeleteDirectory() {
            Directory.Delete(WorkingDirectoryPath, true);
        }

        //TODO: move to a helper class
        protected static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }
    }
}