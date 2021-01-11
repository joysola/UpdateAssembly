using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace TileImageViewer.Helpers
{
    internal class NetShareFolderEnmu
    {
        [StructLayout(LayoutKind.Sequential)]
        protected struct SHARE_INFO_1
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi1_netname;

            [MarshalAs(UnmanagedType.U4)]
            public uint shi1_type;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi1_remark;
        }

        [DllImport("Netapi32.dll", EntryPoint = "NetShareEnum")]
        protected static extern int NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.U4)] uint level, out IntPtr bufptr,
                                                 [MarshalAs(UnmanagedType.U4)] int prefmaxlen, [MarshalAs(UnmanagedType.U4)] out uint entriesread,
                                                 [MarshalAs(UnmanagedType.U4)] out uint totalentries, [MarshalAs(UnmanagedType.U4)] out uint resume_handle);

        public static string[] NetShareList(string server)
        {
            IntPtr buffer;
            uint entriesread;
            uint totalentries;
            uint resume_handle;

            //-1应该是获取所有的share,msdn里面的例子是这么写的,返回0表示成功
            if (NetShareEnum(server, 1, out buffer, -1, out entriesread, out totalentries, out resume_handle) == 0)
            {
                Int32 ptr = buffer.ToInt32();
                ArrayList alShare = new ArrayList();
                for (int i = 0; i < entriesread; i++)
                {
                    SHARE_INFO_1 shareInfo = (SHARE_INFO_1)Marshal.PtrToStructure(new IntPtr(ptr), typeof(SHARE_INFO_1));
                    if (shareInfo.shi1_type == 0)//Disk drive类型
                    {
                        alShare.Add(shareInfo.shi1_netname);
                    }
                    ptr += Marshal.SizeOf(shareInfo);
                }
                string[] share = new string[alShare.Count];
                for (int i = 0; i < alShare.Count; i++)
                {
                    share[i] = alShare[i].ToString();
                }
                return share;
            }
            else
                return null;
        }
    }
}