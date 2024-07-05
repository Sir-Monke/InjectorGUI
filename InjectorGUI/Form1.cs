using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InjectorGUI
{
    public partial class Injector : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        const uint PROCESS_CREATE_THREAD = 0x0002;
        const uint PROCESS_QUERY_INFORMATION = 0x0400;
        const uint PROCESS_VM_OPERATION = 0x0008;
        const uint PROCESS_VM_WRITE = 0x0020;
        const uint PROCESS_VM_READ = 0x0010;
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_READWRITE = 0x04;
        const uint MEM_RELEASE = 0x8000;
        const uint INFINITE = 0xFFFFFFFF;

        private string[] DllPaths;
        private int ProcId = 0;

        public Injector()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RandomBackgroundImage(sender);
            ListProcesses();
        }

        private void RandomBackgroundImage(object sender)
        {
            string imagesFolder = "InjectorGUI.Images.";
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var imageResources = resourceNames.Where(name => name.StartsWith(imagesFolder)).ToList();
            if (imageResources.Count > 0)
            {
                Random rand = new Random();
                int index = rand.Next(imageResources.Count);
                string selectedImageResource = imageResources[index];
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(selectedImageResource))
                {
                    if (stream != null)
                    {
                        this.BackgroundImage = Image.FromStream(stream);
                    }
                    else
                    {
                        MessageBox.Show("Could not find the specified image resource.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No images found in the specified folder.");
            }
        }

        private void AddDllToList()
        {
            string displayName;
            int index;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
                openFileDialog.Title = "Select a DLL file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fullPath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(fullPath);
                    string folderName = Path.GetFileName(Path.GetDirectoryName(fullPath));
                    displayName = $"{folderName}\\{fileName}";
                    index = DllSelector.Items.Count;
                    if (index >= 0 && index <= DllSelector.Items.Count)
                    {
                        DllSelector.Items.Insert(index, displayName);
                    }
                    else
                    {
                        MessageBox.Show("Invalid index", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RemoveSelectedDll()
        {
            if (DllSelector.SelectedIndex != -1)
            {
                DllSelector.Items.RemoveAt(DllSelector.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ListProcesses()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process proc in processes)
            {
                try
                {
                    if (proc.MainWindowHandle != IntPtr.Zero)
                    {
                        SelectProcess.Items.Add($"Process: {proc.ProcessName} - {proc.Id}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not retrieve info for process ID: {proc.Id}, Error: {ex.Message}");
                }
            }
        }

        private void InjectDll(int procId, string dllPath)
        {
            if (procId != 0)
            {
                IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, procId);
                if (hProcess != IntPtr.Zero)
                {
                    IntPtr lpBaseAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)dllPath.Length + 1, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                    if (lpBaseAddress != IntPtr.Zero)
                    {
                        byte[] dllPathBytes = Encoding.ASCII.GetBytes(dllPath);
                        if (WriteProcessMemory(hProcess, lpBaseAddress, dllPathBytes, (uint)dllPathBytes.Length + 1, out _))
                        {
                            IntPtr hKernel32 = GetModuleHandle("kernel32.dll");
                            IntPtr pLoadLibraryA = GetProcAddress(hKernel32, "LoadLibraryA");
                            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, pLoadLibraryA, lpBaseAddress, 0, out _);
                            if (hThread != IntPtr.Zero)
                            {
                                WaitForSingleObject(hThread, INFINITE);
                                CloseHandle(hThread);
                                MessageBox.Show("Injected");
                            }
                        }
                        VirtualFreeEx(hProcess, lpBaseAddress, 0, MEM_RELEASE);
                    }
                    CloseHandle(hProcess);
                }
            }
            else
            {
                MessageBox.Show("Proc Id is NULL.");
            }
        }

        private void AddDLLBtn_Click(object sender, EventArgs e)
        {
            AddDllToList();
        }

        private void RemoveDllBtn_Click(object sender, EventArgs e)
        {
            RemoveSelectedDll();
        }

        private void SelectProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = SelectProcess.SelectedItem.ToString();
        }
    }
}
