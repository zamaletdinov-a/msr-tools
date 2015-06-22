/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010-2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using MSR.Data.Entities.Mapping;
using MSR.Data.Entities.Mapping.PathSelectors;
using MSR.Tools.Mapper;
using MSR.Tools.StatGenerator;

namespace MSR.Tools.Debugger
{
	class Program
	{
		private static string configFile;

		static void Main(string[] args)
		{
			Console.BufferHeight = 10000;

			//configFile = @"E:\repo\gnome-terminal\gnome-terminal.config"; // 3436 revisions
			//configFile = @"E:\repo\dia\dia.config"; // 4384 revisions
			//configFile = @"E:\repo\gnome-vfs\gnome-vfs.config"; // 5550 revisions
			//configFile = @"E:\repo\gedit\gedit.config"; // 6556 revisions
			//configFile = @"E:\repo\git\git.config";
			//configFile = @"E:\repo\linux-2.6\linux-2.6.config";
			//configFile = @"E:\repo\django\django.config";
			//configFile = @"E:\repo\postgresql\postgresql.config";
			//configFile = @"E:\repo\nhibernate\nhibernate.config";
			//configFile = @"E:\repo\msr\msr.config";
            configFile = @"D:\ProjectsFromGit\MSRGit\couchdb.config"; // 2000 revisions //13998 revisions
			//configFile = @"E:\repo\frund\frund.config";
			//configFile = @"E:\repo\httpd\httpd.config";
			//configFile = @"E:\repo\subtle\subtle.config";
			//configFile = @"E:\repo\hc\hc.config";
			//configFile = @"E:\repo\pgadmin3\pgadmin3.config";
			//configFile = @"E:\repo\gnuplot\gnuplot.config";
			//configFile = @"E:\repo\couchdb\couchdb.config";
			//configFile = @"E:\repo\dovecot\dovecot.config";

			//Debug();
			Mapping();
			//PartialMapping();
			//MapReleases();
			//MapSyntheticReleases(5, 0.6);
			//GenerateStat();

			Console.ReadKey();
		}
		static void Debug()
		{
			DebuggingTool debugger = new DebuggingTool(configFile);
			debugger.FindDiffError(1, "/sha1_file.c");
		}
		static void Mapping()
		{
			MappingTool mapper = new MappingTool(configFile);

			mapper.Info();
		    //mapper.Map(false, 2000);
			//mapper.Truncate(10);
			//mapper.Check(2000);
		}
		static void PartialMapping()
		{
			MappingTool mapper = new MappingTool(configFile);

			mapper.PartialMap(1901,
				new IPathSelector[]
				{
					new TakePathByList()
					{
						Paths = new string[] { "/src/gpexecute.inc" }
					}
				}
			);
		}
		static void MapReleases()
		{
			MappingTool mapper = new MappingTool(configFile);
			mapper.MapReleases(
				new Dictionary<string,string>()
				{
					{ "e8b5079917c0734637e444018c304ea057ad7f32", "Synthetic Release # 1" },
					{ "37541bbf91d09ad0200f2c26c7a3f79fdf050801", "Synthetic Release # 2" },
					{ "2e3d4ea2bdb8515772a198a863575ddbd32fd09c", "Synthetic Release # 3" },
					{ "6e512ec75fba7372babb04988c6cbf5197a26438", "Synthetic Release # 4" },
					{ "4137a8eb9b2bef436a7e975f4ffe58af49072d94", "Synthetic Release # 5" },
				}
			);
		}
		static void MapSyntheticReleases(int count, double stabilizationProbability)
		{
			MappingTool mapper = new MappingTool(configFile);
			mapper.MapSyntheticReleases(count, stabilizationProbability);
		}
		static void GenerateStat()
		{
			GeneratingTool generator = new GeneratingTool(configFile);
			generator.GenerateStat(null, "d:/temp/1", null);
		}
	}
}
