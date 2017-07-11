using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeModulePackage {
	class Program {
		static void Main(string[] args) {
			var currentProject = args[0]; // e.g. Mindshift.SC.AutoPublish
			var configuration = args[1]; // e.g. Release

			// TODO: merge all the zips
			var archives = new List<ZipArchive>();

			var rootPath = @"C:\_Projects\Mindshift.SC\Main\"; // TODO: get this programably.

			archives.Add(new ZipArchive(File.Open(rootPath + @"Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Core.files.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + @"Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Core.scitems.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + @"Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Master.files.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + @"Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Master.scitems.update", FileMode.Open)));


			archives.Add(new ZipArchive(File.Open(rootPath + currentProject + @"\bin\Package_" + configuration + @"\" + currentProject + ".TDS.Core.files.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + currentProject + @"\bin\Package_" + configuration + @"\" + currentProject + ".TDS.Core.scitems.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + currentProject + @"\bin\Package_" + configuration + @"\" + currentProject + ".TDS.Master.files.update", FileMode.Open)));
			archives.Add(new ZipArchive(File.Open(rootPath + currentProject + @"\bin\Package_" + configuration + @"\" + currentProject + ".TDS.Master.scitems.update", FileMode.Open)));



			using (var memoryStream = new MemoryStream()) {
				using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true)) {
					var result = Merge(archives, archive);
				}
				// TODO: version? 
				using (var fileStream = new FileStream(rootPath + @"Mindshift.SC.AutoPublish.Master\bin\Package_Release\Mindshift.SC.AutoPublish.update", FileMode.Create)) {
					memoryStream.Seek(0, SeekOrigin.Begin);
					memoryStream.CopyTo(fileStream);
				}
			}

			//			- so write a program to take:
			//Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Core.files.update
			//Mindshift.SC.Common.TDS.Core\Package_Release\Mindshift.SC.Common.TDS.Core.scitems.update

			//And then add the ones from our current project:

		}


		public static ZipArchive Merge(List<ZipArchive> archives, ZipArchive archive) {
			if (archives == null) throw new ArgumentNullException("archives");
			if (archives.Count == 1) return archives.Single();

			foreach (var zipArchive in archives) {
				foreach (var zipArchiveEntry in zipArchive.Entries) {
					var file = archive.CreateEntry(zipArchiveEntry.FullName);

					using (var entryStream = file.Open()) {
						using (var streamWriter = new StreamWriter(entryStream)) { streamWriter.Write(zipArchiveEntry.Open()); }
					}
				}
			}

			return archive;
		}
	}

}
