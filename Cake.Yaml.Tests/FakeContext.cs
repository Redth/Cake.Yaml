﻿using System;
using Cake.Core.IO;
using Cake.Core;
using System.Collections.Generic;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Cake.Yaml.Tests
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;


        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath(System.IO.Path.GetFullPath(AppContext.BaseDirectory));

            var environment = Cake.Testing.FakeEnvironment.CreateUnixEnvironment (false);

            var fileSystem = new Cake.Testing.FakeFileSystem (environment);
            var globber = new Globber (fileSystem, environment);
            log = new FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var registry = new WindowsRegistry ();

            var toolLocator = new ToolLocator(environment, new ToolRepository(environment), new ToolResolutionStrategy(fileSystem, environment, globber, new FakeConfiguration()));
            context = new CakeContext(fileSystem, environment, globber, log, args, processRunner, registry, toolLocator);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }

        public ICakeContext CakeContext {
            get { return context; }
        }

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, log.Entries);
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Entries)
                Console.WriteLine (m);
        }
    }
}
