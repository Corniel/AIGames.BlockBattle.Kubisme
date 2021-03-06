﻿using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Deployoment
{
	[TestFixture, Category(Category.Deployment)]
	public class DeployerTest
	{
		[Test]
		public void Deploy_Cubism_CompileAndZip()
		{
			var dir = new FileInfo(GetType().Assembly.Location).Directory;
			var collectDir = new DirectoryInfo(Path.Combine(dir.FullName, @"../../../AIGames.BlockBattle.Kubisme"));
			var full = collectDir.FullName;
			var version = typeof(KubismeBot).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
			var nr = int.Parse(version.Split('.')[0]);
			Deployer.Run(collectDir, "Kubisme", nr.ToString("0000"), false);
		}
	}
}
