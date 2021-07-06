using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Filepicker_Multi_Workaround
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenFileName
	{
		public int structSize = 0;
		public IntPtr dlgOwner = IntPtr.Zero;
		public IntPtr instance = IntPtr.Zero;

		public String filter = null;
		public String customFilter = null;
		public int maxCustFilter = 0;
		public int filterIndex = 0;

		public IntPtr file;
		public int maxFile = 0;

		public String fileTitle = null;
		public int maxFileTitle = 0;

		public String initialDir = null;

		public String title = null;

		public int flags = 0;
		public short fileOffset = 0;
		public short fileExtension = 0;

		public String defExt = null;

		public IntPtr custData = IntPtr.Zero;
		public IntPtr hook = IntPtr.Zero;

		public String templateName = null;

		public IntPtr reservedPtr = IntPtr.Zero;
		public int reservedInt = 0;
		public int flagsEx = 0;
	}

	enum OpenFileNameFlags
	{
		OFN_HIDEREADONLY = 0x4,
		OFN_FORCESHOWHIDDEN = 0x10000000,
		OFN_ALLOWMULTISELECT = 0x200,
		OFN_EXPLORER = 0x80000,
		OFN_FILEMUSTEXIST = 0x1000,
		OFN_PATHMUSTEXIST = 0x800
	}

	public class LibWrap
	{
		//BOOL GetOpenFileName(LPOPENFILENAME lpofn);

		[DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

		public static string[] ShowOpenFileDialog(string dialogTitle, string startPath, string filter, bool showHidden, bool allowMultiSelect)
		{
			const int MAX_FILE_LENGTH = 2048;

			OpenFileName ofn = new OpenFileName();

			ofn.structSize = Marshal.SizeOf(ofn);
			ofn.filter = filter;
			ofn.fileTitle = new String(new char[MAX_FILE_LENGTH]);
			ofn.maxFileTitle = ofn.fileTitle.Length;
			ofn.initialDir = startPath;
			ofn.title = dialogTitle;
			ofn.flags = (int)OpenFileNameFlags.OFN_HIDEREADONLY | (int)OpenFileNameFlags.OFN_EXPLORER | (int)OpenFileNameFlags.OFN_FILEMUSTEXIST | (int)OpenFileNameFlags.OFN_PATHMUSTEXIST;


			// Create buffer for file names
			ofn.file = Marshal.AllocHGlobal(MAX_FILE_LENGTH * Marshal.SystemDefaultCharSize);
			ofn.maxFile = MAX_FILE_LENGTH;

			// Initialize buffer with NULL bytes
			for (int i = 0; i < MAX_FILE_LENGTH * Marshal.SystemDefaultCharSize; i++)
			{
				Marshal.WriteByte(ofn.file, i, 0);
			}

			if (showHidden)
			{
				ofn.flags = ofn.flags | (int)OpenFileNameFlags.OFN_FORCESHOWHIDDEN;
			}

			if (allowMultiSelect)
			{
				ofn.flags = ofn.flags | (int)OpenFileNameFlags.OFN_ALLOWMULTISELECT;
			}

			if (GetOpenFileName(ofn))
			{
				List<string> selectedFilesList = new List<string>();

				IntPtr filePointer = ofn.file;

				long pointer = (long)filePointer;

				string file = Marshal.PtrToStringAuto(filePointer);

				// Retrieve file names
				while (file.Length > 0)
				{
					selectedFilesList.Add(file);

					pointer += file.Length * Marshal.SystemDefaultCharSize + Marshal.SystemDefaultCharSize;
					filePointer = (IntPtr)pointer;
					file = Marshal.PtrToStringAuto(filePointer);
				}

				if (selectedFilesList.Count == 1)
				{
					Marshal.FreeHGlobal(ofn.file);
					return selectedFilesList.ToArray();
				}
				else
				{
					// Multiple files selected, add directory
					string[] selectedFiles = new string[selectedFilesList.Count - 1];

					for (int i = 0; i < selectedFiles.Length; i++)
					{
						selectedFiles[i] = selectedFilesList[0];

						if (!selectedFiles[i].EndsWith("\\"))
						{
							selectedFiles[i] += "\\";
						}

						selectedFiles[i] += selectedFilesList[i + 1];
					}
					Marshal.FreeHGlobal(ofn.file);
					return selectedFiles;
				}
			}
			else
			{
				Marshal.FreeHGlobal(ofn.file);
				return null;
			}
		}
	}
}
