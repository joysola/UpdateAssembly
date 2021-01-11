using System;

public class PageException
{
    public static int ERR_NON_BASE_FILE = 1001;
    public static int ERR_NON_SLIDE_DATA = 1002;

    public String Message { get; set; }

    public int ErrCode { get; set; } = 0;
}