using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace OpenTap.Package.PackageInstallHelpers
{
    internal class PackageInstallStep : TestStep
    {
        public string Target { get; set; }
        public PackageDef[] Packages { get; set; }
        public bool Force { get; set; }

        public override void Run()
        {
            var repositories = new HashSet<string>();

            foreach (var pkg in Packages)
            {
                switch (pkg.PackageSource)
                {
                    case IRepositoryPackageDefSource s1:
                        repositories.Add(s1.RepositoryUrl);
                        break;
                    case IFilePackageDefSource s2:
                        repositories.Add(Path.GetDirectoryName(s2.PackageFilePath));
                        break;
                }
            }

            var action = new PackageInstallAction()
            {
                InstallDependencies = false,
                IgnoreDependencies = true,
                NonInteractive = true,
                Force = Force,
                PackageReferences = Packages.Select(p => new PackageSpecifier(p.Name,
                        new VersionSpecifier(p.Version, VersionMatchBehavior.Exact), p.Architecture, p.OS))
                    .ToArray(),
                Target = Target,
                Repository = repositories.ToArray()
            };

            try
            {
                using var fileLock = FileLock.Create(Target);
                if (fileLock.WaitOne(TimeSpan.FromMinutes(1)) == false)
                    throw new TimeoutException($"Operation timed out while waiting for an exclusive lock on directory '{Target}'.");
                var result = action.Execute(CancellationToken.None);
                UpgradeVerdict(result == 0 ? Verdict.Pass : Verdict.Fail);
            }
            catch
            {
                UpgradeVerdict(Verdict.Error);
            }
        }
    }
}