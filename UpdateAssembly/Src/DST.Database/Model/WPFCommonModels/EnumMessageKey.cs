using System.ComponentModel;

namespace DST.Database.WPFCommonModels
{
    public enum EnumMessageKey
    {
        /// <summary>
        /// 关闭登录界面
        /// </summary>
        [Description("关闭登录界面")]
        CloseLogin,

        /// <summary>
        /// 刷新当前位置信息
        /// </summary>
        [Description("刷新当前位置信息")]
        RefreshMenuInfo,

        /// <summary>
        /// 查看患者病例切片文件位置
        /// </summary>
        [Description("查看患者病例切片文件位置")]
        ViewFilePath,

        /// <summary>
        /// 是否展示返回按钮(同步展示切片)
        /// </summary>
        [Description("是否展示返回按钮(同步展示切片)")]
        ShowReturnButton,

        /// <summary>
        /// 外借编辑
        /// </summary>
        [Description("外借编辑")]
        LendOutEdit,

        /// <summary>
        /// 归还编辑
        /// </summary>
        [Description("归还编辑")]
        GiveBackEdit,

        /// <summary>
        /// 返回主页
        /// </summary>
		[Description("返回主页")]
        ShowHome,

        /// <summary>
        /// 切换焦点至看片，不选中病理档案
        /// </summary>
        [Description("切换焦点至看片")]
        UnSelectPathology,

        /// <summary>
        /// 批量外借
        /// </summary>
        [Description("批量外借")]
        BatchLendOut,

        /// <summary>
        /// 玻片信息列表改变
        /// </summary>
        [Description("玻片信息列表改变")]
        SlideInfoListChanged,

        /// <summary>
        /// 显示报告PDF文件
        /// </summary>
        [Description("显示报告PDF文件")]
        ShowReportPdf,

        /// <summary>
        /// 显示注销界面
        /// </summary>
        [Description("显示注销界面")]
        ShowLoginOut,

        /// <summary>
        /// 注销当前登录用户信息
        /// </summary>
        [Description("注销当前登录用户信息")]
        LoginOut,

        /// <summary>
        /// 打印报告
        /// </summary>
        [Description("打印报告")]
        PrintReport,

        /// <summary>
        /// 身份证输入
        /// </summary>
        IDCardChange,
    }
}