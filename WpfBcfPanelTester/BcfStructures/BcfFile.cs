using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Bimbot.Bcf.Bcf2;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Bimbot.Bcf
{
   public class BcfFile
   {
      public ZipArchive archive;

      public bool isValidBcf { get; private set; }
      
      public ProjectExtension project { get; private set; }
      public Version version { get; private set; }
 
      public Dictionary<string, Markup> markups { get; private set; }
      public Dictionary<string, ViewPoint> viewpoints { get; private set; }
//      public Dictionary<string, Bitmap> issues { get; private set; }

      // Constructor
      public BcfFile(byte[] zippedBytes)
      {
         markups = new Dictionary<string, Markup>();
         isValidBcf = false;

         if (zippedBytes == null)
            return;

         Stream zippedStream = new MemoryStream(zippedBytes);
         try
         {
            archive = new ZipArchive(zippedStream);

            if (archive == null)
               return;

            // Get root data bcf (project and version)
            foreach (ZipArchiveEntry file in archive.Entries)
            {
               // project file
               if (file.Name.Equals("ProjectInfo.bcfp"))
                  project = DeserializeProject(file.Open());

               // version file
               else if (file.Name.Equals("bcf.version"))
                  version = DeserializeVersion(file.Open());
            }

            // ADD ISSUES FOR EACH SUBFOLDER containing a markup.bcf file
            foreach (ZipArchiveEntry file in archive.Entries)
            {
               // Issues
               if (!file.Name.Equals("markup.bcf"))
                  continue;

               string issueId = file.FullName.Substring(0, file.FullName.IndexOf('/'));

               var bcfissue = DeserializeMarkup(file.Open());
               markups.Add(issueId, bcfissue);
            }

            // Add viewpoints to issues
            foreach (ZipArchiveEntry file in archive.Entries)
            {
               // skip files that are not viewpoints
               if (!file.Name.EndsWith(".bcfv"))
                  continue;

               // Find the id
               string issueId = file.FullName.Substring(0, file.FullName.IndexOf('/'));
               if (!markups.ContainsKey(issueId))
                  continue;

               Markup issue = markups[issueId];
               if (issue.Viewpoints == null) //bcf 1.0
               {
                  issue.Viewpoints = new ViewPoint[1];
                  issue.Viewpoints[0] = new ViewPoint();
                  issue.Viewpoints[0].Viewpoint = file.Name;
                  issue.Viewpoints[0].ViewpointRef = DeserializeViewpoint(file.Open());
                  issue.Viewpoints[0].Snapshot = null;
               }
               else
               {
                  foreach (var vp in issue.Viewpoints)
                  {
                     if (vp.Viewpoint.Equals(file.Name))
                     {
                        vp.Viewpoint = file.Name;
                        vp.ViewpointRef = DeserializeViewpoint(file.Open());
                        break;
                     }
                  }
               }
            }

            // Add snapshots to viewpoints of snapshots
            foreach (ZipArchiveEntry file in archive.Entries)
            {
               // skip files that are not viewpoints
               if (!file.Name.EndsWith(".png"))
                  continue;

               // Find the id
               string issueId = file.FullName.Substring(0, file.FullName.IndexOf('/'));
               if (!markups.ContainsKey(issueId))
                  continue;

               Markup issue = markups[issueId];
               if (issue.Viewpoints.Length == 1 && issue.Viewpoints[0].Snapshot == null)
               {
                  issue.Viewpoints[0].Snapshot = file.Name;
                  issue.Viewpoints[0].SnapshotRef = new BitmapImage();
                  issue.Viewpoints[0].SnapshotRef.BeginInit();
                  issue.Viewpoints[0].SnapshotRef.StreamSource = file.Open();
                  issue.Viewpoints[0].SnapshotRef.CacheOption = BitmapCacheOption.OnLoad;
                  issue.Viewpoints[0].SnapshotRef.EndInit();

               }
               else
               {
                  foreach (ViewPoint vp in issue.Viewpoints)
                  {
                     if (vp.Snapshot.Equals(file.Name))
                     {
                        vp.SnapshotRef = new BitmapImage();
                        vp.SnapshotRef.BeginInit();
                        vp.SnapshotRef.StreamSource = file.Open();
                        vp.SnapshotRef.CacheOption = BitmapCacheOption.OnLoad;
                        vp.SnapshotRef.EndInit();
                        break;
                     }
                  }
               }
            }
            isValidBcf = true;
         }
         catch (System.Exception e)
         {
            MessageBox.Show("Invalid data in bcf byte buffer (possibly incomplete)");
         }
      }

      private static ProjectExtension DeserializeProject(Stream data)
      {
         ProjectExtension output = null;
         try
         {
            MemoryStream ms = ToMemoryStream(data);
            ms.Position = 0;

            var serializerM = new XmlSerializer(typeof(ProjectExtension));
            output = serializerM.Deserialize(ms) as ProjectExtension;
         }
         catch (System.Exception ex1)
         {
            MessageBox.Show("exception: " + ex1);
         }
         return output;
      }

      private static Version DeserializeVersion(Stream data)
      {
         Version output = null;
         try
         {
            MemoryStream ms = ToMemoryStream(data);
            ms.Position = 0;

            var serializerM = new XmlSerializer(typeof(Version));
            output = serializerM.Deserialize(ms) as Version;
         }
         catch (System.Exception ex1)
         {
            MessageBox.Show("exception: " + ex1);
         }
         return output;
      }

      private static Markup DeserializeMarkup(Stream data)
      {
         Markup output = null;
         try
         {
            MemoryStream ms = ToMemoryStream(data);
            ms.Position = 0;

            var serializerM = new XmlSerializer(typeof(Markup));
            output = serializerM.Deserialize(ms) as Markup;
         }
         catch (System.Exception ex1)
         {
            MessageBox.Show("exception: " + ex1);
         }
         return output;
      }

      private static MemoryStream ToMemoryStream(Stream stream)
      {
         MemoryStream ms = new MemoryStream();
         {
            int read;
            byte[] buffer = new byte[16 * 1024];
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
               ms.Write(buffer, 0, read);
            }
         }
         return ms;
      }

      private static VisualizationInfo DeserializeViewpoint(Stream data)
      {
         VisualizationInfo output = null;
         try
         {
            MemoryStream ms = ToMemoryStream(data);
            ms.Position = 0;

            var serializerS = new XmlSerializer(typeof(VisualizationInfo));
            output = serializerS.Deserialize(ms) as VisualizationInfo;
         }
         catch (System.Exception ex1)
         {
            MessageBox.Show("exception: " + ex1);
         }
         return output;
      }
   }
}